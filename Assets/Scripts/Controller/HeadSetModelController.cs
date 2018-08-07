using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CTool.Tween;

public class HeadSetModelController : MonoBehaviour {

    private Vector3 pos;
    private Quaternion rot;

    private bool objectReset = false;
    //private AudioSource collision;

    //private Transform Origin; 
	// Use this for initialization
	void Start () {
        rot = transform.rotation;
        pos = transform.position;

        //collision = gameObject.GetComponent<AudioSource>();
       //Origin = gameObject.transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision other)
    {  
        if(other.gameObject.tag == "plane")
        {
                if (!objectReset)
                {
                    StartCoroutine(WaitAndExcecute(2.0F));
                    Manager.Instance.AudioManager.PlayCollisionAudio();
                //collision.Play();
                }
            }
    }

    IEnumerator WaitAndExcecute(float waitTime)
    {
        objectReset = true;
        yield return new WaitForSeconds(waitTime);
        transform.DOScale(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        transform.position = pos ;
        transform.rotation = rot;
        transform.localScale = Vector3.one*0.5f;
        objectReset = false;
        //等待之后执行的动作  
        //transform.DOMove(pos, 1);
        //transform.DORotate(rot.eulerAngles, 1);
        // transform.position = pos;
        //transform.rotation = rot; 
    }
}
