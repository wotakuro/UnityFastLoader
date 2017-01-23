#include "LoaderUtil.h"

using namespace NativeLoader;

int LoaderUtil::GetInt(void *ptr){
	unsigned char *p_byte = reinterpret_cast<unsigned char*>(ptr);
	
	int val = (p_byte[0] << 0) +
		(p_byte[1] << 8) +
		(p_byte[2] << 16) +
		(p_byte[3] << 24);
	return val;
}
