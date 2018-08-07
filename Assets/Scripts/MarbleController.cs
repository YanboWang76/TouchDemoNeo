using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Libdexmo.Unity.HandController;
using Libdexmo.Client;

public class MarbleController : MonoBehaviour {
    public float ForceFeedbackLastTime=0.1f;
    public DexmoController controller;
    public UnityHandPoseManager leftPoseManager;
    public bool[] targets = new bool[5] { false, true, false, false, false };
    private Libdexmo.Client.Controller ClientController;
    public int AssignedID;
    public float Stiffness;
    public float PositionSetPoint;
    public bool InwardControl;

    private string fingerFlag;//Which part of finger touches the marble first
    private bool Flag;//

    private float leftIndexBending;

    //public float offset;
    //private bool isVibrating;
    ////private bool isLocked;

    //private bool isImpeding;
    //private bool constantForceApply;


    private Rigidbody rb;
    public Vector3 Force = new Vector3(0,0,5);
	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        ClientController = controller.LibdexmoClientController;
        fingerFlag = null;
        Flag = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.AddForce(Force);
        }
	}
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("index") && other.gameObject.layer.Equals(12))
        {
            if (Flag == true)
                return;
            fingerFlag = other.gameObject.name;
            leftIndexBending = leftPoseManager.CurHandRotationNormalized.Index.Bend.Value;
            float[] stiffnessArray = new float[5];
            stiffnessArray[1] = Stiffness;
            float[] PositionSetPointArray = new float[5];
            PositionSetPointArray[1] = leftIndexBending;
            bool[] InwardControlArray = new bool[5];
            InwardControlArray[1] = InwardControl;
            Debug.Log("enter " + other.name);
            
            //Sending force feedback command to the client 
            ClientController.ImpedanceControlFingers(AssignedID, targets, stiffnessArray, PositionSetPointArray, InwardControlArray);
            Debug.Log("hit");
            //Stop force feedback after a given time, can be modified
            StartCoroutine(WaitAndReleaseForceFeedBack(ForceFeedbackLastTime));

            //Force feedback can only be triggered once in 0.5 second
            Flag = true;
            StartCoroutine(WaitAndSetFlagFalse(0.5f));
        }
    }

    IEnumerator WaitAndReleaseForceFeedBack(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ClientController.StopImpedanceControlFingersAll(1);
    }

    IEnumerator WaitAndSetFlagFalse(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Flag = false;
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.name.Equals(fingerFlag)&& other.gameObject.layer.Equals(12))
    //    {
    //        Debug.Log("exit "+other.name);
    //        ClientController.StopImpedanceControlFingersAll(1);
    //        //fingerFlag = null;
    //    }
    //}
}

