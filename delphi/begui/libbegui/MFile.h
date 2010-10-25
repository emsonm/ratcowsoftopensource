/*
  $Header: /cvsroot/befpc/begui/begui/libbegui/MFile.h,v 1.2 2002/04/23 18:37:29 memson Exp $
  
  $Revision: 1.2 $
  
  $Log: MFile.h,v $
  Revision 1.2  2002/04/23 18:37:29  memson
  *** empty log message ***

  Revision 1.1  2002/04/12 23:32:56  memson

  Added quite a bit.

  Got basic file handling soeted out. Also got the FilePanel's working (see
  example project)

  Popup menu now only responds to a right click (at last!!)

  
*/

#ifndef MFILE_H
#define MFILE_H

#include <storage/Path.h>
#include <File.h>
#include <FilePanel.h>
#include <SupportDefs.h>
#include <Control.h>

typedef void (*dialog_Message)(void* sender, uint32 msg, const char *path);//, entry_ref ref);

extern "C"{

#define SAVE_PANEL_MESSAGE 'svpm'
#define OPEN_PANEL_MESSAGE 'oppm'

}

class MFile : public BFile{
public:
  MFile(const char *path, uint32 openMode);
  MFile(const entry_ref *ref, uint32 openMode); 
  //add whatever
};

class MFilePanel; //forward...

class MDialogPlugin {
private:
  dialog_Message fDialogMsg;
  uint32 fDialogType;
public:
  MDialogPlugin(const uint32 dialogType);
  virtual ~MDialogPlugin();
  virtual void AttachDialogEvent(dialog_Message msg); 
  virtual void DoExecute(BPath path);
};

class MFilePanel : public BFilePanel, 
                   public MDialogPlugin{
public:
  MFilePanel(file_panel_mode panel_type,
             bool allow_multiple_selection = true, 
             BMessage *msg = 0,
             bool modal = true);
};

class MOpenPanel : public MFilePanel{
public:
  MOpenPanel(bool allow_multiple_selection = true,  
             bool modal = true);
};

class MSavePanel : public MFilePanel{
public:
  MSavePanel(bool allow_multiple_selection = true, 
             bool modal = true);
};

extern "C"{

MFile* MFile_Create(const char *path, uint32 openMode);
MFile* MFile_Create_Ref(const entry_ref *ref, uint32 openMode);
void MFile_Free(MFile* file);

off_t MFile_Seek(MFile* file, off_t offset, int32 seekMode);
off_t MFile_Position(MFile* file); 

ssize_t MFile_Read(MFile* file, void *buffer, size_t size); 
ssize_t MFile_ReadAt(MFile* file, off_t pos, void *buffer, size_t size); 
ssize_t MFile_Write(MFile* file, const void *buffer, size_t size); 
ssize_t MFile_WriteAt(MFile* file, off_t pos, const void *buffer, size_t size); 

status_t MFile_SetSize(MFile* file, off_t size); 
status_t MFile_GetSize(MFile* file, off_t *size); 
status_t MFile_SetTo(MFile* file, const char *path, uint32 open_mode);

//use this for both Save and Open Panels...
void MFilePanel_AttachDialogEvent(MFilePanel* dlg, dialog_Message msg);

MOpenPanel* MOpenPanel_Create(); 
void MOpenPanel_Free(MOpenPanel* dlg); 
void MOpenPanel_Show(MOpenPanel* dlg); 
void MOpenPanel_Hide(MOpenPanel* dlg); 
bool MOpenPanel_IsShowing(MOpenPanel* dlg); 

MSavePanel* MSavePanel_Create(); 
void MSavePanel_Free(MSavePanel* dlg); 
void MSavePanel_Show(MSavePanel* dlg); 
void MSavePanel_Hide(MSavePanel* dlg); 
bool MSavePanel_IsShowing(MSavePanel* dlg); 

uint test_func(void);

}

#endif MFILE_H
