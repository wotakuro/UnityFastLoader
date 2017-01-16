using UnityEngine;
using System;
using System.Collections;

using System.IO;
using System.Runtime.InteropServices;

namespace FastLoader
{
    public struct TextureData
    {

        int width;
        int heght;
        int format;
        int flags;

        IntPtr textureData;
        int size;

    }

    public class TextureLoader
    {
        private byte[] bufferData;

        private TextureLoader()
        {
            this.bufferData = new byte[16 * 1024 * 1024];
        }

        private void LoadToBuffer(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                fs.Read(bufferData, 0, bufferData.Length);
            }
        }

        public Texture2D CreateTexture2DFromBuffer()
        {
            Marshal.UnsafeAddrOfPinnedArrayElement(this.bufferData, 0);
            return null;
        }

        private int GetWidthFromBuffer()
        {
            return 0;
        }
        private int GetHeighhFromBuffer()
        {
            return 0;
        }
        private TextureFormat GetUnityTextureFormat()
        {
            return TextureFormat.Alpha8;
        }

        private bool IsUseMipMap()
        {
            return false;
        }
    }
}