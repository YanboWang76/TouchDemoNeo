using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Libdexmo.Unity.HandController;
using Libdexmo.Client;

public class IndependanceFingerController : MonoBehaviour {

    public DexmoController controller;
    //public UnityHandPoseManager leftPoseManager;
    public bool[] targets = new bool[5] { false, true, false, false, false };
    private Libdexmo.Client.Controller ClientController;
    public int AssignedID;
    public float Stiffness;
    public float PositionSetPoint;
    public bool InwardControl;

    public float offset;
    //private bool isVibrating;
    //private bool isLocked;

    private bool isImpeding;
    private bool constantForceApply;
    //private bool isLatencyTesting;
    // Use this for initialization
    void Start()
    {
        ClientController = controller.LibdexmoClientController;
        //isVibrating = isLatencyTesting = isLocked = 
        isImpeding = constantForceApply = false;
    }
    void KeyboardControll()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("update impedance");
            float[] stiffnessArray = new float[5];
            stiffnessArray[1] = Stiffness;
            float[] PositionSetPointArray = new float[5];
            PositionSetPointArray[1] = PositionSetPoint;
            bool[] InwardControlArray = new bool[5];
            InwardControlArray[1] = InwardControl;
            ClientController.ImpedanceControlFingers(AssignedID, targets, stiffnessArray, PositionSetPointArray, InwardControlArray);
            isImpeding = true;

        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (constantForceApply) return;
            Debug.Log("start impending");
            float[] stiffnessArray = new float[5];
            stiffnessArray[1] = Stiffness;
            float[] PositionSetPointArray = new float[5];
            PositionSetPointArray[1] = PositionSetPoint;
            bool[] InwardControlArray = new bool[5];
            InwardControlArray[1] = InwardControl;
            ClientController.ImpedanceControlFingers(AssignedID, targets, stiffnessArray, PositionSetPointArray, InwardControlArray);
            isImpeding = true;

        }
        //constant force 
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("costant force impending");
            constantForceApply = true;
        }


        /* No locking in current hardware 

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (isLocked)//tofix
                {
                    Debug.Log("stop locking");
                    ClientController.StopContinuousLockFingers(AssignedID, targets);//tofix
                    isLocked = false;//tofix
                }
                else
                {

                    Debug.Log("start locking");
                    ClientController.StopContinuousLockFingers(AssignedID, targets);//tofix;
                    isLocked = true;//tofix
                }
            }
        */

        /* I can't understand what this testing is but it turns out makes no effect

            if (Input.GetKeyDown(KeyCode.T))
            {
                if (isLatencyTesting)
                {

                    Debug.Log("stop testing");
                    ClientController.StopImpedanceControlFingers(AssignedID, targets);//tofix
                    isLatencyTesting = false;//tofix
                }
                else
                {

                    Debug.Log("start testing");
                    float[] stiffnessArray = new float[5];
                    stiffnessArray[1] = Stiffness;
                    float[] PositionSetPointArray = new float[5];
                    PositionSetPointArray[1] = PositionSetPoint;
                    bool[] InwardControlArray = new bool[5];
                    InwardControlArray[1] = InwardControl;
                    ClientController.ImpedanceControlLatencyTest(AssignedID, targets, stiffnessArray, PositionSetPointArray, InwardControlArray);
                    isLatencyTesting = true;
                }
            }
        */

        /*no viberating in current hardware 

            if (Input.GetKeyDown(KeyCode.V))
            {
                if (isVibrating)//tofix
                {
                    Debug.Log("stop V-ing");
                    ClientController.StopContinuousVibrateFingers(AssignedID, targets);//tofix
                    isVibrating = false;//tofix
                }
                else
                {

                    Debug.Log("start V-ing");
                    ClientController.StartContinuousVibrateFingers(AssignedID, targets);//tofix;
                    isVibrating = true;//tofix
                }
            }
    	*/
    }
    //float getCurrentPosition()
    //{

    //    return leftPoseManager.CurHandRotationNormalized.Index.Bend.Value;
    //}
    //void ConstantForce()
    //{
    //    float[] stiffnessArray = new float[5];
    //    stiffnessArray[1] = Stiffness;
    //    float[] PositionSetPointArray = new float[5];
    //    float currentFingerPosition = getCurrentPosition();
    //    if (currentFingerPosition < offset)
    //    {
    //        PositionSetPointArray[1] = 0;
    //    }
    //    else
    //    {
    //        PositionSetPointArray[1] = currentFingerPosition - offset;
    //    }
    //    bool[] InwardControlArray = new bool[5];
    //    InwardControlArray[1] = InwardControl;
    //    ClientController.ImpedanceControlFingers(AssignedID, targets, stiffnessArray, PositionSetPointArray, InwardControlArray);
    //    isImpeding = true;

    //}
    // Update is called once per frame
    void Update()
    {
        KeyboardControll();
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("stop all force feedback");
            ClientController.StopImpedanceControlFingersAll(AssignedID);
            isImpeding = constantForceApply = false;
        }
    }
}
