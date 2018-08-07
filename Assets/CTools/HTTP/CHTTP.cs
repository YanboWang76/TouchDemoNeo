using CTool.Extension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class CHTTP
{
	private const string TAG = "CLF CHTTP";

	public static void GetHTTP(this MonoBehaviour mono, string url, Action<string> fulFillAction)
	{
		mono.StartCoroutine (getHTTP(url,fulFillAction));
	}

	private static IEnumerator getHTTP(string url, Action<string> fulFillAction)
	{
		UnityWebRequest www = UnityWebRequest.Get (url);
		yield return www.SendWebRequest ();
		if (www.isNetworkError || www.isHttpError) {
			Debug.unityLogger.LogError (TAG, "Get Text Error");
			if (null != fulFillAction)
				fulFillAction (string.Empty);
		} else {
			if (null != fulFillAction)
				fulFillAction (www.downloadHandler.text);
		}
	}
}
