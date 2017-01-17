#include "FileLoaderSteram.h"

namespace NativeLoader{
    
    /***
     Archieve Format
     
     Offset         | Size		| Explain
     -------------------------------------------
     0              | 8			| Header('FSTARC\0\0')
     8              | 4         | Numdber of data
     12             | 4         | FileNameSize( FNameSize ) aligned by 16 byte
     -------------------------------------------
     N * 8 + 0      | 4         | DataType
     N * 12 + 4     | 4         | DataIndex( aligned by 16 byte)
     N * 12 + 4     | 4         | DataSize
     ----------- X NumberOfData ---------------------
     16 + Num * 12  | FNameSize | Filenames( separated by ¥0 )
                    | 4         | CRC32 of Files data
                    | 12        | 0 fill
     ------------------------------------------------
     DataIndex[N]   |DataSize[N]| ArchivedData
     ----------- X NumberOfData ---------------------
     Last 4         | 4         | CRC32 of All data
     */
    class ArchivedData{
        int m_dataNum;
        int m_headerSize;
        int *m_sizeOfFile;
        int *m_typeOfFile;
        char *m_filenameBlock;
        char **m_filenames;
        
        FileLoaderStream stream;
        
    public:
        static const int TYPE_TEXTURE = 1;
        
    public:
        void Mount(const char *file);
        void Unmount();
        
        int GetType(int idx)const;
        int GetSize(int idx)const;
        const char* GetFileName(int idx)const;
        
        FileLoaderStream* GetLoaderStream(int idx);
    };

}
