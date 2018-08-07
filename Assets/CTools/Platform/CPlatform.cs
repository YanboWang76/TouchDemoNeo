using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlatform  {
	public static string ABundlesOutputPath{get{ return Application.dataPath;}}
	public static string GetPlatformName()
	{
		string platformName;
		switch (Application.platform) {
		case RuntimePlatform.Android:
			platformName = "Android";
			break;
		case RuntimePlatform.WindowsPlayer:
		case RuntimePlatform.WindowsEditor:
			platformName = "Win";
			break;
		case RuntimePlatform.IPhonePlayer:
			platformName = "IOS";
			break;
		default :
			platformName = "None";
			break;
		}
		return platformName;
	}
}
