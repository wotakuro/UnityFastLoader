#include "TextureLoader.h"
#include "FileLoaderStream.h"

using NativeLoader;

TextureLoader::TextureLoader(MemoryBuffer& buffer, int bufferSize){
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
bool TextureLoader::LoadBody(FileStream * stream){
}

