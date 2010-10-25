#include "MFont.h"
#include "debug.h"

BFont* BFont_Create(){
  return new BFont();
}

BFont* BFont_Create_ref(const BFont &font){
  return new BFont(font);
}

BFont* BFont_Create_pointer(const BFont *font){
 return new BFont(font);
}

status_t BFont_SetFamilyAndStyle(BFont* fnt, const font_family family, const font_style style){
  return fnt->SetFamilyAndStyle(family, style);
}
 
status_t BFont_SetFamilyAndFace(BFont* fnt, const font_family family, uint16 face){
    return fnt->SetFamilyAndFace(family, face);
} 
                
void BFont_SetSize(BFont* fnt, float size){
  fnt->SetSize(size);
} 

void BFont_SetShear(BFont* fnt, float shear){
  fnt-> SetShear(shear);
} 

void BFont_SetRotation(BFont* fnt, float rotation){
  fnt->SetRotation(rotation);
} 

void BFont_SetSpacing(BFont* fnt, uint8 spacing){
  fnt->SetSpacing(spacing);
} 

void BFont_SetEncoding(BFont* fnt, uint8 encoding){
  fnt->SetEncoding(encoding);
} 

void BFont_SetFace(BFont* fnt, uint16 face){
  fnt->SetFace(face);
} 

void BFont_SetFlags(BFont* fnt, uint32 flags){
  fnt->SetFlags(flags);
} 

void BFont_GetFamilyAndStyle(BFont* fnt, font_family *family, font_style *style){
  fnt->GetFamilyAndStyle(family, style);
} 

float BFont_Size(BFont* fnt){
  return fnt->Size();
} 

float BFont_Shear(BFont* fnt){
  return fnt->Shear();
} 

float BFont_Rotation(BFont* fnt){
  return fnt->Rotation();
}
 
uint8 BFont_Spacing(BFont* fnt){
  return fnt->Spacing();
} 

uint8 BFont_Encoding(BFont* fnt){
  return fnt->Encoding();
} 

uint16 BFont_Face(BFont* fnt){
  return fnt->Face();
} 

uint32 BFont_Flags(BFont* fnt){
  return fnt->Flags();
}
