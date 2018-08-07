using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCaseAnimatorController : MonoBehaviour {

    //[SerializeField]
    //private Animation bombCaseOpen;
    //[SerializeField]
    //private AnimationClip bombCaseClose;
	[SerializeField]
	private ButtonManager toolCaseManager;
    private Animator bombCaseAnimator;
    private string bombCaseOpen = "BombCaseOpen";
    //private string bombCaseClose = "BombCaseClose";
    public bool caseIsOpen;

	private bool _toolCaseOpen = false;
    private int _currentDisplay;
    [SerializeField]
    private GameObject _knobDisplay;
	private KnobDisplayController knobDisplayController;
    // Use this for initialization
    void Start () {
        //var toolCase = GameObject.Find("BombCase");
        //_knobDisplay = GameObject.Find("KnobDisplay");
        bombCaseAnimator = GetComponent<Animator>();
		knobDisplayController = _knobDisplay.GetComponent<KnobDisplayController> ();
        caseIsOpen = false;
        //bombCaseAnimator.Play(bombCaseOpen);
		toolCaseManager.CaseOpen += ToolCaseOpen;
    }
	
	// Update is called once per frame
	void Update () {
        CheckForOpen();
	}

    private void CheckForOpen()
    {
		_currentDisplay = knobDisplayController.GetKnobDisplay();//Get the current display number from the KnobDisplayController script
		if (!caseIsOpen && _currentDisplay == 283 && _toolCaseOpen)
        {
            bombCaseAnimator.Play(bombCaseOpen);
            Manager.Instance.AudioManager.PlayBombCaseOpenAudio();
            caseIsOpen = !caseIsOpen;
            //StartCoroutine(WaitAndSetCaseStatus(1F));
        }
    }

	private void ToolCaseOpen(object sender, EventArgs e)
	{
		_toolCaseOpen = true;
	}
    //IEnumerator WaitAndSetCaseStatus(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    //等待之后执行的动作  
    //    caseIsOpen = !caseIsOpen;
    //}
}
