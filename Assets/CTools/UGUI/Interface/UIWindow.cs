using UnityEngine;
using System.Collections;

public abstract class UIWindow : MonoBehaviour,IUIWindow
{
	protected bool needDestory = false;
	#region IUIWindow implementation
	public bool NeedDestory {
		get {
			return needDestory;
		}
		set {
			needDestory = value;
		}
	}
	public Transform Trans {
		get {
			return transform;
		}
	}
	#endregion
	

}

