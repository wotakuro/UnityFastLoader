#include <stdlib.h>

namespace NativeLoader{
    
    class LoaderUtil{
    public:
        static bool Uncompress(const void *src,void *dest , int srcSize , int destSize);
        static int GetInt(void *ptr);
        static int GetCrc32(void *ptr,int size);
    };
    
}
