#include "FileLoaderStream.h"
#include "MemoryBuffer.h"

#include <fcntl.h>
#include <unistd.h>



using namespace NativeLoader;

FileLoaderStream::FileLoaderStream():fd(-1){
}

FileLoaderStream::~FileLoaderStream(){
    Close();
    
}

void FileLoaderStream::Open(const char *file){

    this->Close();
    fd = open(file,O_RDONLY);

}

void FileLoaderStream::Close(){
    if( fd >= 0){
        close(fd);
        fd = -1;
    }
}

int FileLoaderStream::ReadBlock(MemoryBuffer &writeBuffer,int size){
    if( fd < 0 ){
        return -1;
    }
    writeBuffer.PrepareForDynamicAppend(size);
    size_t ret = read(fd, writeBuffer.GetNextAppendPtr(), size );
    writeBuffer.NextPtr(size);
    
    return (int)ret;
}

void FileLoaderStream::SeekFromTop(int idx){
    if( fd < 0 ){
        return;
    }
    lseek(fd, SEEK_SET, idx);
}

void FileLoaderStream::SeekRelative(int idx){
    if( fd < 0){
        return;
    }
    lseek(fd, SEEK_CUR, idx);
}
