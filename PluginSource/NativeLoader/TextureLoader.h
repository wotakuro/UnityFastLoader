#include <stdlib.h>

namespace NativeLoader{

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
	28		| 4			| The size of decompressedData
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
		int m_bodySize;

		void *m_workingMemory;
		int m_workingMemorySize;
		MemoryBuffer &m_buffer;

	public:
		TextureLoader(MemoryBuffer& buffer, int bufferSize );
		~TextureLoader();


		int GetWidth()const;
		int GetHeight()const;
		int GetFormat()const;
		int GetFlags()const;
		int GetBodySize()const;
		void* GetRawData()const;

        bool LoadFileHeader( FileLoaderStream *stream);
        bool LoadTexture( FileLoaderStream *stream);
    private:
        void LoadBody( FileStream * stream);
    };
}

extern "C"{
    bool FastLoad_Mount_ArchiveFile(const char *file);
    bool FastLoad_UnMount_ArchiveFile();
    
    bool FastLoad_Texture_LoadFile(int idx);
    // call afeter "FastLoad_Texture_LoadFile"
    //-----------
	int FastLoad_Texture_GetWidth();
	int FastLoad_Texture_GetHeight();
	int FastLoad_Texture_GetFormat();
	int FastLoad_Texture_GetFlags();
	int FastLoad_Texture_GetBody();
	void* FastLoad_Texture_GetRawData();
    //-----------
}
