using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchSceneController : MonoBehaviour {
    
    // Use this for initialization
    void Start ()
    {
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered");
        if (other.tag == "VRHeadset")
        {
            Debug.Log(other.tag);
            Manager.Instance.GameSceneManager.LoadBombExpertScene();
        }
    }
}
