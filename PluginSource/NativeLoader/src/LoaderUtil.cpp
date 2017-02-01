#include "LoaderUtil.h"
#include "3rd_party/lz4/lz4.h"
#include <stdio.h>

using namespace NativeLoader;

const char LoaderUtil::TEXTURE_HEADER_SIG[8] ={
    0x46,0x53,0x54,0x54,0x45,0x58,0x00,0x00,
};


int LoaderUtil::GetInt(const void *ptr){
	const unsigned char *p_byte = reinterpret_cast<const unsigned char*>(ptr);
	
	int val = (p_byte[0] << 0) +
		(p_byte[1] << 8) +
		(p_byte[2] << 16) +
		(p_byte[3] << 24);
	return val;
}


bool LoaderUtil::Uncompress(const void *src,void *dest , int srcSize , int destSize){
    
    const char *currentReadBody = reinterpret_cast<const char*>(src);
    const char *currentReadHeader = reinterpret_cast<const char*>(src);
    char *currentWriteDest = reinterpret_cast<char*>(dest);
    int blockNum = LoaderUtil::GetInt( src );
    int headerSize = 8 + blockNum * 8;

    currentReadHeader += 8;
    currentReadBody += headerSize;
    
    int result = 0;
    for( int i = 0 ; i < blockNum; ++ i ){
        int originSize = LoaderUtil::GetInt(currentReadHeader);
        currentReadHeader += 4;
        int compressedSize = LoaderUtil::GetInt(currentReadHeader);
        currentReadHeader += 4;

        int tmp = LZ4_decompress_safe(currentReadBody,currentWriteDest,compressedSize,originSize);

        currentWriteDest += originSize;
        currentReadBody += compressedSize;
        result |= tmp;
    }
    
    return (result == 0 );
}


bool LoaderUtil::CheckHeaderSig( const void *header ,const void *data , int size ){
    
    const char *s1 = reinterpret_cast<const char*>(header);
    const char *s2 = reinterpret_cast<const char*>(data);
    
    for( int i = 0 ;i < size ; ++ i ){
        if( (*s1) != (*s2)){
            return false;
        }
        ++s1;
        ++s2;
    }
    return true;
}
