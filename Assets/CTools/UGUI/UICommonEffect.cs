using UnityEngine;
using System.Collections;

public class UICommonEffect :IUIStorageEffect
{
	#region IUIStorageEffect implementation
	public void ShowEffect (UIWindow element)
	{
		element.Trans.Show ();
	}
	public void HideEffect (UIWindow element)
	{
		element.Trans.Hide ();
	}
	#endregion
	
}

