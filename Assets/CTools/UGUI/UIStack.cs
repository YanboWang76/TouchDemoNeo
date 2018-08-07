using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStack :IUIStorage
{

	private IUIStorageEffect mEffect;
	private Stack uiStack = new Stack();

	private UIWindow curElement = null;

	public UIStack(IUIStorageEffect effect)
	{
		mEffect = effect;
	}
	#region IUIStorage implementation

	public void UIPop (bool isShowlast = true)
	{
		if (null != curElement) {
			mEffect.HideEffect (curElement);
			if(uiStack.Count>0)
				uiStack.Pop ();
		}
		if (isShowlast&&uiStack.Count>0) {
			curElement = (UIWindow)uiStack.Pop ();
			mEffect.ShowEffect (curElement);
			Debug.Log ("curElement" + curElement.name);
		} else {
			curElement = null;
			uiStack.Clear();
		}
	}

	public UIWindow CurWindow {
		get {
			return (UIWindow)uiStack.Pop ();
		}
	}

	public void UIPush (UIWindow element)
	{
		if (null != element) {
			if (null != curElement&& curElement!=element) 
				mEffect.HideEffect (curElement);
			uiStack.Push (element);
			curElement = element;
			mEffect.ShowEffect (curElement);
		}
	}
	#endregion
	
}
