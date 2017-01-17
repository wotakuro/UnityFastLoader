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

        public static int GetFlagInt(bool compress,bool mipmap,bool lenear)
        {
            int val = 0;
            if (compress) { val |= 0x01; }
            if (mipmap) { val |= 0x02; }
            if (lenear) { val |= 0x04; }
            return val;
        }

        public static bool GetCompressFromFlag(int val)
        {
            return ((val & 0x01) != 0);
        }
        public static bool GetMipmapFromFlag(int val)
        {
            return ((val & 0x02) != 0);
        }
        public static bool GetLenearFromFlag(int val)
        {
            return ((val & 0x04) != 0);
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