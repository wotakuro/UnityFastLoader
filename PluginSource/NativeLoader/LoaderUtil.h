#include <stdlib.h>

namespace NativeLoader{
    
    class LoaderUtil{
    public:
        static const int TEXTURE_HEAD_SIG_SIZE = 8 ;
        static const char TEXTURE_HEADER_SIG[TEXTURE_HEAD_SIG_SIZE];

        
        /* block struct
         * -----------------
         * | 0 | 4 | blockNum
         * | 4 | 4 | 0
         * ---------------
         * | 8 + N*8 + 0 | 4 | uncompressedSize
         * | 8 + N*8 + 4 | 4 | compressedSize
         * ---------------
         * blockData 
         * ---------------
         */
        static bool Uncompress(const void *src,void *dest , int srcSize , int destSize);
        
        static int GetInt(const void *ptr);
        static int GetCrc32(void *ptr,int size);
        static bool CheckHeaderSig( const void *header ,const void *data , int size );
    };
    
}
