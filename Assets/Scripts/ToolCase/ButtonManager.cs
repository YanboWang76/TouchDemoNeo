using System;
using UnityEngine;
using Libdexmo.Unity.Core.Utility;
using Libdexmo.Unity.Triggering;
using System.Collections;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour {

	public event EventHandler CaseOpen;

    [SerializeField]
    private ButtonController _button1;
    [SerializeField]
    private ButtonController _button2;
    [SerializeField]
    private Animator ToolCaseAnimator;

    private ButtonController _pressedButton;

    void Awake()
    {
        if (Miscellaneous.CheckNullAndLogError(_button1) ||
            Miscellaneous.CheckNullAndLogError(_button2) )
        {
            return;
        }
        _button1.ButtonPressed += OnButtonPressed;
        _button2.ButtonPressed += OnButtonPressed;
    }

    /// <summary>
    /// Handles the ButtonPressed event triggered by each color buttons. It figures
    /// out which button sends this event by looking at the sender argument and
    /// changes color accordingly.
    /// </summary>
    /// <param name="sender">Sender of the event.</param>
    /// <param name="args">Event args that include the color to change.</param>
    
    private void OnButtonPressed(object sender, EventArgs e)
    {
        ButtonController button = sender as ButtonController;
        if (button == null)
        {
            Debug.Log("Unknown button trigger starts.");
            return;
        }
        if (_pressedButton == null)//If no button is been pressed right now, set this button as the first pressed button. 
        {
            _pressedButton = button;
        }
        else
        {
            if (_pressedButton != button)//If the button being pressed right now does not equals to the previous one, set the previous IsPressed to false, and the current one as _pressedButton
            {
                //_pressedButton.IsPressed = false;
                StartCoroutine(WaitAndSetFalse(1f,button));
                //button.IsPressed = false;
                ToolCaseAnimator.Play("ToolCaseOpen");
				CaseOpen (this, new EventArgs ());
                ///In case of only allowing a single button to be pressed at a time, comment the three lines above and uncomment the three lines below.
                //_toolCaseAnimatorController.PlayToolCaseOpenAnimation();
                //_pressedButton.IsPressed = false;
                
            }
        }
    }

    IEnumerator WaitAndSetFalse(float waitTime,ButtonController button)
    {
        yield return new WaitForSeconds(waitTime);
        _pressedButton.IsPressed = false;
        button.IsPressed = false;
        _pressedButton = button;
    }

    void OnDestroy()
    {
        if (_button1 != null)
        {
            _button1.ButtonPressed -= OnButtonPressed;
        }
        if (_button2 != null)
        {
            _button2.ButtonPressed -= OnButtonPressed;
        }
    }
}
