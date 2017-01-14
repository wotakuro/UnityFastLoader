#include <stdlib.h>

namespace NativeLoader{
    
    class LoaderUtil{
    public:
        static int GetInt(void *ptr);
        static int GetCrc32(void *ptr,int size);
    };
    
}
