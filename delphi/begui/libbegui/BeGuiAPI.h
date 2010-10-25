#ifndef BEGUIAPI_H
#define BEGUIAPI_H

#include <Application.h>
#include <Window.h>
#include <View.h>
#include <Control.h>
#include <Button.h>
#include <TextControl.h>
#include <ScrollView.h>
#include <Rect.h>
#include <String.h>
#include <Message.h>
#include <Alert.h>
#include <Menu.h>
#include <MenuItem.h>
#include <PopUpMenu.h>
#include <MenuBar.h>
#include "MFont.h"
#include "MList.h"
#include "BeGuiClasses.h"


////////////////////////////////////////////////////////////////////

typedef void (*base_Message)(void* sender, uint32 msg); //callback event 
typedef void (*mouseMoved_Message)(void* sender, float x, float y, uint32 code, uint32 msg); 
typedef void (*mouseAction_Message)(void* sender, float x, float y); 
typedef void (*keyAction_Message)(void* sender, const char *bytes, int32 numBytes); 
typedef void (*drawAction_Message)(void* sender, float left, float top, float right, float bottom); 

//Exports...

#ifdef __cplusplus
extern "C" {
#endif 

int test(void);
CTest* CTest_Create(void);
int CTest_GetValue(CTest* ptr);
void CTest_Free(CTest* ptr);
void CTest_SetValue(CTest* ptr, int val);

////////////////////////////////////////////////////////////////////////////////////
// application
////////////////////////////////////////////////////////////////////////////////////
MApplication* MApplication_Create(void);
void MApplication_Free(MApplication *app);
void MApplication_Run(MApplication *app);
MForm* MApplication_GetMainForm(MApplication *app);
MForm* MApplication_AddForm(MApplication *app, float left, float top, float right, float bottom, char *name, char *caption);
MForm* MApplication_AddForm_int32(MApplication *app, int32 left, int32 top, int32 right, int32 bottom, char *name, char *caption);

////////////////////////////////////////////////////////////////////////////////////
// form
////////////////////////////////////////////////////////////////////////////////////
void MForm_AddChild(MForm *form, BControl* ctrl);
void MForm_AttachMouseMovedDispatcher(MForm *form, mouseMoved_Message msg);
void MForm_AttachMouseDownDispatcher(MForm *form, mouseAction_Message msg);
void MForm_AttachMouseUpDispatcher(MForm *form, mouseAction_Message msg);
void MForm_AttachKeyDownDispatcher(MForm *form, keyAction_Message msg);
void MForm_AttachKeyUpDispatcher(MForm *form, keyAction_Message msg);
void MForm_AttachDrawDispatcher(MForm *form, drawAction_Message msg);
void MForm_Show(MForm *frm);
void MForm_Hide(MForm *frm);
void MForm_AddPopUpMenu(MForm *frm, MPopUpMenu *mni);
int32 MForm_getWidth(MForm *frm);
int32 MForm_getHeight(MForm *frm);
void MForm_setWidth(MForm *frm, int32 value);
void MForm_setHeight(MForm *frm, int32 value);

////////////////////////////////////////////////////////////////////////////////////
//button
////////////////////////////////////////////////////////////////////////////////////
MButton* MButton_Create(float left, float top, float right, float bottom, char *caption);
MButton* MButton_Create_int32(int32 left, int32 top, int32 right, int32 bottom, char *caption);
void  MButton_AttachClickDispatcher(MButton *btn, base_Message msg);
char* MButton_getCaption(MButton *btn);
void  MButton_setCaption(MButton *btn, char* caption);
void  MButton_AttachMouseMovedDispatcher(MButton *btn, mouseMoved_Message msg);
void  MButton_AttachMouseDownDispatcher(MButton *btn, mouseAction_Message msg);
void  MButton_AttachMouseUpDispatcher(MButton *btn, mouseAction_Message msg);
void  MButton_AttachKeyDownDispatcher(MButton *btn, keyAction_Message msg);
void  MButton_AttachKeyUpDispatcher(MButton *btn, keyAction_Message msg);
void  MButton_AttachDrawDispatcher(MButton *btn, drawAction_Message msg);

////////////////////////////////////////////////////////////////////////////////////
//edit
////////////////////////////////////////////////////////////////////////////////////
MEdit* MForm_AddMEdit(MForm *form, float left, float top, float right, float bottom, char *caption);
MEdit* MForm_AddMEdit_int32(MForm *form, int32 left, int32 top, int32 right, int32 bottom, char *caption);
char*  MEdit_getText(MEdit* edt);
void   MEdit_setText(MEdit* edt, char* text);
void   MEdit_AttachClickDispatcher(MEdit *edt, base_Message msg);
void   MEdit_AttachMouseMovedDispatcher(MEdit *edt, mouseMoved_Message msg);
void   MEdit_AttachMouseDownDispatcher(MEdit *edt, mouseAction_Message msg);
void   MEdit_AttachMouseUpDispatcher(MEdit *edt, mouseAction_Message msg);
void   MEdit_AttachKeyDownDispatcher(MEdit *edt, keyAction_Message msg);
void   MEdit_AttachKeyUpDispatcher(MEdit *edt, keyAction_Message msg);
void   MEdit_AttachDrawDispatcher(MEdit *edt, drawAction_Message msg);

////////////////////////////////////////////////////////////////////////////////////
//memo
////////////////////////////////////////////////////////////////////////////////////
MMemo* MForm_AddMMemo(MForm *form, float left, float top, float right, float bottom, char *caption);
MMemo* MForm_AddMMemo_int32(MForm *form, int32 left, int32 top, int32 right, int32 bottom, char *caption);
void   MMemo_AttachMouseMovedDispatcher(MMemo *memo, mouseMoved_Message msg);
void   MMemo_AttachMouseDownDispatcher(MMemo *memo, mouseAction_Message msg);
void   MMemo_AttachMouseUpDispatcher(MMemo *memo, mouseAction_Message msg);
void   MMemo_AttachKeyDownDispatcher(MMemo *memo, keyAction_Message msg);
void   MMemo_AttachKeyUpDispatcher(MMemo *memo, keyAction_Message msg);
void   MMemo_AttachDrawDispatcher(MMemo *memo, drawAction_Message msg);
const char* MMemo_Text(MMemo *memo);
int32 MMemo_TextLength(MMemo *memo); 
void MMemo_SetText(MMemo *memo, char *text);

////////////////////////////////////////////////////////////////////////////////////
//panel
////////////////////////////////////////////////////////////////////////////////////
MPanel* MForm_AddMPanel(MForm* frm, float left, float top, float right, float bottom, char *name);
MPanel* MForm_AddMPanel_int32(MForm* frm, int32 left, int32 top, int32 right, int32 bottom, char *name);
void MPanel_AddChild(MPanel *panel, BControl* ctrl);

////////////////////////////////////////////////////////////////////////////////////
//popupmenu
////////////////////////////////////////////////////////////////////////////////////
MPopUpMenu* MPopUpMenu_Create(const char *name);
void MPopUpMenu_AddItem(MPopUpMenu *mnu, MMenuItem *itm);

////////////////////////////////////////////////////////////////////////////////////
//menuitem
////////////////////////////////////////////////////////////////////////////////////
MMenuItem* MMenuItem_Create(const char *name);
void MMenuItem_AttachMenuClickDispatcher(MMenuItem *mni, base_Message msg);

////////////////////////////////////////////////////////////////////////////////////
//menubar
////////////////////////////////////////////////////////////////////////////////////
BMenuBar* BMenuBar_Create(MForm* frm, const char *name);
void BMenuBar_AddItem(BMenuBar *mnu, BMenu *itm);

////////////////////////////////////////////////////////////////////////////////////
//BMenu
////////////////////////////////////////////////////////////////////////////////////
BMenu* BMenu_Create(const char *name);
void BMenu_AddItem(BMenu *mnu, MMenuItem *itm);

////////////////////////////////////////////////////////////////////////////////////
//checkbox
////////////////////////////////////////////////////////////////////////////////////
MCheckBox* MCheckBox_Create(float left, float top, float right, float bottom, char *caption);
MCheckBox* MCheckBox_Create_int32(int32 left, int32 top, int32 right, int32 bottom, char *caption);
MCheckBox* MForm_AddMCheckBox(MForm* frm, float left, float top, float right, float bottom, char *name);
MCheckBox* MForm_AddMCheckBox_int32(MForm* frm, int32 left, int32 top, int32 right, int32 bottom, char *name);
char* MCheckBox_getCaption(MCheckBox *cbx);
void  MCheckBox_setCaption(MCheckBox *cbx, char* caption);
void  MCheckBox_AttachMouseMovedDispatcher(MCheckBox *cbx, mouseMoved_Message msg);
void  MCheckBox_AttachMouseDownDispatcher(MCheckBox *cbx, mouseAction_Message msg);
void  MCheckBox_AttachMouseUpDispatcher(MCheckBox *cbx, mouseAction_Message msg);
void  MCheckBox_AttachKeyDownDispatcher(MCheckBox *cbx, keyAction_Message msg);
void  MCheckBox_AttachKeyUpDispatcher(MCheckBox *cbx, keyAction_Message msg);
void  MCheckBox_AttachDrawDispatcher(MCheckBox *cbx, drawAction_Message msg);
bool  MCheckBox_Checked(MCheckBox *cbx);

////////////////////////////////////////////////////////////////////////////////////
//radiobutton
////////////////////////////////////////////////////////////////////////////////////
MRadioButton* MRadioButton_Create(float left, float top, float right, float bottom, char *caption);
MRadioButton* MRadioButton_Create_int32(int32 left, int32 top, int32 right, int32 bottom, char *caption);
MRadioButton* MForm_AddMRadioButton(MForm* frm, float left, float top, float right, float bottom, char *name);
MRadioButton* MForm_AddMRadioButton_int32(MForm* frm, int32 left, int32 top, int32 right, int32 bottom, char *name);
char* MRadioButton_getCaption(MRadioButton *cbx);
void  MRadioButton_setCaption(MRadioButton *cbx, char* caption);
void  MRadioButton_AttachMouseMovedDispatcher(MRadioButton *cbx, mouseMoved_Message msg);
void  MRadioButton_AttachMouseDownDispatcher(MRadioButton *cbx, mouseAction_Message msg);
void  MRadioButton_AttachMouseUpDispatcher(MRadioButton *cbx, mouseAction_Message msg);
void  MRadioButton_AttachKeyDownDispatcher(MRadioButton *cbx, keyAction_Message msg);
void  MRadioButton_AttachKeyUpDispatcher(MRadioButton *cbx, keyAction_Message msg);
void  MRadioButton_AttachDrawDispatcher(MRadioButton *cbx, drawAction_Message msg);
bool  MRadioButton_Checked(MRadioButton *cbx);

////////////////////////////////////////////////////////////////////////////////////
//api
////////////////////////////////////////////////////////////////////////////////////

status_t beapi_beep();
status_t beapi_system_beep(const char * event_name);
status_t beapi_add_system_beep_event(const char * event_name, uint32 flags);
int beapi_testfunc();

//needed??
uint32 GetBaseMessage(void);
uint32 GetNextMessage(void);
void GenericAlert(const char *message);

#ifdef __cplusplus
} 
#endif

#endif
