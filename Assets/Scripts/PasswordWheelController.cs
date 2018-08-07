using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordWheelController : MonoBehaviour {

    private Vector3 nearest36Degree;
    private int nearestY;
    private int round;

    //public float xRotation = 5.0F;
    //public float x;

    public MeshRenderer mesh;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        
            if (gameObject.transform.localEulerAngles.y % 36 != 0)
            {
                if (gameObject.transform.localEulerAngles.y % 36 >= 18f)
                {
                    nearestY = (int)Mathf.Round(transform.localEulerAngles.y + (36 - transform.localEulerAngles.y % 36));
                }
                //else
                //{
                //    nearestY = (int)Mathf.Round(transform.localEulerAngles.x - transform.localEulerAngles.x % 36);
                //}
            }

            mesh.transform.localEulerAngles = new Vector3(0, nearestY, 0);
        
    }


    private void OnTriggerExit(Collider other)
    {
    }
}
