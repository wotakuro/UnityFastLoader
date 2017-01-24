#include "LoaderUtil.h"
#include "3rd_party/lz4/lz4.h"

using namespace NativeLoader;

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
