using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CTool.Tween;

public class HeartController : MonoBehaviour {

    private int frameCount;
    public int LifeFrames = 60;
    private double ox, oy, oz;
    private double x, y, z;

	// Use this for initialization
	void Start () {
        frameCount = 0;
        ox = transform.localScale.x;
        oy = transform.localScale.y;
        oz = transform.localScale.z;

        x = transform.localScale.x * 1.3f;
        y = transform.localScale.y ;
        z = transform.localScale.z * 1.3f;
    }
	
	// Update is called once per frame
	void Update () {
        frameCount++;
        if(frameCount == LifeFrames)
        {

            //Debug.Log(x + " " + y + " " + z);
            //gameObject.transform.localScale = new Vector3((float)x, (float)y, (float)z);
            transform.DOScale(new Vector3((float)x, (float)y, (float)z), 0.3f);
            frameCount = 0;
            StartCoroutine(WaitAndResetScale(0.3f));
        }
	}

    IEnumerator WaitAndResetScale(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        transform.DOScale(new Vector3((float)ox, (float)oy, (float)oz), 0.3f);
        //gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
}
