using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropController : MonoBehaviour {

    public float LifeTime = 1f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //StartCoroutine(WaitAndDestroy(LifeTime));
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Plane"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator WaitAndDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
