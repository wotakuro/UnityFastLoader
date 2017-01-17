using UnityEngine;
using System.Collections;

namespace FastLoader
{
    public class Crc32
    {
        private const int TABLE_LENGTH = 256;
        private static uint[] crcTable;

 
        static Crc32()
        {
            if (crcTable != null) { return; }
            crcTable = new uint[TABLE_LENGTH];
            for (uint i = 0; i < TABLE_LENGTH; i++)
            {
                var x = i;
                for (var j = 0; j < 8; j++)
                {
                    x = (uint)((x & 1) == 0 ? x >> 1 : -306674912 ^ x >> 1);
                }
                crcTable[i] = x;
            }
        }

        public static int GetValue(byte[] data,int start,int length)
        {
            uint num = uint.MaxValue;
            for (var i = start; i < length; i++)
            {
                num = crcTable[(num ^ data[i]) & 0xff] ^ num >> 8;
            }

            uint val = (uint)(num ^ -1);

            if (val < 0x80000000)
            {
                return (int)val;
            }
            return (int)(-(0xffffffff - val)) -1;
        }
    }
}
