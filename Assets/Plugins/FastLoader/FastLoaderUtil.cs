using UnityEngine;
using System.Collections;

namespace FastLoader
{
    public class FastLoaderUtil
    {

        public static readonly byte[] FastTextureHeader = new byte[]{
            0x46,0x53,0x54,0x54,0x45,0x58,0x00,0x00,
        };

        public static readonly byte[] FastDatarArchiveHeader = new byte[]{
            0x46,0x53,0x54,0x41,0x52,0x43,0x00,0x00
        };

        public static byte[] GetByteArrayFromInt(int val)
        {
            byte[] ret = new byte[4];
            for (int i = 0; i < 4; ++i)
            {
                ret[i] = (byte)((val >> (i * 8)) & 0xff);
            }
            return ret;
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
    }
}