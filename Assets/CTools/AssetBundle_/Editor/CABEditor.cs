using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CABEditor : MonoBehaviour {
	[MenuItem("CFramework/AB/Build iOS")]
	public static void BuildABiOS()
	{
		string outputPath = CABPath.ABBuildOutPutDir (RuntimePlatform.IPhonePlayer);

		CIO.CreatDir (outputPath);

		CABBuilder.BuildAssetBundles (BuildTarget.iOS);

		AssetDatabase.Refresh ();
	}

	[MenuItem("CFramework/AB/Build Android")]
	public static void BuildABAndroid()
	{
		string outputPath = CABPath.ABBuildOutPutDir (RuntimePlatform.Android);

		CIO.CreatDir(outputPath);

		CABBuilder.BuildAssetBundles (BuildTarget.Android);

		AssetDatabase.Refresh ();

	}

}
