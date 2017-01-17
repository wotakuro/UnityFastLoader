using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace FastLoader
{
    public class TextureAssetCreate
    {

        private List<string> pathList = new List<string>();

        [MenuItem("Tools/CreateTextureAsset")]
        public static void CreateTextureDatas()
        {
            var executer = new TextureAssetCreate();
            executer.Execute();
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
                WriteTextureToStream(fs, texture, true);
                fs.Close();
            }
        }

        private void WriteTextureToStream(Stream stream,Texture2D texture ,bool isCompress)
        {
            byte[] rawData = texture.GetRawTextureData();
            byte[] writeData = null;
            if (isCompress)
            {
                writeData = LZ4.LZ4Codec.Encode(rawData, 0, rawData.Length);
            }
            else
            {
                writeData = rawData;
            }
            byte[] fileData = new byte[writeData.Length + 32 ];

            int flag = FastLoaderUtil.GetFlagInt(isCompress,(texture.mipmapCount > 1), (texture.filterMode != FilterMode.Point));

            // write header
            System.Array.Copy(FastLoaderUtil.FastTextureHeader, 0, fileData, 0, FastLoaderUtil.FastTextureHeader.Length);
            FastLoaderUtil.GetByteArrayFromInt(texture.width, fileData, 8);
            FastLoaderUtil.GetByteArrayFromInt(texture.height, fileData, 12);
            FastLoaderUtil.GetByteArrayFromInt((int)texture.format, fileData, 16);
            FastLoaderUtil.GetByteArrayFromInt(flag, fileData, 20);
            FastLoaderUtil.GetByteArrayFromInt(writeData.Length, fileData, 24);
            FastLoaderUtil.GetByteArrayFromInt(rawData.Length, fileData, 28);

            System.Array.Copy(writeData, 0, fileData, 32, writeData.Length);
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