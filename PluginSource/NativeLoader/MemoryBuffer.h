#include <stdlib.h>

namespace NativeLoader{
    class MemoryBuffer{
    private:
        void *m_workingMemory;
        int m_workingMemorySize;
        int m_referenceCount;
        int m_ptrIndex;
        
    public:
        static MemoryBuffer& Create(int size);
        
        void* GetData(int index)const;
        int GetDataSize()const;
        
        void ResetData();
        void AppendData(const void *ptr,int size);

        void PrepareForDynamicAppend(int size);
        void *GetNextAppendPtr();
        int NextPtr( int size );
        
        void Resize(int size);
        void Shrink(int size);
        
        void IncReferenceCount();
        void DecReferenceCount();
        
    private:
        MemoryBuffer(int size);
        ~MemoryBuffer();
    };
    
}
