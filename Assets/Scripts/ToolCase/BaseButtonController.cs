using System;
using UnityEngine;
using System.Collections;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.Utility;
using Libdexmo.Unity.Touchables;
using Libdexmo.Unity.Triggering;

public class BaseButtonController : MonoBehaviour
{
	//public event EventHandler ButtonPressed;
	public bool IsPressed
	{
		get { return _isPressed; }
		set { UpdateButtonStatus(value); }
	}
		
	[SerializeField]
	private ButtonMover _buttonMover;
	[SerializeField]
	private TwoStagePositionChangeTouchableForceFeedback _touchable;
	//[SerializeField]
	//private Animator ToolCaseAnimator;

	protected bool _isPressed;

	void Awake()
	{
		if (Miscellaneous.CheckNullAndLogError(_buttonMover) ||
			Miscellaneous.CheckNullAndLogError(_touchable))
		{
			return;
		}
		_buttonMover.ButtonPressed += OnButtonPressed;
	}

	protected void ChangeTouchableOnPressed()
	{
		_touchable.UpdateForceFeedbackState(TwoStageForceFeedbackState.Hard);
	}

	protected void ChangeTouchableOnReleased()
	{
		_touchable.UpdateForceFeedbackState(TwoStageForceFeedbackState.Soft);
	}

	protected void UpdateButtonStatus(bool isPressedNow)
	{
		bool isPressedLastTime = _isPressed;
		if (isPressedLastTime && !isPressedNow)
		{
			Release();
			_isPressed = false;
		}
	}

	protected void Release()
	{
		_buttonMover.Release();
		ChangeTouchableOnReleased();
	}

	public virtual void OnButtonPressed(object sender, EventArgs args)
	{
		//ButtonPressed(this,arg);
		//Miscellaneous.InvokeEvent(ButtonPressed, this);
		ChangeTouchableOnPressed();
		///In case of only allowing a single button to be pressed at a time, uncomment the line below.
		//ToolCaseAnimator.Play("ToolCaseOpen");
		_isPressed = true;
	}
		
	protected void PassTriggerStartEvent(object sender, CollisionTriggerActionEventArgs args)
	{
		//Miscellaneous.InvokeEvent(TriggerStart, sender, args);
		ChangeTouchableOnPressed();
		//StartCoroutine(Miscellaneous.DelayCoroutine(ChangeTouchableOnPressed, 1));
	}

	protected void PassTriggerEndEvent(object sender, CollisionTriggerActionEventArgs args)
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
