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
                fs.Write(FastLoaderUtil.FastTextureHeader, 0, FastLoaderUtil.FastTextureHeader.Length);
                fs.Write(FastLoaderUtil.GetByteArrayFromInt(texture.width), 0, 4);
                fs.Write(FastLoaderUtil.GetByteArrayFromInt(texture.height), 0, 4);
                fs.Write(FastLoaderUtil.GetByteArrayFromInt((int)texture.format), 0, 4);
                fs.Write(FastLoaderUtil.GetByteArrayFromInt((int)0), 0, 4); // todo flag

                fs.Write(FastLoaderUtil.GetByteArrayFromInt(rawData.Length), 0, 4);
                fs.Write(FastLoaderUtil.GetByteArrayFromInt(rawData.Length), 0, 4);
                fs.Write(rawData, 0, rawData.Length);
                fs.Write(FastLoaderUtil.GetByteArrayFromInt((int)0), 0, 4); // todo crc32

                fs.Close();
            }
        }

        private int GetFlagFromTexture(Texture2D texture, bool isCompressed)
        {

            return 0;
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