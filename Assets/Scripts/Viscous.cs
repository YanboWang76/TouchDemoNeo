using System.Collections;
using System.Collections.Generic;
using HackInDexmo.Scripts;
using UnityEngine;

public class Viscous : MonoBehaviour
{
    public BypassController controller;
    public float viscosity; 
	public bool[] active = new bool[10];

    Transform[] fingers = new Transform[10];
    Vector3[] previousPosition = new Vector3[10];
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            active[i] = false;
            previousPosition[i] = new Vector3(0, 0, 0);
        }
        GameObject left = controller.leftPoseManager.gameObject;
        GameObject right = controller.rightPoseManager.gameObject;

        fingers[0] = right.transform.Find("thumb1/thumb2/thumb3/thumb4");
        fingers[1] = right.transform.Find("index1/index2/index3/index4");
        fingers[2] = right.transform.Find("middle1/middle2/middle3/middle4");
        fingers[3] = right.transform.Find("ring1/ring2/ring3/ring4");
        fingers[4] = right.transform.Find("pinky1/pinky2/pinky3/pinky4");
        fingers[5] = left.transform.Find("thumb1/thumb2/thumb3/thumb4");
        fingers[6] = left.transform.Find("index1/index2/index3/index4");
        fingers[7] = left.transform.Find("middle1/middle2/middle3/middle4");
        fingers[8] = left.transform.Find("ring1/ring2/ring3/ring4");
        fingers[9] = left.transform.Find("pinky1/pinky2/pinky3/pinky4");

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        for (int i = 0; i < 10; i++)
        {
            if (active[i])
            {
				Vector3 move =  fingers[i].position - previousPosition[i];
				bool inwardControl = Vector3.Dot(move,fingers[i].up)>0;
				float stiffness = viscosity*Vector3.Magnitude(move);
				controller.dragFingerByDirectionAndDistance(i/5,i%5,stiffness,inwardControl);
                
            }

            previousPosition[i] = fingers[i].position;
        }

    }
   
   
    void OnTriggerEnter(Collider other)
    {
        int fingerID = Utility.GetFingerId(other);
        if (fingerID != -1)
        {
            active[fingerID] = true;
			previousPosition[fingerID] = fingers[fingerID].position;
        }
    }
    void OnTriggerExit(Collider other)
    {
        int fingerID = Utility.GetFingerId(other);
        if (fingerID != -1)
        {
			controller.stopForceFeedback(fingerID/5,fingerID%5);
            active[fingerID] = false;
        }
    }
}
