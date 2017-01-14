using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class TextureAssetCreate {

	private List<string> pathList = new List<string>();

	[MenuItem("Tools/CreateTextureAsset")]
	public static void CreateTextureDatas(){
		var executer = new TextureAssetCreate ();
		executer.Execute ();
	}

	private void Execute(){
		var guids = AssetDatabase.FindAssets ("t:Texture2D");
		foreach (var guid in guids) {
			string path = AssetDatabase.GUIDToAssetPath (guid);
			pathList.Add (path);
		}
		string outputDir = "Target";
		CreateDirectory ( outputDir );

		foreach (var path in pathList) {
			Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D> (path);
			string outputPath = Path.Combine( outputDir,path );

			FileStream fs = File.OpenWrite (outputPath);
			fs.Close ();
		}
	}

	private readonly byte[] FastTextureHeader = new byte[]{};

	private byte[] GetByteArrayFromInt(int val){
		byte[] ret = new byte[4];
		for (int i = 0; i < 4; ++i) {
			ret [i] = (byte)((val >> (i * 8) ) & 0xff);
		}
		return ret;
	}

	private void CreateDirectory(string outputDir){
		HashSet< string > directorySet = new HashSet<string> ();
		foreach (var path in pathList) {
			string dir = Path.GetDirectoryName (path);
			if (!directorySet.Contains (dir)) {
				directorySet.Add (dir);
			}
		}
		foreach (string createDir in directorySet) {
			string targetPath = Path.Combine (outputDir, createDir);
			if (Directory.Exists (targetPath)) {
				Directory.CreateDirectory (targetPath);
			}
		}
	}

}
