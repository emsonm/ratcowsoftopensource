#include "MFile.h"
#include "BeGuiAPI.h"
#include <stdlib.h>
#include <string.h>

//nasty global..
void *savepanel; // MSavePanel 
void *openpanel;
void *memo;

void file_io(void* sender, uint32 msg, const char *path){
  char *buffer; //for output
  void *file;   //for MFile reference
  
  buffer = (char*) malloc(27); //create some space in the buffer
  buffer = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //add domething to put in file
  
  //file output...
  file = MFile_Create(path, B_READ_WRITE | B_CREATE_FILE); //file read/write and create if not already there
  MFile_Write( (MFile*)file, buffer, strlen(buffer) ); //output buffer contents
  MFile_Free((MFile*)file); //clear the file pointer
}

void file_io3(void* sender, uint32 msg, const char *path){
  void *file;   //for MFile reference
  char *buffer; //for input
  off_t size;
  
  //file output...
  file = MFile_Create(path, B_READ_WRITE); //file read/write and create if not already there
  
  MFile_GetSize( (MFile*)file, &size );
  buffer = (char*) malloc(size); //create some space in the buffer
  
  MFile_Read( (MFile*)file, buffer, sizeof(char) * size ); //output buffer contents
  MFile_Free((MFile*)file); //clear the file pointer
  MMemo_SetText((MMemo*)memo, buffer);
  //printf("\n input:\n%s\n", buffer);
}

void file_io2(void* sender, uint32 msg, const char *path){
  void *file;   //for MFile reference
  
  //file output...
  file = MFile_Create(path, B_READ_WRITE | B_CREATE_FILE); //file read/write
  MFile_Write( (MFile*)file, MMemo_Text((MMemo*)memo), MMemo_TextLength((MMemo*)memo) ); //output buffer contents
  MFile_Free((MFile*)file); //clear the file pointer
}

/* save button click */
void doclick(void* Sender, uint32 msg){
  MSavePanel_Show((MSavePanel*) savepanel); //show the panel (this is modal)
}

/* open button click */
void doclick2(void* Sender, uint32 msg){
  MOpenPanel_Show((MOpenPanel*) openpanel); //show the panel (this is modal)
}

int main(){
  void *app;       // MApplication reference
  void *btn, *btn2;
  void *frm;
  
  // Matt 11/04/2002 : Must have a MApplication to use a MFilePanel of descendents
  app = MApplication_Create(); //create an MApplication reference
  
  frm = MApplication_GetMainForm((MApplication*)app);
  MForm_setWidth((MForm*)frm, 250);
  MForm_setHeight((MForm*)frm, 250);
  
  btn = MButton_Create(5, 25, 35, 20, "save"); 
  MForm_AddChild((MForm*)frm, (BControl*)btn);
  MButton_AttachClickDispatcher((MButton*)btn, doclick);
  
  btn2 = MButton_Create(40, 25, 75, 20, "open"); 
  MForm_AddChild((MForm*)frm, (BControl*)btn2);
  MButton_AttachClickDispatcher((MButton*)btn2, doclick2);
 
  
  savepanel = MSavePanel_Create(); //create a MSavePanel reference
  //MFilePanel_AttachDialogEvent((MFilePanel*)savepanel, file_io); //attach callback for event
  MFilePanel_AttachDialogEvent((MFilePanel*)savepanel, file_io2); //attach callback for event
  
  openpanel = MOpenPanel_Create(); //create a MSavePanel reference
  //MFilePanel_AttachDialogEvent((MFilePanel*)savepanel, file_io); //attach callback for event
  MFilePanel_AttachDialogEvent((MFilePanel*)openpanel, file_io3); //attach callback for event
  
  /* create a memo - like edit, must be done in main form's thread */ 
  memo  = MForm_AddMMemo((MForm*)frm, 30, 100, 150, 150, "test5");
  
  MApplication_Run((MApplication*)app); //run the application
  
  MApplication_Free((MApplication*)app); //clean up the app

}
