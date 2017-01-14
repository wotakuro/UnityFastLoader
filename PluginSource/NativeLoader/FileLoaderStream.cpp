#include "FileLoaderStream.h"

using NativeLoader;

FileLoaderStream::FileLoaderStream():fp(NULL){
}

FileLoaderStream::~FileLoaderStream(){
    Close();
}

void FileLoaderStream::Open(const char *file){
    this->Close();
    fp = fopen( file ,"rb" );
}

void FileLoaderStream::Close(){
    if( fp != NULL){
        fclose(fp);
        fp = NULL;
    }
}

int FileLoaderStream::ReadBlock(MemoryBuffer &writeBuffer,int size){
    if( fp == NULL){
        return;
    }
    writeBuffer->PrepareForDynamicAppend(size);

    int ret = fread( writeBuffer->GetNextAppendPtr(),size,  1 , fp);
    writeBuffer->NextPtr(size);
    
    return ret;
}

void FileLoaderStream::SeekFromTop(int idx){
    if( fp == NULL){
        return;
    }
    fseek(fp,inx , SEEK_SET);
}

void FileLoaderStream::SeekRelative(int idx){
    if( fp == NULL){
        return;
    }
    fseek(fp,inx , SEEK_CUR);
}
