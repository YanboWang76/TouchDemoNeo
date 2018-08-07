using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCaseStatusManager : MonoBehaviour {
    
	public event EventHandler CaseClose;

    [SerializeField]
    private Animator toolCaseAnimator;
    [HideInInspector]
    public bool caseIsOpen;

    private string toolCaseOpen = "ToolCaseOpen";
    private string toolCaseClose = "ToolCaseClose";
    
    // Use this for initialization
    void Start () {
        caseIsOpen = false;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("CaseIsOpen: " + caseIsOpen);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(12))//The number of "hand" layer
            if (caseIsOpen)
            {
                toolCaseAnimator.Play(toolCaseClose);
			    CaseClose (this, new EventArgs ());
                //Manager.Instance.AudioManager.PlayCollisionAudio();
            }
    }

    public void SetCaseStatusClose()
    {
        caseIsOpen = false;
    }

    public void SetCaseStatusOpen()
    {
        caseIsOpen = true;
    }
    
}
