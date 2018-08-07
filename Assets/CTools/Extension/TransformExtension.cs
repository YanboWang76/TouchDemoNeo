using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension  {
	
	public static Transform Hide(this Transform selfTrans){
		if (selfTrans.gameObject.activeSelf)
			selfTrans.gameObject.SetActive (false);
		return selfTrans;
	}

	public static Transform Show(this Transform selfTrans){
		if (!selfTrans.gameObject.activeSelf)
			selfTrans.gameObject.SetActive (true);
		return selfTrans;
	}

	public static void DestorySeft(this Transform selfTrans,float delayTime =0f){
		selfTrans.gameObject.DestorySelf (delayTime);
	}

}
