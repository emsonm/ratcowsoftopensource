#include "MFile.h"
#include "debug.h"

/////////////////////////////////////////////////
// Class MFile

MFile::MFile(const char *path, uint32 openMode): 
  BFile(path, openMode)
{

}

MFile::MFile(const entry_ref *ref, uint32 openMode):
  BFile(ref, openMode)
{

}

/////////////////////////////////////////////////
// Exported Functions

MFile* MFile_Create(const char *path, uint32 openMode){
  return new MFile(path, openMode);
}

MFile* MFile_Create_Ref(const entry_ref *ref, uint32 openMode){
  return new MFile(ref, openMode);
}

void MFile_Free(MFile* file){
  delete file;
}

off_t MFile_Seek(MFile* file, off_t offset, int32 seekMode){
  return file->Seek(offset, seekMode);
}

off_t MFile_Position(MFile* file){
  return file->Position();
}

ssize_t MFile_Read(MFile* file, void *buffer, size_t size){
  return file->Read(buffer, size);
}

ssize_t MFile_ReadAt(MFile* file, off_t pos, void *buffer, size_t size){
  return file->ReadAt(pos, buffer, size);
}

ssize_t MFile_Write(MFile* file, const void *buffer, size_t size){
  return file->Write(buffer, size);
}

ssize_t MFile_WriteAt(MFile* file, off_t pos, const void *buffer, size_t size){
  return file->WriteAt(pos, buffer, size);
}

status_t MFile_SetSize(MFile* file, off_t size){
  return file->SetSize(size);
}

status_t MFile_GetSize(MFile* file, off_t *size){
  return file->GetSize(size);
}

status_t MFile_SetTo(MFile* file, const char *path, uint32 open_mode){
  return file->SetTo(path, open_mode);
}

/////////////////////////////////////////////////
// Generic File Panel

MFilePanel::MFilePanel(file_panel_mode panel_type,
                       bool allow_multiple_selection, 
                       BMessage *msg = 0,
                       bool modal) : 
  BFilePanel(panel_type, 
             0, 
             0, 
             0, 
             allow_multiple_selection, 
             msg, 
             0, 
             modal, 
             true),
  MDialogPlugin(panel_type)
{

}

///

void MFilePanel_AttachDialogEvent(MFilePanel* dlg, dialog_Message msg)
{
  dlg->AttachDialogEvent(msg);
} 

/////////////////////////////////////////////////
// Open Panel

MOpenPanel::MOpenPanel(bool allow_multiple_selection,  
                       bool modal) : 
  MFilePanel(B_OPEN_PANEL,
             allow_multiple_selection, 
             0, 
             modal)
{
  BMessage *msg = new BMessage(OPEN_PANEL_MESSAGE);
  
  msg->AddPointer("source", this);
  
  SetMessage(msg);
}

// flattened API

MOpenPanel* MOpenPanel_Create()
{
  return new MOpenPanel();
}

void MOpenPanel_Free(MOpenPanel* dlg)
{
  delete &dlg;
}

void MOpenPanel_Show(MOpenPanel* dlg)
{
  dlg->Show();
}

void MOpenPanel_Hide(MOpenPanel* dlg)
{
  dlg->Hide();
}

bool MOpenPanel_IsShowing(MOpenPanel* dlg)
{
  return dlg->IsShowing();
}


/////////////////////////////////////////////////
// Save Panel

MSavePanel::MSavePanel(bool allow_multiple_selection, 
                       bool modal) : 
  MFilePanel(B_SAVE_PANEL,
             allow_multiple_selection, 
             0, 
             modal)
{
  BMessage *msg = new BMessage(SAVE_PANEL_MESSAGE);
  
  msg->AddPointer("source", this);
  
  SetMessage(msg);
  
}

//flattened API

MSavePanel* MSavePanel_Create()
{
  return new MSavePanel();
}

void MSavePanel_Free(MSavePanel* dlg)
{
  delete &dlg;
}

void MSavePanel_Show(MSavePanel* dlg)
{
  dlg->Show();
}

void MSavePanel_Hide(MSavePanel* dlg)
{
  dlg->Hide();
}

bool MSavePanel_IsShowing(MSavePanel* dlg)
{
  return dlg->IsShowing();
}

/////////////////////////////////////////////////
// Property

MDialogPlugin::MDialogPlugin(const uint32 dialogType)
{
  fDialogType = dialogType;
  fDialogMsg = 0;
}

MDialogPlugin::~MDialogPlugin()
{
  fDialogMsg = 0;
}

void MDialogPlugin::AttachDialogEvent(dialog_Message msg)
{
  fDialogMsg = msg;
}

void MDialogPlugin::DoExecute(BPath path)
{
  uint32 amsg;
  
  switch (fDialogType){
    case B_SAVE_PANEL:
      amsg = SAVE_PANEL_MESSAGE; //Matt 11/04/2002 : this is a bug...
      break;
    case B_OPEN_PANEL:
      amsg = OPEN_PANEL_MESSAGE; //Matt 11/04/2002 : this is a bug...
      break;
    default: amsg = 0; 
  }
  
  if (fDialogMsg != NULL){
    SendText("before the code in MDialogPlugin calls event");
    fDialogMsg( this, amsg, path.Path() ); 
    SendText("after the code in MDialogPlugin calls event");
  }
}

/////////////////////////////////////////////////
// Utility test function
uint test_func(void){
  return 1024;
}
