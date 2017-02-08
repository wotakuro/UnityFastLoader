using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateAssetBundle {

	[MenuItem("Tools/CreateAssetBundle")]
	public static void Exec(){
		BuildPipeline.BuildAssetBundles (Application.streamingAssetsPath,
			BuildAssetBundleOptions.ChunkBasedCompression,
			EditorUserBuildSettings.activeBuildTarget);
	}
}

