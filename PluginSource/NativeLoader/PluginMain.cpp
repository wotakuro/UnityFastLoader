#include "FileLoaderStream.h"
#include "TextureLoader.h"



extern "C"{
	bool FastLoad_Mount_ArchiveFile(const char *file);
	bool FastLoad_UnMount_ArchiveFile();

	bool FastLoad_Texture_LoadFile(int idx);
	// call afeter "FastLoad_Texture_LoadFile"
	//-----------
	int FastLoad_Texture_GetWidth();
	int FastLoad_Texture_GetHeight();
	int FastLoad_Texture_GetFormat();
	int FastLoad_Texture_GetFlags();
	int FastLoad_Texture_GetBody();
	void* FastLoad_Texture_GetRawData();
	//-----------
}
