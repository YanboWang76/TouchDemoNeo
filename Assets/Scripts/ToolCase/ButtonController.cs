using System;
using UnityEngine;
using System.Collections;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.Utility;
using Libdexmo.Unity.Touchables;
using Libdexmo.Unity.Triggering;

public class ButtonController : BaseButtonController
{
    public event EventHandler ButtonPressed;

    public override void OnButtonPressed(object sender, EventArgs args)
    {
        //ButtonPressed(this,arg);
        Miscellaneous.InvokeEvent(ButtonPressed, this);
        ChangeTouchableOnPressed();
        ///In case of only allowing a single button to be pressed at a time, uncomment the line below.
        //ToolCaseAnimator.Play("ToolCaseOpen");
        _isPressed = true;
    }
}
