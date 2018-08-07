using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension  {

	public static void DestorySelf(this GameObject selfObj,float delayTime = 0f){
		GameObject.Destroy (selfObj, delayTime);
	}

	public static GameObject Show(this GameObject selfObj){
		if(!selfObj.activeSelf)
			selfObj.SetActive (true);
		return selfObj;
	}

	public static GameObject Hide(this GameObject selfObj){
		if (selfObj.activeSelf)
			selfObj.SetActive (false);
		return selfObj;
	}

}
