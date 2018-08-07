using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class CABBuilder  {
	public static string overloadedDevelopmentServerURL = "";
	public static void BuildAssetBundles(BuildTarget buildTarget)
	{
		string outputPath = Path.Combine(CPlatform.ABundlesOutputPath,  CPlatform.GetPlatformName());
		if (Directory.Exists (outputPath)) {
			Directory.Delete (outputPath,true);
		}
		Directory.CreateDirectory (outputPath);
		Object[] SelectedAsset = Selection.GetFiltered(typeof(Object),SelectionMode.DeepAssets);

		//		BuildPipeline.BuildAssetBundles(outputPath,SelectedAsset,BuildAssetBundleOptions.None,buildTarget);
//		GenerateVersionConfig (outputPath);
		if(Directory.Exists(Application.streamingAssetsPath+"/QAB")){
			Directory.Delete (Application.streamingAssetsPath+"/QAB",true);
		}
		Directory.CreateDirectory (Application.streamingAssetsPath+"/QAB");
		FileUtil.ReplaceDirectory (CPlatform.ABundlesOutputPath,Application.streamingAssetsPath+"/QAB");
		AssetDatabase.Refresh ();
	}
}
