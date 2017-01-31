using UnityEngine;
using System.Collections;

using LZ4;
namespace FastLoader
{
	public class Lz4Util
	{
		public static int BlockSize = 128 * 1024;

		/*
		 * -----------------
		 * | 0 | 4 | blockNum
		 * | 4 | 4 | 0
		 * ---------------
		 * | 8 + N*8 + 0 | 4 | uncompressedSize
		 * | 8 + N*8 + 4 | 4 | compressedSize
		 * ---------------
		 * blockData 
		 * ---------------
		 */
		public static int Encode( byte []src ,int srcOffset , int srcLength ,byte []dest , int destOffset,int destLength ){

			int blockNum = (src.Length + BlockSize -1 )/ BlockSize;
			int headerSize = 8 + blockNum * 8;

			FastLoaderUtil.GetByteArrayFromInt (blockNum, dest, destOffset + 0);
			FastLoaderUtil.GetByteArrayFromInt (0, dest, destOffset + 4);

			int sizeSum = headerSize;
			int currentDestOffset = destOffset + headerSize;
			for (int i = 0; i < blockNum; ++i) {
				// write to data
				int originSize = BlockSize;
				if ( BlockSize + (i * BlockSize) > srcLength ) {
					originSize = srcLength - (i * BlockSize);
				}
				int compressedSize = LZ4Codec.Encode (src, srcOffset + ( i * BlockSize), originSize, dest, 
					currentDestOffset, destLength - currentDestOffset + destOffset);
				// write to header block
				FastLoaderUtil.GetByteArrayFromInt ( originSize, dest, destOffset + 8 + (i * 8) + 0);
				FastLoaderUtil.GetByteArrayFromInt ( compressedSize, dest, destOffset + 8 + (i * 8) + 4 );


				currentDestOffset += compressedSize;
				sizeSum += compressedSize;
			}
			return sizeSum;
		}
		public static int Decode( byte []src ,int srcOffset , int srcLength,byte []dest , int destOffset,int destLength ){
			int blockNum = FastLoaderUtil.GetIntFromByteArray( src , srcOffset + 0 );
			int headerSize = 8 + blockNum * 8;
			int currentDestOffset = destOffset;
			int currentSrcOffset = srcOffset + headerSize;

			int destLengthSum = 0;

			for (int i = 0; i < blockNum; ++i) {
				int originSize = FastLoaderUtil.GetIntFromByteArray( src , srcOffset + 8 + (i*8)+0 );
				int compressedSize = FastLoaderUtil.GetIntFromByteArray( src , srcOffset + 8 + (i*8)+4 );


				LZ4Codec.Decode (src, currentSrcOffset, compressedSize, dest, currentDestOffset, originSize);

				currentSrcOffset += compressedSize;
				currentDestOffset += originSize;
				destLengthSum += originSize;
			}

			return destLengthSum;
		}
	}
}
