#include <stdlib.h>

#define USE_OPEN_GL

#ifdef USE_OPEN_GL
// if use OpenGL
#include <OpenGLES/ES3/gl.h>
#include <OpenGLES/ES3/glext.h>

#endif

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
#ifdef USE_OPEN_GL
        GLuint CreateRawTextureWithOpenGL();
#endif
        bool LoadTexture( FileLoaderStream &stream, MemoryBuffer &readBuffer,MemoryBuffer *deflateBuffer);
        
	private:
		bool LoadFileHeader(FileLoaderStream &stream,MemoryBuffer &readBuffer);
        bool LoadBody( FileLoaderStream &stream,MemoryBuffer &readBuffer,MemoryBuffer *deflateBuffer);
        
        
        /** Unity FormatList
         Alpha8             1   (ios/android)
         ARGB4444           2   (ios/android)
         RGB24              3   (ios/android)
         RGBA32             4   (ios/android)
         ARGB32             5   (ios/android)
         RGB565             7   (ios/android)
         R16                9
         DXT1               10
         DXT5               12
         RGBA4444           13  (ios/android)
         BGRA32             14
         RHalf              15
         RGHalf             16
         RGBAHalf           17
         RFloat             18
         RGFloat            19
         RGBAFloat          20
         YUY2               21
         DXT1Crunched       28
         DXT5Crunched       29
         PVRTC_RGB2         30  (ios)
         PVRTC_RGBA2        31  (ios)
         PVRTC_RGB4         32  (ios)
         PVRTC_RGBA4        33  (ios)
         ETC_RGB4           34  (android)
         ATC_RGB4           35
         ATC_RGBA8          36
         EAC_R              41
         EAC_R_SIGNED       42
         EAC_RG             43
         EAC_RG_SIGNED      44
         ETC2_RGB           45  (android)
         ETC2_RGBA1         46  (android)
         ETC2_RGBA8         47  (android)
         ASTC_RGB_4x4       48
         ASTC_RGB_5x5       49
         ASTC_RGB_6x6       50
         ASTC_RGB_8x8       51
         ASTC_RGB_10x10     52
         ASTC_RGB_12x12     53
         ASTC_RGBA_4x4      54
         ASTC_RGBA_5x5      55
         ASTC_RGBA_6x6      56
         ASTC_RGBA_8x8      57
         ASTC_RGBA_10x10    58
         ASTC_RGBA_12x12    59
         ETC_RGB4_3DS       60
         ETC_RGBA8_3DS      61
         */
        static bool IsCompressedFormat(int format);

#ifdef USE_OPEN_GL
        static int GetGLCompressedFormat(int format);

        static int GetGLInternalFormat(int format);
        static int GetGLFormat(int format);
#endif
    };
}
