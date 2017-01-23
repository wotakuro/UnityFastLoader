#include "TextureLoader.h"
#include "FileLoaderStream.h"
#include "MemoryBuffer.h"

using namespace NativeLoader;

TextureLoader::TextureLoader(MemoryBuffer& buffer):
m_buffer(buffer)
{
}

TextureLoader::~TextureLoader(){
}


void* TextureLoader::GetRawData()const{
    return this->m_buffer.GetData(32);
}

bool TextureLoader::LoadTexture(FileLoaderStream *stream){
	bool headerFlag = this->LoadFileHeader(stream);
	if (!headerFlag){ return false; }
	bool bodyFlag = this->LoadBody(stream);
	return bodyFlag;
}
bool TextureLoader::LoadFileHeader(FileLoaderStream *stream){
	this->m_buffer.ResetData();
}
bool TextureLoader::LoadBody(FileLoaderStream * stream){
}

