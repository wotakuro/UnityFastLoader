#include "FileLoaderStream.h"
#include "TextureLoader.h"
#include "MemoryBuffer.h"

using namespace NativeLoader;

static TextureLoader *g_nativeTextureLoader = NULL;
static MemoryBuffer *g_loadWorkMemory = NULL;
static MemoryBuffer *g_uncompressWorkMemory = NULL;

extern "C"{
    /*
    bool FastLoad_Mount_ArchiveFile(const char *file){
    }
    bool FastLoad_UnMount_ArchiveFile(){
    }
     */

    bool FastLoad_Texture_LoadFile(char *file){
        
        if( !g_loadWorkMemory ){
            g_loadWorkMemory = MemoryBuffer::Create( 1024 * 1024 * 8 );
        }
        if( !g_uncompressWorkMemory ){
            g_uncompressWorkMemory = MemoryBuffer::Create( 1024 * 1024 * 16 );
        }
        if( ! g_nativeTextureLoader ){
            g_nativeTextureLoader = new TextureLoader();
        }
        
        FileLoaderStream stream;
        stream.Open(file);
        g_nativeTextureLoader->LoadTexture(stream, *g_loadWorkMemory, g_uncompressWorkMemory);
        stream.Close();
        return true;
    }
	// call afeter "FastLoad_Texture_LoadFile"
	//-----------
    int FastLoad_Texture_GetWidth(){
        if( g_nativeTextureLoader ){
            return g_nativeTextureLoader->GetWidth();
        }
        return 0;
    }
    int FastLoad_Texture_GetHeight(){
        if( g_nativeTextureLoader ){
            return g_nativeTextureLoader->GetHeight();
        }
        return 0;
    }
    int FastLoad_Texture_GetFormat(){
        if( g_nativeTextureLoader ){
            return g_nativeTextureLoader->GetFormat();
        }
        return 0;
    }
    int FastLoad_Texture_GetFlags(){
        if( g_nativeTextureLoader ){
            return g_nativeTextureLoader->GetFlags();
        }
        return 0;
    }
    int FastLoad_Texture_GetBodySize(){
        if( g_nativeTextureLoader ){
            return g_nativeTextureLoader->GetBodySize();
        }
        return 0;
    }
    void* FastLoad_Texture_GetRawData(){
        if(g_nativeTextureLoader){
            return g_nativeTextureLoader->GetRawData();
        }
        return NULL;
    }
    bool FastLoad_Texture_NativeCreateSupport(int format){
        return TextureLoader::IsNativeCreateSupport(format);
    }
    
#ifdef USE_OPEN_GL
    GLuint FastLoad_Texture_Create_OpenGL(){
        if( g_nativeTextureLoader ){
            return g_nativeTextureLoader->CreateRawTextureWithOpenGL();
        }
		return (GLuint)0;
    }
    
    void FastLoad_Texture_Delete_OpenGL(GLuint texture){
        if( g_nativeTextureLoader ){
            g_nativeTextureLoader->ReleaseTexture(texture);
        }
    }
#endif
	//-----------
}

