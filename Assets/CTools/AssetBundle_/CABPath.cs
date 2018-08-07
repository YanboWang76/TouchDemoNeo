﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CABPath  {
	private CABPath(){}
	/// <summary>
	/// 资源输出的路径
	/// </summary>
	public static string ABBuildOutPutDir(RuntimePlatform platform) {
		string retDirPath = null;
		switch (platform) {
		case RuntimePlatform.Android:
			retDirPath = Application.streamingAssetsPath + "/QAB/Android";
			break;
		case RuntimePlatform.IPhonePlayer:
			retDirPath = Application.streamingAssetsPath + "/QAB/iOS";
			break;
		case RuntimePlatform.WindowsPlayer:
		case RuntimePlatform.WindowsEditor:
			retDirPath = Application.streamingAssetsPath + "/QAB/Windows";
			break;
		case RuntimePlatform.OSXPlayer:
		case RuntimePlatform.OSXEditor:
			retDirPath = Application.streamingAssetsPath + "/QAB/OSX";
			break;
		}

		return retDirPath;
	}

	/// <summary>
	/// 打包之前的源资源文件
	/// </summary>
	public static string SrcABDir  {
		get {
			return Application.dataPath + "/QArt/QAB";
		}
	}
}
