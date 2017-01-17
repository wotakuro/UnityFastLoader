#include "LoaderUtil.h"

using NativeLoader;

int LoaderUtil::GetInt(void *ptr){
	byte *p_byte = reinterpret_cast<byte*>(ptr);
	
	int val = (p_byte[0] << 0) +
		(p_byte[1] << 8) +
		(p_byte[2] << 16) +
		(p_byte[3] << 24);
	return val;
}