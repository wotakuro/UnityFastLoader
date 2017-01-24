#include "TextureLoader.h"
#include "FileLoaderStream.h"
#include "MemoryBuffer.h"
#include "LoaderUtil.h"

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
    stream.ReadBlock(readBuffer, this->m_compressedSize) ;
    if( !( m_flags &0x01 )){
        this->m_bodyPtr = readBuffer.GetData(0);
    }
    else{
        void *ptr = NULL;
        deflateBuffer->ResetData();
        deflateBuffer->PrepareForDynamicAppend( this->m_uncompressedSize);
        ptr = deflateBuffer->GetNextAppendPtr();
        LoaderUtil::Uncompress( readBuffer.GetData(0), ptr , this->m_compressedSize, this->m_uncompressedSize);
        this->m_bodyPtr = ptr;
    }
    return true;
}

