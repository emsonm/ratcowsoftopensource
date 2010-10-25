#ifndef MFONT_H
#define MFONT_H

/*
  $Header: /cvsroot/befpc/begui/begui/libbegui/MFont.h,v 1.2 2002/04/27 08:29:51 memson Exp $
  
  $Revision: 1.2 $
  
  $Log: MFont.h,v $
  Revision 1.2  2002/04/27 08:29:51  memson

  updated


*/

#include <Font.h>

#ifdef __cplusplus
extern "C" {
#endif 

//flattened api

BFont* BFont_Create();
BFont* BFont_Create_ref(const BFont &font);
BFont* BFont_Create_pointer(const BFont *font);
status_t BFont_SetFamilyAndStyle(BFont* fnt, const font_family family, const font_style style); 
status_t BFont_SetFamilyAndFace(BFont* fnt, const font_family family, uint16 face); 
                
void BFont_SetSize(BFont* fnt, float size); 
void BFont_SetShear(BFont* fnt, float shear); 
void BFont_SetRotation(BFont* fnt, float rotation); 
void BFont_SetSpacing(BFont* fnt, uint8 spacing); 
void BFont_SetEncoding(BFont* fnt, uint8 encoding); 
void BFont_SetFace(BFont* fnt, uint16 face); 
void BFont_SetFlags(BFont* fnt, uint32 flags); 

void BFont_GetFamilyAndStyle(BFont* fnt, font_family *family, font_style *style); 
float BFont_Size(BFont* fnt); 
float BFont_Shear(BFont* fnt); 
float BFont_Rotation(BFont* fnt); 
uint8 BFont_Spacing(BFont* fnt); 
uint8 BFont_Encoding(BFont* fnt); 
uint16 BFont_Face(BFont* fnt); 
uint32 BFont_Flags(BFont* fnt);


#ifdef __cplusplus
} 
#endif

#endif 
