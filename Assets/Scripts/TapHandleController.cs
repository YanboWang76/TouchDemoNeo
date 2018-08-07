using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapHandleController : MonoBehaviour {

    public GameObject Handle;
    public InstantiateWaterController TapMouth;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(CheckAngle(Handle.transform.localEulerAngles.y) < 0)
        {
            Handle.transform.localEulerAngles = new Vector3(Handle.transform.localEulerAngles.x, 0, Handle.transform.localEulerAngles.z);
        }
        if (CheckAngle(Handle.transform.localEulerAngles.y) > 90)
        {
            Handle.transform.localEulerAngles = new Vector3(Handle.transform.localEulerAngles.x, 90, Handle.transform.localEulerAngles.z);
        }
        if(Handle.transform.localEulerAngles.y >= 0 && Handle.transform.localEulerAngles.y < 30)
        {
            TapMouth.SetStatusClose();
        }
        if(Handle.transform.localEulerAngles.y >= 30 && Handle.transform.localEulerAngles.y < 60)
        {
            TapMouth.SetStatueMiddle();
        }
        if(Handle.transform.localEulerAngles.y >= 60 && Handle.transform.localEulerAngles.y <= 90)
        {
            TapMouth.SetStatusLarge();
        }
        //Debug.Log(Handle.transform.localEulerAngles.y);
	}

    public float CheckAngle(float value)
    {
        float angle = value - 180;

        if (angle > 0)
            return angle - 180;

        return angle + 180;
    }
}
