#include <stdlib.h>
#include <stdio.h>
namespace NativeLoader{
    
    class MemoryBuffer;
    
    class FileLoaderStream{
        FILE *fp;
    public:
        FileLoaderStream();
        ~FileLoaderStream();
        
        void Open(const char *file);
        void Close();
        
        int ReadBlock(MemoryBuffer &writeBuffer,int size);
        
        void SeekFromTop(int idx);
        void SeekRelative(int idx);
    };

}
