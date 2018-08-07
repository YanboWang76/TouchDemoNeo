using System;
using UnityEngine;
using System.Collections;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.Utility;
using Libdexmo.Unity.Touchables;
using Libdexmo.Unity.Triggering;

public class TimerButtonController : BaseButtonController
{
	public event EventHandler ButtonPressed;

    public TimerController _timerController;

	public override void OnButtonPressed(object sender, EventArgs args)
    {
        //ButtonPressed(this,arg);
        Miscellaneous.InvokeEvent(ButtonPressed, this);
        ChangeTouchableOnPressed();
        _timerController.AddTime(60);
        _isPressed = true;
    }		
}