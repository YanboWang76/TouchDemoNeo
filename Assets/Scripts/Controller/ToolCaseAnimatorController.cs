using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCaseAnimatorController : MonoBehaviour {

    private Animator toolCaseAnimator;
    private string toolCaseOpen = "ToolCaseOpen";
    private string toolCaseClose = "ToolCaseClose";
    private AnimatorStateInfo animatorInfo;
    public bool caseIsOpen;
    // Use this for initialization
    void Start () {
        var toolCase = GameObject.Find("ToolCaseCap");
        toolCaseAnimator = toolCase.GetComponent<Animator>();
        caseIsOpen = false;
        //toolCaseAnimator.Play(toolCaseOpen);
    }

    // Update is called once per frame
    void Update () {
        //animatorInfo = toolCaseAnimator.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(animatorInfo.normalizedTime);
        //if ((animatorInfo.normalizedTime >= 1.0f) && ((animatorInfo.IsName(toolCaseOpen) || (animatorInfo.IsName(toolCaseClose)))))
        //{
        //    Debug.Log("BombIsOpenBefore:" + caseIsOpen);
        //    caseIsOpen = !caseIsOpen;
        //}
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(12))//The number of "hand" layer
        {
            if (!caseIsOpen)
            {
                PlayToolCaseOpenAnimation();
                //Manager.Instance.AudioManager.PlayCollisionAudio();
            }
            else
            {
                PlayToolCaseCloseAnimation();
            }
        }
    }

    public void PlayToolCaseOpenAnimation()
    {
        toolCaseAnimator.Play(toolCaseOpen);
    }

    public void PlayToolCaseCloseAnimation()
    {
        toolCaseAnimator.Play(toolCaseClose);
    }
    //public void SetCaseStatusWithDelay(float delayTime)
    //{
    //    StartCoroutine(WaitAndSetCaseStatus(delayTime));
    //}

    //IEnumerator WaitAndSetCaseStatus(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    //等待之后执行的动作  
    //    caseIsOpen = !caseIsOpen;
    //    //Debug.Log("Wait and" + caseIsOpen);
    //}

    //public bool GetCaseStatus()
    //{
    //    return caseIsOpen;
    //}
}


