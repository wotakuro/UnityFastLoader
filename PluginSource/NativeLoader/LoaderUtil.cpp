#include "LoaderUtil.h"
#include "3rd_party/lz4/lz4.h"

using namespace NativeLoader;

const char LoaderUtil::TEXTURE_HEADER_SIG[8] ={
    0x46,0x53,0x54,0x54,0x45,0x58,0x00,0x00,
};


int LoaderUtil::GetInt(void *ptr){
	unsigned char *p_byte = reinterpret_cast<unsigned char*>(ptr);
	
	int val = (p_byte[0] << 0) +
		(p_byte[1] << 8) +
		(p_byte[2] << 16) +
		(p_byte[3] << 24);
	return val;
}


bool LoaderUtil::Uncompress(const void *src,void *dest , int srcSize , int destSize){
    int result = LZ4_decompress_safe(reinterpret_cast<const char*>(src),reinterpret_cast<char*>(dest),srcSize,destSize);
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
