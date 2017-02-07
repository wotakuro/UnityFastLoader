﻿
#if( UNITY_IOS || UNITY_ANDROID ) && !UNITY_EDITOR
#define NATIVE_PLUGIN_LOAD
#endif

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

        public bool compress
        {
            get
            {
                return FastLoaderUtil.GetCompressFromFlag(this.flags);
            }
        }
        public bool mipmap
        {
            get
            {
                return FastLoaderUtil.GetMipmapFromFlag(this.flags);
            }
        }
        public bool lenear
        {
            get
            {
                return FastLoaderUtil.GetLenearFromFlag(this.flags);
            }
        }
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
		#if !NATIVE_PLUGIN_LOAD
        private byte[] bufferData;
        private byte[] uncompressedBuffer;
		#endif

		#if NATIVE_PLUGIN_LOAD
#if UNITY_ANDROID
        private const string DllImportName = "libnativeloader";
#else 
        private const string DllImportName = "__Internal";

#endif

		[DllImport( DllImportName )]
		static extern bool FastLoad_Texture_LoadFile(string file);
		[DllImport( DllImportName )]
		static extern int FastLoad_Texture_GetWidth();
		[DllImport( DllImportName )]
		static extern int FastLoad_Texture_GetHeight();
		[DllImport( DllImportName )]
		static extern int FastLoad_Texture_GetFormat();
		[DllImport( DllImportName )]
		static extern int FastLoad_Texture_GetBodySize();
		[DllImport( DllImportName )]
		static extern IntPtr FastLoad_Texture_GetRawData();
		[DllImport( DllImportName )]
		static extern int FastLoad_Texture_GetFlags();


		[DllImport( DllImportName )]
		static extern IntPtr FastLoad_Texture_Create_OpenGL();
#endif


        private TextureData textureData;

        public TextureLoader()
        {
			#if !NATIVE_PLUGIN_LOAD
            this.bufferData = new byte[16 * 1024 * 1024];
            this.uncompressedBuffer = new byte[16 * 1024 * 1024];
			#endif
            this.textureData = new TextureData();
        }

        public void LoadToBuffer(string path)
        {
			#if NATIVE_PLUGIN_LOAD
			FastLoad_Texture_LoadFile( path );
			#else
            using (FileStream fs = File.OpenRead(path))
            {
                int read = fs.Read(bufferData, 0, bufferData.Length);
            }
			#endif
        }


        private bool CheckHeaderFromBuffer()
        {
            return true;
        }

		public Texture2D CreateTexture2DFromBuffer()
        {
            bool result = LoadDataFromBuffer();
            if (!result)
            {
                return null;
            }

			Texture2D tex = null;
			#if NATIVE_PLUGIN_LOAD
			// native load

			tex = Texture2D.CreateExternalTexture(textureData.width, textureData.heght,
				textureData.UnityFormat,textureData.mipmap,textureData.lenear,
				FastLoad_Texture_Create_OpenGL() );
			tex.UpdateExternalTexture(tex.GetNativeTexturePtr() );
			tex.filterMode = FilterMode.Bilinear;
			tex.wrapMode = TextureWrapMode.Repeat;
			#else
            tex = new Texture2D(textureData.width, textureData.heght, 
                textureData.UnityFormat,textureData.mipmap,textureData.lenear);

            tex.LoadRawTextureData(textureData.rawData, textureData.dataSize);
			tex.Apply(textureData.mipmap,true);
			#endif
            return tex;
        }

        private bool LoadDataFromBuffer()
        {
			#if NATIVE_PLUGIN_LOAD
			textureData.width = FastLoad_Texture_GetWidth();
			textureData.heght = FastLoad_Texture_GetHeight();
			textureData.format = FastLoad_Texture_GetFormat();
			textureData.flags = FastLoad_Texture_GetFlags();
			textureData.compressedSize = 0;
			textureData.dataSize = FastLoad_Texture_GetBodySize();
			textureData.rawData = FastLoad_Texture_GetRawData();

			#else
            if( !FastLoaderUtil.CheckHeader( this.bufferData ,0, FastLoaderUtil.FastTextureHeader) ){
                return false;
            }
            textureData.width = FastLoaderUtil.GetIntFromByteArray(this.bufferData, 8);
            textureData.heght = FastLoaderUtil.GetIntFromByteArray(this.bufferData, 12);
            textureData.format = FastLoaderUtil.GetIntFromByteArray(this.bufferData, 16);
            textureData.flags = FastLoaderUtil.GetIntFromByteArray(this.bufferData, 20);
            textureData.compressedSize = FastLoaderUtil.GetIntFromByteArray(this.bufferData, 24);
            textureData.dataSize = FastLoaderUtil.GetIntFromByteArray(this.bufferData, 28);

            if (textureData.compress)
            {
				Lz4Util.Decode(this.bufferData, 32, textureData.compressedSize, this.uncompressedBuffer, 0,textureData.dataSize);
                textureData.rawData = Marshal.UnsafeAddrOfPinnedArrayElement(this.uncompressedBuffer, 0);
            }
            else
            {
                textureData.rawData = Marshal.UnsafeAddrOfPinnedArrayElement(this.bufferData, 32);
            }
			#endif
            return true;
        }

    }
}