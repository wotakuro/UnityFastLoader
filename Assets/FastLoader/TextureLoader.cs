using UnityEngine;
using System;
using System.Collections;

using System.IO;
using System.Runtime.InteropServices;

namespace FastLoader
{
    public struct TextureData
    {

        public int width;
        public int heght;
        public int format;
        public int flags;

        public IntPtr rawData;
        public int dataSize;
        public int compressedSize;

        public TextureFormat UnityFormat
        {
            get
            {
                return (TextureFormat)format;
            }
        }
    }

    public class TextureLoader
    {
        private byte[] bufferData;

        private TextureData textureData;

        public TextureLoader()
        {
            this.bufferData = new byte[16 * 1024 * 1024];
            this.textureData = new TextureData();
        }

        public void LoadToBuffer(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                int read = fs.Read(bufferData, 0, bufferData.Length);
            }
        }


        private bool CheckHeaderFromBuffer()
        {
            return true;
        }

        public Texture2D CreateTexture2DFromBuffer()
        {
            this.LoadDataFromBuffer();

            Texture2D tex = new Texture2D(textureData.width, textureData.heght, textureData.UnityFormat,false);

            tex.LoadRawTextureData(textureData.rawData, textureData.dataSize);
            tex.Apply();
            return tex;
        }


        private void LoadDataFromBuffer()
        {
            textureData.width = FastLoaderUtil.GetIntFromByteArray(this.bufferData, 8);
            textureData.heght = FastLoaderUtil.GetIntFromByteArray(this.bufferData, 12);
            textureData.format = FastLoaderUtil.GetIntFromByteArray(this.bufferData, 16);
            textureData.flags = FastLoaderUtil.GetIntFromByteArray(this.bufferData, 20);
            textureData.compressedSize = FastLoaderUtil.GetIntFromByteArray(this.bufferData, 24);
            textureData.dataSize = FastLoaderUtil.GetIntFromByteArray(this.bufferData, 28);

            textureData.rawData = Marshal.UnsafeAddrOfPinnedArrayElement(this.bufferData, 32);

        }

    }
}