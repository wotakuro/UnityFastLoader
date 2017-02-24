using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FastLoader
{
    public class TextureLoaderUtil
    {

        public static int GetFlagInt(bool compress, bool mipmap, bool lenear)
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
    }
}