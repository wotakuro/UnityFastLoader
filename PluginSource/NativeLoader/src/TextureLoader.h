#include <stdlib.h>

namespace NativeLoader{
    class FileLoaderStream;
    class MemoryBuffer;

	/***
	TextureFormat

	Offset	| Size		| Explain
	-------------------------------------------
	0		| 8			| Header('FSTTEX\0\0')
	8		| 4			| TextureWidth
	12		| 4			| TextureHeight
	16		| 4			| TextureFormat
	20		| 4			| Flags(lz4,encrypt,mipmap,flagment,)
	24		| 4			| The size of bodyData
	28		| 4			| The size of uncompressedData
	-------------------------------------------
	32		| bodySize	| TextureRawData
	-------------------------------------------
    last 4	| 4         | CRC32 of AllData
	*/
	class TextureLoader{
	private:
		int m_width;
		int m_height;

		int m_format;
		int m_flags;
        int m_compressedSize;
		int m_uncompressedSize;
        void *m_bodyPtr;
        
        MemoryBuffer *m_decompressWorking;
	public:
		TextureLoader();
		~TextureLoader();


		inline int GetWidth()const{
			return m_width;
		}
		inline int GetHeight()const{
			return m_height;
		}
		inline int GetFormat()const{
			return m_format;
		}
		inline int GetFlags()const{
			return m_flags;
		}
		inline int GetBodySize()const{
			return m_uncompressedSize;
		}


		void* GetRawData()const;

        bool LoadTexture( FileLoaderStream &stream, MemoryBuffer &readBuffer,MemoryBuffer *deflateBuffer);
	private:
		bool LoadFileHeader(FileLoaderStream &stream,MemoryBuffer &readBuffer);
        bool LoadBody( FileLoaderStream &stream,MemoryBuffer &readBuffer,MemoryBuffer *deflateBuffer);
    };
}
