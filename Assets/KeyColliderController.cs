using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyColliderController : MonoBehaviour {

    public BypassController ByPassController;
    public bool Flag;

    //private double ox, oy, oz;
    //private double x, y, z;


    // Use this for initialization
    void Start () {
        Flag = false;

        //ox = transform.localScale.x;
        //oy = transform.localScale.y;
        //oz = transform.localScale.z;

        //x = transform.localScale.x * 1.3f;
        //y = transform.localScale.y;
        //z = transform.localScale.z * 1.3f;

    }
	
	// Update is called once per frame
	void Update () {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("KeySlot"))
        {
            
            if (Flag == true)
                return;
            ByPassController.dragFingerByDirectionAndDistance(1, 0, 5, true, 0.1f);
            ByPassController.dragFingerByDirectionAndDistance(1, 1, 5, true, 0.1f);
            Flag = true;
            StartCoroutine(WaitAndReleaseForceFeedback(0.3f));
            StartCoroutine(WaitAndSetFlagFalse(1.5f));
        }
    }

    IEnumerator WaitAndReleaseForceFeedback(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        ByPassController.stopForceFeedback(1, 0);
        ByPassController.stopForceFeedback(1, 1);

    }

    IEnumerator WaitAndSetFlagFalse(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Flag = false;
    }
}
