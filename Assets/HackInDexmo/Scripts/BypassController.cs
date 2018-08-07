using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Libdexmo.Unity.HandController;
using Libdexmo.Client;
using Libdexmo.Unity.Core;

public class BypassController : MonoBehaviour
{
    public DexmoController controller;
    public UnityHandPoseManager leftPoseManager;
    public UnityHandPoseManager rightPoseManager;
    private Controller ClientController;
    private class sustainingForceFeedbackInfo
    {
        public bool active = false;
        public float offset = 0;
        public float stiffness = 0;
        public bool inwardControl = true;
    }

    private sustainingForceFeedbackInfo[] sustainingForceFeedbackInfoArray = new sustainingForceFeedbackInfo[10]; //0-4 right hand, 5-9 left hand

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            sustainingForceFeedbackInfoArray[i] = new sustainingForceFeedbackInfo();
        }
        ClientController = controller.LibdexmoClientController;

    }

    void FixedUpdate()
    {
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
            sustainingForceFeedbackInfo info = sustainingForceFeedbackInfoArray[i];
            if (info.active)
            {
                int assignedID = i / 5;
                int fingerID = i % 5;

                target[assignedID][fingerID] = true;

                inwardControl[assignedID][fingerID] = info.inwardControl;

                stiffness[assignedID][fingerID] = info.stiffness;

                float currentPosition = getCurrentPosition(assignedID, fingerID);

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

        //emit
        ClientController.ImpedanceControlFingers(assignedID, target, stiffnessArray, positionSetPoint, inwardControlArray);



    }
    /// <summary>
    /// drag a finger according to direction and distance
    /// </summary>
    /// <param name="assignedID">the ID of the hand of the finger, 0 for right hand and 1 for left hand *for now*</param>
    /// <param name="fingerID">the ID of the finger, 0 for thumb and 4 for pinky</param>
    /// <param name="stiffness">the stiffness of the impedance</param>
    /// <param name="inwardControl">the direction of the drag</param>
    /// <param name="offset"> the distance to drag, 0.05 by default</param>
    public void dragFingerByDirectionAndDistance(int assignedID,int fingerID,float stiffness,bool inwardControl,float offset = 0.05f){
        float currentPosition = getCurrentPosition(assignedID, fingerID);
        float targetPosition = 0;
        if(inwardControl){
            if(currentPosition<offset)targetPosition = 0;
            else targetPosition = currentPosition-offset;
        }else{
            if(currentPosition>1-offset)targetPosition = 1f;
            else targetPosition = currentPosition+offset;
        }
        
        float[] positionSetPoint = new float[5];
        positionSetPoint[fingerID] = targetPosition;
        bool[] target = new bool[5] { false, false, false, false, false };
        target[fingerID] = true;
        float[] stiffnessArray = new float[5];
        stiffnessArray[fingerID] = stiffness;
        bool[] inwardControlArray = new bool[5];
        inwardControlArray[fingerID] = inwardControl;
        
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

        sustainingForceFeedbackInfo info = sustainingForceFeedbackInfoArray[assignedID * 5 + fingerID];
        info.active = true;
        info.stiffness = stiffness;
        info.inwardControl = inwardControl;
        info.offset = 0;

    }
    /// <summary>
    /// drag the finger by a constant force
    /// </summary>
    /// <param name="assignedID">the ID of the hand of the finger, 0 for right hand and 1 for left hand</param>
    /// <param name="fingerID">the ID of the finger, 0 for thumb and 4 for pinky</param>
    /// <param name="stiffness">the stiffness of the impedance</param>
    /// <param name="inwardControl">direction of the force feedback, draging fingers outward if true</param>
    public void constantForceDrag(int assignedID, int fingerID, float stiffness, bool inwardControl) { 
        sustainingForceFeedbackInfo info = sustainingForceFeedbackInfoArray[assignedID * 5 + fingerID];
        info.active = true;
        info.stiffness = stiffness;
        info.inwardControl = inwardControl;
        info.offset = 0.1f;

    }
    /// <summary>
    /// stop all the force apply on the finger
    /// </summary>
    /// <param name="assignedID">the ID of the hand of the finger, 0 for right hand and 1 for left hand</param>
    /// <param name="fingerID">the ID of the finger, 0 for thumb and 4 for pinky</param>
    public void stopForceFeedback(int assignedID,int fingerID){
        sustainingForceFeedbackInfo info = sustainingForceFeedbackInfoArray[assignedID * 5 + fingerID];
        info.active = false;
        info.stiffness = 0;
        info.inwardControl = true;
        info.offset =0;
        bool[] target = new bool[5] { false, false, false, false, false };
        target[fingerID] = true;
        controller.ImpedanceControlStopFingers(assignedID,target);
    }
}