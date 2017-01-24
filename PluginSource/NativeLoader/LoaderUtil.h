#include <stdlib.h>

namespace NativeLoader{
    
    class LoaderUtil{
    public:
        static const int TEXTURE_HEAD_SIG_SIZE = 8 ;
        static const char TEXTURE_HEADER_SIG[TEXTURE_HEAD_SIG_SIZE];

        static bool Uncompress(const void *src,void *dest , int srcSize , int destSize);
        static int GetInt(void *ptr);
        static int GetCrc32(void *ptr,int size);
        static bool CheckHeaderSig( const void *header ,const void *data , int size );
    };
    
}
