#include "TextureLoader.h"
#include "FileLoaderStream.h"
#include "MemoryBuffer.h"
#include "LoaderUtil.h"


#if UNITY_ANDROID
// Android defines from open glES3
#define GL_COMPRESSED_RGB8_ETC2                          0x9274
#define GL_COMPRESSED_SRGB8_ETC2                         0x9275
#define GL_COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2      0x9276
#define GL_COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2     0x9277
#define GL_COMPRESSED_RGBA8_ETC2_EAC                     0x9278
#define GL_COMPRESSED_SRGB8_ALPHA8_ETC2_EAC              0x9279

#endif

using namespace NativeLoader;

TextureLoader::TextureLoader():
m_width(0),m_height(0),
m_format(0),m_flags(0),m_uncompressedSize(0),m_compressedSize(0),
m_bodyPtr(NULL),m_decompressWorking(NULL)
{
}

TextureLoader::~TextureLoader(){
    if( m_decompressWorking ){
        m_decompressWorking->DecReferenceCount();
        m_decompressWorking = NULL;
    }
}


void* TextureLoader::GetRawData()const{
    return m_bodyPtr;
}


#ifdef USE_OPEN_GL

GLuint TextureLoader::CreateRawTextureWithOpenGL(){
    
    
    GLint curGLTex = 0;
    glGetIntegerv(GL_TEXTURE_BINDING_2D, &curGLTex);
    
    GLuint tex;
    glGenTextures(1, &tex);
    glBindTexture(GL_TEXTURE_2D, tex);
    
    
    if( IsCompressedFormat(m_format)){
        glCompressedTexImage2D(GL_TEXTURE_2D, 0, GetGLCompressedFormat(m_format),
                               m_width, m_height, 0, m_uncompressedSize , m_bodyPtr);
    }else{
        glTexImage2D(GL_TEXTURE_2D, 0, GetGLInternalFormat(m_format),
                     m_width, m_height, 0, GetGLFormat(m_format),
                     GL_UNSIGNED_BYTE,
                     m_bodyPtr);
    }
    
    
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
    
    glBindTexture(GL_TEXTURE_2D, curGLTex);
    return tex;
}

void TextureLoader::ReleaseTexture( GLuint texture){
    glDeleteTextures(1, &texture);
}

#endif


bool TextureLoader::LoadTexture(FileLoaderStream &stream,MemoryBuffer &readBuffer,MemoryBuffer *deflateBuffer){
    bool headerFlag = this->LoadFileHeader(stream,readBuffer);
    if (!headerFlag){ return false; }
    bool bodyFlag = this->LoadBody(stream,readBuffer,deflateBuffer);
    return bodyFlag;
}



bool TextureLoader::LoadFileHeader(FileLoaderStream &stream,MemoryBuffer &readBuffer){
    readBuffer.ResetData();
    stream.ReadBlock( readBuffer, 32);
    
    if( !LoaderUtil::CheckHeaderSig(LoaderUtil::TEXTURE_HEADER_SIG, readBuffer.GetData(0), LoaderUtil::TEXTURE_HEAD_SIG_SIZE)){
        return false;
    }
    
    this->m_width = LoaderUtil::GetInt( readBuffer.GetData(8) );
    this->m_height = LoaderUtil::GetInt( readBuffer.GetData(12) );
    this->m_format = LoaderUtil::GetInt( readBuffer.GetData(16) );
    this->m_flags = LoaderUtil::GetInt( readBuffer.GetData(20) );
    
    this->m_compressedSize = LoaderUtil::GetInt( readBuffer.GetData(24) );
    this->m_uncompressedSize = LoaderUtil::GetInt( readBuffer.GetData(28) );
    
    return true;
}

bool TextureLoader::LoadBody(FileLoaderStream &stream,MemoryBuffer &readBuffer,MemoryBuffer *deflateBuffer){
    readBuffer.ResetData();
    if( !( m_flags &0x01 )){
        stream.ReadBlock(readBuffer, this->m_compressedSize) ;
        this->m_bodyPtr = readBuffer.GetData(0);
    }
    else{
        // compressed format
        stream.ReadBlock(readBuffer, this->m_compressedSize) ;
        void *ptr = NULL;
        deflateBuffer->ResetData();
        deflateBuffer->PrepareForDynamicAppend( this->m_uncompressedSize);
        ptr = deflateBuffer->GetNextAppendPtr();
        LoaderUtil::Uncompress( readBuffer.GetData(0), ptr ,
                               this->m_compressedSize,
                               this->m_uncompressedSize);
        this->m_bodyPtr = ptr;
    }
    return true;
}




/** Unity FormatList
 Alpha8             1   (ios/android)
 ARGB4444           2   (ios/android)
 RGB24              3   (ios/android)
 RGBA32             4   (ios/android)
 ARGB32             5   (ios/android)
 RGB565             7   (ios/android)
 RGBA4444           13  (ios/android)
 PVRTC_RGB2         30  (ios)
 PVRTC_RGBA2        31  (ios)
 PVRTC_RGB4         32  (ios)
 PVRTC_RGBA4        33  (ios)
 ETC_RGB4           34  (android)
 ETC2_RGB           45  (android)
 ETC2_RGBA1         46  (android)
 ETC2_RGBA8         47  (android)
 */

bool TextureLoader::IsNativeCreateSupport(int format){
    // ios/android
    switch( format){
        case 1://Alpha8
        case 3://RGB24
        case 4://RGBA32
        case 7://RGB565
        case 13://RGBA4444
            return true;
    }
#if UNITY_IOS
    switch( format){
        case 30://PVRTC_RGB2
        case 31://PVRTC_RGBA2
        case 32://PVRTC_RGB4
        case 33://PVRTC_RGBA4
            return true;
    }
#endif
    
#if UNITY_ANDROID
    switch( format){
        case 34://ETC_RGB4
        case 45://ETC2_RGB
        case 46://ETC2_RGBA1
        case 47://ETC2_RGBA8
            return true;
    }
#endif
    
    return false;
}


bool TextureLoader::IsCompressedFormat(int format){
    
    switch( format){
        case 1://Alpha8
        case 3://RGB24
        case 4://RGBA32
        case 7://RGB565
        case 13://RGBA4444
            return false;
    }
    return true;
}

#ifdef USE_OPEN_GL
int TextureLoader::GetGLCompressedFormat(int format){
#if UNITY_IOS
    switch( format){
        case 30://PVRTC_RGB2
            return GL_COMPRESSED_RGB_PVRTC_2BPPV1_IMG;
        case 31://PVRTC_RGBA2
            return GL_COMPRESSED_RGBA_PVRTC_2BPPV1_IMG;
        case 32://PVRTC_RGB4
            return GL_COMPRESSED_RGB_PVRTC_4BPPV1_IMG;
        case 33://PVRTC_RGBA4
            return GL_COMPRESSED_RGBA_PVRTC_4BPPV1_IMG;
    }
#endif
    
#if UNITY_ANDROID
    switch( format){
        case 34://ETC_RGB4
            return GL_ETC1_RGB8_OES;
        case 45://ETC2_RGB
            return GL_COMPRESSED_RGB8_ETC2;
        case 46://ETC2_RGBA1
            return GL_COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2;
        case 47://ETC2_RGBA8;
            return GL_COMPRESSED_RGBA8_ETC2_EAC;
    }
#endif

    return 0;
}

int TextureLoader::GetGLInternalFormat(int format){
    switch( format){
        case 1://Alpha8
			return GL_ALPHA;
        case 3://RGB24
            return GL_RGB;
        case 4://RGBA32
            return GL_RGBA;
        case 7://RGB565
            return GL_RGB565;
        case 13://RGBA4444
			return GL_RGBA;
    }

    return 0;
}

int TextureLoader::GetGLFormat(int format){
    // ios/android
    switch( format){
        case 1://Alpha8
			return GL_ALPHA;
        case 3://RGB24
            return GL_RGB;
        case 4://RGBA32
            return GL_RGBA;
        case 7://RGB565
            return GL_RGB565;
        case 13://RGBA4444
			return GL_RGBA;
    }
    return 0;
}

#endif


