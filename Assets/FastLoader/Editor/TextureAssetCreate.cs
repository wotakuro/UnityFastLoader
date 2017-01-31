using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace FastLoader
{
    public class TextureAssetCreate
    {

        private List<string> pathList = new List<string>();
		private byte[] lz4Buffer;

        [MenuItem("Tools/CreateTextureAsset")]
        public static void CreateTextureDatas()
        {
            var executer = new TextureAssetCreate();
            executer.Execute();
        }

		private TextureAssetCreate(){
			this.lz4Buffer = new byte[32 * 1024 * 1024];
		}

        private void Execute()
        {
            var guids = AssetDatabase.FindAssets("t:Texture2D");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                pathList.Add(path);
            }
            string outputDir = "Target/" + EditorUserBuildSettings.activeBuildTarget.ToString();
            CreateDirectory(outputDir);

            foreach (var path in pathList)
            {
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                byte[] rawData = texture.GetRawTextureData();
                string outputPath = Path.Combine(outputDir, path) + ".bin";

                FileStream fs = File.OpenWrite(outputPath);
				WriteTextureToStream(fs, texture, true , 1024* 256);
                fs.Close();
				// Copy To Streaming
				string copyFile = Path.Combine(Application.streamingAssetsPath, Path.GetFileName(outputPath) );
				File.Delete (copyFile);
				File.Copy(outputPath , copyFile);
            }
        }

		private void WriteTextureToStream(Stream stream,Texture2D texture ,bool isCompress,int blockSize)
        {
            byte[] rawData = texture.GetRawTextureData();
            byte[] writeData = null;
			int writeDataSize = 0;
            if (isCompress)
            {
				writeData = this.lz4Buffer;
				writeDataSize = Lz4Util.Encode (blockSize , rawData, 0, rawData.Length, writeData, 0, writeData.Length);
				Debug.Log ("Write " + rawData.Length + "->" + writeDataSize);
            }
            else
            {
                writeData = rawData;
				writeDataSize = rawData.Length;
            }
			byte[] fileData = new byte[writeDataSize + 32 ];

            int flag = FastLoaderUtil.GetFlagInt(isCompress,(texture.mipmapCount > 1), (texture.filterMode != FilterMode.Point));

            // write header
            System.Array.Copy(FastLoaderUtil.FastTextureHeader, 0, fileData, 0, FastLoaderUtil.FastTextureHeader.Length);
            FastLoaderUtil.GetByteArrayFromInt(texture.width, fileData, 8);
            FastLoaderUtil.GetByteArrayFromInt(texture.height, fileData, 12);
            FastLoaderUtil.GetByteArrayFromInt((int)texture.format, fileData, 16);
            FastLoaderUtil.GetByteArrayFromInt(flag, fileData, 20);
			FastLoaderUtil.GetByteArrayFromInt(writeDataSize, fileData, 24);
            FastLoaderUtil.GetByteArrayFromInt(rawData.Length, fileData, 28);

			System.Array.Copy(writeData, 0, fileData, 32, writeDataSize);
            stream.Write(fileData,0,fileData.Length);
            // calc crc32
            byte[] crc32 = new byte[4];
            FastLoaderUtil.GetByteArrayFromInt(Crc32.GetValue(fileData, 0, fileData.Length), crc32, 0);
            stream.Write(crc32, 0, crc32.Length);
        }


        private void CreateDirectory(string outputDir)
        {
            HashSet<string> directorySet = new HashSet<string>();
            foreach (var path in pathList)
            {
                string dir = Path.GetDirectoryName(path);
                if (!directorySet.Contains(dir))
                {
                    directorySet.Add(dir);
                }
            }
            foreach (string createDir in directorySet)
            {
                string targetPath = Path.Combine(outputDir, createDir);
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
            }
        }

    }
}