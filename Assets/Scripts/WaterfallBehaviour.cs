using System.Collections;
using System.Collections.Generic;
using HackInDexmo.Scripts;
using UnityEngine;

public class WaterfallBehaviour : MonoBehaviour
{

    public BypassController controller;
    public float stiffness;

    public Vector3 waterCurrent = new Vector3(0, -1, 0);

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// emit coresponding command according to the caller 
    /// </summary>
    /// <param name="other">the trigger caller</param>
    /// <param name="isEnter">true if it's on trigger enter, false if it's on trigger exit</param>
    void emitCommand(Collider other, bool isEnter)
    {
        if (other.gameObject.layer.Equals(12)) //if it's part of a hand
        {
            //Debug.Log(other.name);
            int assignedID = -1;
            int fingerID = -1;
            bool inwardControl;

            //find fingerID

            if (other.name.Contains("thumb"))
            {
                fingerID = 0;
            }
            else if (other.name.Contains("index"))
            {
                fingerID = 1;
            }
            else if (other.name.Contains("middle"))
            {
                fingerID = 2;
            }
            else if (other.name.Contains("ring"))
            {
                fingerID = 3;
            }
            else if (other.name.Contains("pinky"))
            {
                fingerID = 4;
            }
            else return;
            //find assignedID

            Transform current = other.gameObject.transform;
            Vector3 handBack;
            while (current != null)
            {
                //Debug.Log(current.name);
                if (current.name.Equals("TranslucentLeft"))
                {
                    assignedID = 1;
                    break;
                }
                else if (current.name.Equals("TranslucentRight"))
                {
                    assignedID = 0;
                    break;
                }
                current = current.parent;
            }
            if (current == null) return;
            //Debug.Log(current.name);
            handBack = current.up;

            //find inward control
            inwardControl = Vector3.Dot(handBack, waterCurrent) > 0;

            Debug.Log(inwardControl);

            if (fingerID == -1 || assignedID == -1) return;

            // emit command
            if (isEnter){
                controller.constantForceDrag(assignedID, fingerID, stiffness, inwardControl);
                Debug.Log("cst force drag "+ assignedID+ " " + fingerID+" "+stiffness+" "+inwardControl);
            }else
                controller.stopForceFeedback(assignedID, fingerID);
                Debug.Log("cst force drag stop"+ assignedID+ " " + fingerID);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        emitCommand(other, true);
    }

    void OnTriggerExit(Collider other)
    {
        emitCommand(other, false);
    }
}
