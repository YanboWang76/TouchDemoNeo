using System;
using Libdexmo.Client;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.Utility;
using Libdexmo.Unity.HandController;
using UnityEngine;

namespace HackInDexmo.Scripts
{
    
    public class ReleasePoseEventArgs : EventArgs
    {
        public bool isRight { get; private set; }

        public ReleasePoseEventArgs(bool isRight)
        {

            this.isRight = isRight;
        }
    }
    public class BypassController : MonoBehaviour
    {
        public DexmoController controller;
        public UnityHandPoseManager leftPoseManager;
        public UnityHandPoseManager rightPoseManager;

        private bool ReleaseFlagLeft = false;
        private bool ReleaseFlagLeftBefore = false;
        private bool ReleaseFlagRight = false;
        private bool ReleaseFlagRightBefore = false;

        public event EventHandler<ReleasePoseEventArgs> ReleasePose;

        private const float EPSILON = 0.1f;
        private const bool RIGHT = true;
        private const bool LEFT = false;
    
        private Controller ClientController;
        private float[] currentPosition;
        private class ForceFeedbackInfo
        {
            public bool sustaining;
            public float offset;
            public float positionSetPoint;
            public float stiffness;
            public bool inwardControl;

            public ForceFeedbackInfo(bool sustaining = false, float offset = 0, float positionSetPoint = 0, float stiffness = 0, bool inwardControl = true)
            {
                this.sustaining = sustaining;
                this.offset = offset;
                this.positionSetPoint = positionSetPoint;
                this.stiffness = stiffness;
                this.inwardControl = inwardControl;
            }


        }

        private void UpdateCurrentPosition()
        {
            for (int i = 0; i < 10; i++)
            {
                currentPosition[i] = getCurrentPosition(i / 5, i % 5);
            }
        }
        private void CheckRelease()
        {
            ReleaseFlagRight = true;
            for (int i = 0; i < 5; i++)
            {
                if (currentPosition[i] > EPSILON) ReleaseFlagRight = false;
            }

            if (ReleaseFlagRight && !ReleaseFlagRightBefore)
            {
                Debug.Log("Release Right");
                Miscellaneous.InvokeEvent(ReleasePose,this,new ReleasePoseEventArgs(RIGHT));
            }

            ReleaseFlagRightBefore = ReleaseFlagRight;

            ReleaseFlagLeft = true;
            for (int i = 5; i < 10; i++)
            {
                if (currentPosition[i] > EPSILON) ReleaseFlagLeft = false;
            }

            if (ReleaseFlagLeft && !ReleaseFlagLeftBefore)
            {
                Debug.Log("Release Left");
                Miscellaneous.InvokeEvent(ReleasePose,this,new ReleasePoseEventArgs(LEFT));
            }

            ReleaseFlagLeftBefore = ReleaseFlagLeft;
        }
        private ForceFeedbackInfo[] ForceFeedbackInfoArray = new ForceFeedbackInfo[10]; //0-4 right hand, 5-9 left hand

        // Use this for initialization
        void Start()
        {
            currentPosition = new float[10];
            for (int i = 0; i < 10; i++)
            {
                ForceFeedbackInfoArray[i] = new ForceFeedbackInfo();
            }
            ClientController = controller.LibdexmoClientController;

        }

        void FixedUpdate()
        {
            UpdateCurrentPosition();
            CheckRelease();
            updateCommand();
        }
        private void updateCommand()
        {

            bool[][] target = new bool[2][];
            target[0] = new bool[5] { false, false, false, false, false };
            target[1] = new bool[5] { false, false, false, false, false };

            float[][] stiffness = new float[2][];
            stiffness[0] = new float[5];
            stiffness[1] = new float[5];

            float[][] setPosition = new float[2][];
            setPosition[0] = new float[5];
            setPosition[1] = new float[5];

            bool[][] inwardControl = new bool[2][];
            inwardControl[0] = new bool[5];
            inwardControl[1] = new bool[5];

            for (int i = 0; i < 10; i++)
            {
                ForceFeedbackInfo info = ForceFeedbackInfoArray[i];
                if (info.sustaining)
                {
                    int assignedID = i / 5;
                    int fingerID = i % 5;

                    target[assignedID][fingerID] = true;

                    inwardControl[assignedID][fingerID] = info.inwardControl;

                    stiffness[assignedID][fingerID] = info.stiffness;

                    float currentPosition = this.currentPosition[i];

                    if (info.inwardControl)
                    {
                        if (currentPosition < info.offset)
                        {
                            setPosition[assignedID][fingerID] = 0;
                        }
                        else
                        {
                            setPosition[assignedID][fingerID] = currentPosition - info.offset;
                        }

                    }
                    else
                    {
                        if (currentPosition > 1 - info.offset)
                        {
                            setPosition[assignedID][fingerID] = 1;
                        }
                        else
                        {
                            setPosition[assignedID][fingerID] = currentPosition + info.offset;
                        }
                    }
                    info.positionSetPoint = setPosition[assignedID][fingerID];

                }

            }
            for (int i = 0; i < 2; i++)
            {

                controller.ImpedanceControlFingers(i, target[i], stiffness[i], setPosition[i], inwardControl[i]);

            }


        }
        private float getCurrentPosition(int assignedID, int fingerID)
        {

            UnityHandPoseManager hand = assignedID == 0 ? rightPoseManager : leftPoseManager;
            IFingerRotationNormalized[] fingers = hand.CurHandRotationNormalized.Fingers;
            return fingers[fingerID].Bend.Value;
        }

        /// <summary>
        /// give impedance to a finger immediately, usually used when a finger collided with a collider
        /// </summary>
        /// <param name="assignedID">the ID of the hand of the finger, 0 for right hand and 1 for left hand *for now*</param>
        /// <remark> it may changed in the future, if you find any problem, check this parameter</remark>
        /// <param name="fingerID">the ID of the finger, 0 for thumb and 4 for pinky</param>
        /// <param name="stiffness">the stiffness of the impedance</param>
        /// <param name="inwardControl">direction of the force feedback, draging fingers outward if true</param>
        /// <remark>Why the force is outward when inwardControl is true? Because when the force feedback is outward, your finger is moving inward!</remark>
        public void giveImpedanceFromCurrentPosition(int assignedID, int fingerID, float stiffness, bool inwardControl)
        {

            float currentPosition = getCurrentPosition(assignedID, fingerID);

            //prepare parameters
            float[] positionSetPoint = new float[5];
            positionSetPoint[fingerID] = currentPosition;
            bool[] target = new bool[5] { false, false, false, false, false };
            target[fingerID] = true;
            float[] stiffnessArray = new float[5];
            stiffnessArray[fingerID] = stiffness;
            bool[] inwardControlArray = new bool[5];
            inwardControlArray[fingerID] = inwardControl;

            //update info
            ForceFeedbackInfoArray[assignedID * 5 + fingerID] = new ForceFeedbackInfo(false, 0, currentPosition, stiffness, inwardControl);


            //emit command
            ClientController.ImpedanceControlFingers(assignedID, target, stiffnessArray, positionSetPoint, inwardControlArray);


        }


        /// <summary>
        /// drag/push finger to target position
        /// </summary>
        /// <param name="assignedID">the ID of the hand of the finger, 0 for right hand and 1 for left hand *for now*</param>
        /// <param name="fingerID">the ID of the finger, 0 for thumb and 4 for pinky</param>
        /// <param name="stiffness">the stiffness of the impedance</param>
        /// <param name="targetPosition">the target normalized position where the finger will be dragged to. The direction will be calculated automatically</param>
        public void dragFingerByTarget(int assignedID, int fingerID, float stiffness, float targetPosition)
        {

            float currentPosition = getCurrentPosition(assignedID, fingerID);
            bool inwardControl = targetPosition < currentPosition;

            //prepare parameters
            float[] positionSetPoint = new float[5];
            positionSetPoint[fingerID] = targetPosition;
            bool[] target = new bool[5] { false, false, false, false, false };
            target[fingerID] = true;
            float[] stiffnessArray = new float[5];
            stiffnessArray[fingerID] = stiffness;
            bool[] inwardControlArray = new bool[5];
            inwardControlArray[fingerID] = inwardControl;

            //update info
            ForceFeedbackInfoArray[assignedID * 5 + fingerID] = new ForceFeedbackInfo(false, 0, targetPosition, stiffness, inwardControl);

            //emit
            ClientController.ImpedanceControlFingers(assignedID, target, stiffnessArray, positionSetPoint, inwardControlArray);



        }
        /// <summary>
        /// change the stiffness of the force feedback and keep other parameters unchanged (no matter it's impeding or sustaining)
        /// </summary>
        /// <param name="assignedID">the ID of the hand of the finger, 0 for right hand and 1 for left hand *for now*</param>
        /// <param name="fingerID">the ID of the finger, 0 for thumb and 4 for pinky</param>
        /// <param name="stiffness"> new stiffness you want, 0 to release force(the same as stopForceFeedback())</param>
        public void changeStiffness(int assignedID, int fingerID, float stiffness)
        {
            //prepare parameters
            ForceFeedbackInfo info = ForceFeedbackInfoArray[assignedID * 5 + fingerID];

            float[] positionSetPoint = new float[5];
            positionSetPoint[fingerID] = info.positionSetPoint;
            bool[] target = new bool[5] { false, false, false, false, false };
            target[fingerID] = true;
            float[] stiffnessArray = new float[5];
            stiffnessArray[fingerID] = stiffness;
            bool[] inwardControlArray = new bool[5];
            inwardControlArray[fingerID] = info.inwardControl;

            //update info
            info.stiffness = stiffness;

            //emit
            ClientController.ImpedanceControlFingers(assignedID, target, stiffnessArray, positionSetPoint, inwardControlArray);


        }


        /// <summary>
        /// drag a finger according to direction and distance
        /// </summary>
        /// <param name="assignedID"></param>
        /// <param name="fingerID"></param>
        /// <param name="stiffness"></param>
        /// <param name="inwardControl"></param>
        /// <param name="offset"></param>
        public void dragFingerByDirectionAndDistance(int assignedID, int fingerID, float stiffness, bool inwardControl, float offset = 0.05f)
        {
            float currentPosition = getCurrentPosition(assignedID, fingerID);
            float targetPosition = 0;
            if (inwardControl)
            {
                if (currentPosition < offset) targetPosition = 0;
                else targetPosition = currentPosition - offset;
            }
            else
            {
                if (currentPosition > 1 - offset) targetPosition = 1f;
                else targetPosition = currentPosition + offset;
            }

            float[] positionSetPoint = new float[5];
            positionSetPoint[fingerID] = targetPosition;
            bool[] target = new bool[5] { false, false, false, false, false };
            target[fingerID] = true;
            float[] stiffnessArray = new float[5];
            stiffnessArray[fingerID] = stiffness;
            bool[] inwardControlArray = new bool[5];
            inwardControlArray[fingerID] = inwardControl;

            //update info
            ForceFeedbackInfoArray[assignedID * 5 + fingerID] = new ForceFeedbackInfo(false, 0, targetPosition, stiffness, inwardControl);

            //emit command
            ClientController.ImpedanceControlFingers(assignedID, target, stiffnessArray, positionSetPoint, inwardControlArray);


        }

        /// <summary>
        /// set resistance to the finger, only support single direction.
        /// you need to determine the direction according to the movement of the finger
        /// </summary>
        /// <param name="assignedID">the ID of the hand of the finger, 0 for right hand and 1 for left hand</param>
        /// <param name="fingerID">the ID of the finger, 0 for thumb and 4 for pinky</param>
        /// <param name="stiffness">the stiffness of the impedance</param>
        /// <param name="inwardControl">direction of the force feedback, draging fingers outward if true</param>
        public void singleDirectionResistance(int assignedID, int fingerID, float stiffness, bool inwardControl)
        {

            //update info
            ForceFeedbackInfoArray[assignedID * 5 + fingerID] = new ForceFeedbackInfo(true, 0, 0, stiffness, inwardControl);

        }
        /// <summary>
        /// drag the finger by a constant force
        /// </summary>
        /// <param name="assignedID">the ID of the hand of the finger, 0 for right hand and 1 for left hand</param>
        /// <param name="fingerID">the ID of the finger, 0 for thumb and 4 for pinky</param>
        /// <param name="stiffness">the stiffness of the impedance</param>
        /// <param name="inwardControl">direction of the force feedback, draging fingers outward if true</param>
        public void constantForceDrag(int assignedID, int fingerID, float stiffness, bool inwardControl)
        {
            //update info
            ForceFeedbackInfoArray[assignedID * 5 + fingerID] = new ForceFeedbackInfo(true, 0.1f, 0, stiffness, inwardControl);
        }
        /// <summary>
        /// stop all the force apply on the finger
        /// </summary>
        /// <param name="assignedID">the ID of the hand of the finger, 0 for right hand and 1 for left hand</param>
        /// <param name="fingerID">the ID of the finger, 0 for thumb and 4 for pinky</param>
        public void stopForceFeedback(int assignedID, int fingerID)
        {
            //update info
            ForceFeedbackInfoArray[assignedID * 5 + fingerID] = new ForceFeedbackInfo(false, 0, 0);
            bool[] target = new bool[5] { false, false, false, false, false };
            target[fingerID] = true;
            controller.ImpedanceControlStopFingers(assignedID, target);
        }
    }
}