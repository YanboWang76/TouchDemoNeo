using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : CMonoSingleton<UIManager> 
{
	private IUIStorage uiStorage;
	[SerializeField]
	private UICommonEffect effect;

	public UIWindow frontUI{ get; private set;}
	void Awake()
	{
		effect = new UICommonEffect ();
		transform.name = "UIManager";
		uiStorage = new UIStack (effect);
	}
	public void Pop(bool isShowLast = true){
		Debug.Log ("Pop");
		uiStorage.UIPop (isShowLast);
	}


	public void Push(UIWindow element){
		Debug.Log (element.name);
		uiStorage.UIPush (element);
	}
}
