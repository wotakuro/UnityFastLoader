﻿using UnityEngine;
using System.Collections;

namespace FastLoader
{
    public class FastLoaderUtil
    {

        public static readonly byte[] FastTextureHeader = new byte[]{
            0x46,0x53,0x54,0x54,0x45,0x58,0x00,0x00,
        };

        // todo
        public static readonly byte[] FastMeshPrefabHeader = new byte[]{
            0x46,0x53,0x54,0x54,0x45,0x58,0x00,0x00,
        };

        public static readonly byte[] FastDatarArchiveHeader = new byte[]{
            0x46,0x53,0x54,0x41,0x52,0x43,0x00,0x00
        };

        public static void GetByteArrayFromInt(int val,byte[] writeBuf,int index)
        {
            for (int i = 0; i < 4; ++i)
            {
                writeBuf[i + index] = (byte)((val >> (i * 8)) & 0xff);
            }
        }

        public static int GetIntFromByteArray(byte[] arr, int index)
        {
            int val = 0;
            for (int i = 0; i < 4; ++i)
            {
                val += ( ((int)(arr[index + i]) << (i * 8)));
            }
            return val;
        }


        public static void GetFloatFromByteArray(float val, byte[] writeBuf, int index)
        {
            var bytes = System.BitConverter.GetBytes(val);
            for (int i = 0; i < 4; ++i)
            {
                writeBuf[i + index] = bytes[i];
            }
        }



        public static bool CheckHeader(byte[] data, int index,byte [] header)
        {
            int length = header.Length;
            for (int i = 0; i < length; ++i)
            {
                if (data[index + i] != header[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}