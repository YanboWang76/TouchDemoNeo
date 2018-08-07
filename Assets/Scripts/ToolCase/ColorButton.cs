using System;
using UnityEngine;
using System.Collections;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.Utility;
using Libdexmo.Unity.Touchables;
using Libdexmo.Unity.Triggering;

public class ColorButton : MonoBehaviour
{
    public event EventHandler<ColorButtonTriggerEventArgs> ButtonPressed;
    public bool IsPressed
    {
        get { return _isPressed; }
        set { UpdateButtonStatus(value); }
    }

    [SerializeField]
    private ButtonMover _buttonMover;
    [SerializeField]
    private TwoStagePositionChangeTouchableForceFeedback _touchable;
    [SerializeField]
    private Color _color;

    private bool _isPressed;

    void Awake()
    {
        if (Miscellaneous.CheckNullAndLogError(_buttonMover) ||
            Miscellaneous.CheckNullAndLogError(_touchable))
        {
            return;
        }
        _buttonMover.ButtonPressed += OnButtonPressed;
    }

    private void ChangeTouchableOnPressed()
    {
        _touchable.UpdateForceFeedbackState(TwoStageForceFeedbackState.Hard);
    }

    private void ChangeTouchableOnReleased()
    {
        _touchable.UpdateForceFeedbackState(TwoStageForceFeedbackState.Soft);
    }

    private void UpdateButtonStatus(bool isPressedNow)
    {
        bool isPressedLastTime = _isPressed;
        if (isPressedLastTime && !isPressedNow)
        {
            // Release button
            Release();
            _isPressed = false;
        }
    }

    private void Release()
    {
        _buttonMover.Release();
        ChangeTouchableOnReleased();
    }

    private void OnButtonPressed(object sender, EventArgs args)
    {
        ColorButtonTriggerEventArgs colorArgs = new ColorButtonTriggerEventArgs(_color);
        Miscellaneous.InvokeEvent(ButtonPressed, this, colorArgs);
        ChangeTouchableOnPressed();
        _isPressed = true;
    }

    private void PassTriggerStartEvent(object sender, CollisionTriggerActionEventArgs args)
    {
        //Miscellaneous.InvokeEvent(TriggerStart, sender, args);
        ChangeTouchableOnPressed();
        //StartCoroutine(Miscellaneous.DelayCoroutine(ChangeTouchableOnPressed, 1));
    }

    private void PassTriggerEndEvent(object sender, CollisionTriggerActionEventArgs args)
    {
        //Miscellaneous.InvokeEvent(TriggerEnd, sender, args);
        ChangeTouchableOnReleased();
        //StartCoroutine(Miscellaneous.DelayCoroutine(ChangeTouchableOnReleased, 1));
    }

    void OnDestroy()
    {
        if (_buttonMover != null)
        {
            _buttonMover.ButtonPressed -= OnButtonPressed;
        }
    }
}
