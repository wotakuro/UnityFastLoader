using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

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


        public static void GetByteArrayFromFloat(float val, byte[] writeBuf, int index)
        {
            var bytes = System.BitConverter.GetBytes(val);
            for (int i = 0; i < 4; ++i)
            {
                writeBuf[i + index] = bytes[i];
            }
        }

        public static void GetByteArrayFromVector2(Vector2 vec, byte[] writeBuf, int index)
        {
            GetByteArrayFromFloat(vec.x, writeBuf, index);
            GetByteArrayFromFloat(vec.y, writeBuf, index + 4);
        }

        public static void GetByteArrayFromVector3(Vector3 vec, byte[] writeBuf, int index)
        {
            GetByteArrayFromFloat(vec.x, writeBuf, index);
            GetByteArrayFromFloat(vec.y, writeBuf, index + 4);
            GetByteArrayFromFloat(vec.y, writeBuf, index + 8);
        }

        public static void GetByteArrayFromColor(Color col, byte[] writeBuf, int index)
        {
            GetByteArrayFromFloat(col.r, writeBuf, index);
            GetByteArrayFromFloat(col.g, writeBuf, index + 4);
            GetByteArrayFromFloat(col.b, writeBuf, index + 8);
            GetByteArrayFromFloat(col.a, writeBuf, index + 12);
        }
        public static void GetByteArrayFromColor32(Color32 col, byte[] writeBuf, int index)
        {
            writeBuf[index + 0] = col.r;
            writeBuf[index + 1] = col.g;
            writeBuf[index + 2] = col.b;
            writeBuf[index + 3] = col.a;
        }
        public static void GetByteArrayFromMatrix4x4(Matrix4x4 mat, byte[] writeBuf, int index)
        {
            GetByteArrayFromFloat(mat.m00, writeBuf, index + 0);
            GetByteArrayFromFloat(mat.m01, writeBuf, index + 4);
            GetByteArrayFromFloat(mat.m02, writeBuf, index + 8);
            GetByteArrayFromFloat(mat.m03, writeBuf, index + 12);
            GetByteArrayFromFloat(mat.m10, writeBuf, index + 16);
            GetByteArrayFromFloat(mat.m11, writeBuf, index + 20);
            GetByteArrayFromFloat(mat.m12, writeBuf, index + 24);
            GetByteArrayFromFloat(mat.m13, writeBuf, index + 28);
            GetByteArrayFromFloat(mat.m20, writeBuf, index + 32);
            GetByteArrayFromFloat(mat.m21, writeBuf, index + 36);
            GetByteArrayFromFloat(mat.m22, writeBuf, index + 40);
            GetByteArrayFromFloat(mat.m23, writeBuf, index + 44);
            GetByteArrayFromFloat(mat.m30, writeBuf, index + 48);
            GetByteArrayFromFloat(mat.m31, writeBuf, index + 52);
            GetByteArrayFromFloat(mat.m32, writeBuf, index + 56);
            GetByteArrayFromFloat(mat.m33, writeBuf, index + 60);
            /*
            int size = Marshal.SizeOf(typeof(Matrix4x4));
            System.IntPtr ptr = Marshal.AllocHGlobal( size );
            Marshal.StructureToPtr(mat, ptr, false );
//            Marshal.read
            Marshal.FreeHGlobal(ptr);
             * */
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