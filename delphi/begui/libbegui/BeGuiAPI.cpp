/*
  $Header: /cvsroot/befpc/begui/begui/libbegui/BeGuiAPI.cpp,v 1.5 2002/04/23 18:37:29 memson Exp $
  
  $Revision: 1.5 $
  
  $Log: BeGuiAPI.cpp,v $
  Revision 1.5  2002/04/23 18:37:29  memson
  *** empty log message ***

  Revision 1.4  2002/04/13 00:32:16  memson

  Added MMemo_Text and MMemo_TextLength. Updated the testfile demo to save
  text from a MMemo when button clicked.

  Revision 1.3  2002/04/12 23:32:56  memson

  Added quite a bit.

  Got basic file handling soeted out. Also got the FilePanel's working (see
  example project)

  Popup menu now only responds to a right click (at last!!)

  Revision 1.2  2002/04/02 20:42:15  memson

  updated for Eric

  Revision 1.1.1.1  2002/03/31 10:36:07  memson

  initial import into sourceforge

  Revision 1.20  2002/03/28 08:41:02  memson

  Commited to main tree by the above on behalf of :

  2002/03/26 21:47:13 Jourde
  Adding MApplication_AddForm_int32 to create form with correct position (for
  Pascal)

  Revision 1.19  2002/03/26 13:28:51  memson

  added in combobox support - flawed at the moment.

  Revision 1.18  2002/03/14 22:31:41  memson

  Got the Menubar/menu/menuitem and popupmenu/menuitem both working... Had a
  bit of a slack night, so didn't really do much else.

  Revision 1.17  2002/03/14 00:12:38  memson

  Revised the event and property class hierarchies... hopefully makes more
  sense.

  Started to add in the Menu functionality. PopUpMenu is done and 'tested',
  but the BMenuBar/BMenu (mainmenu) stuff is there but untested.

  Added in an OnCreate and OnDestroy event to the events, plust a wierd
  offshoot class to handle the menuitem click.

  Revision 1.16  2002/03/12 23:12:37  memson

  hashed out some details - stopped child windows from screwing up app, and
  also added the close action to form and terminating functionality to the app.

  Revision 1.15  2002/03/12 18:39:16  memson

  Revision 1.14  2002/03/11 23:54:06  memson

  the form works more happily, also sorted out form canvas issues.

  lost hours of work pissing about with pascal - needs quite a bit of revision
  to get it happily working with fpc.

  Revision 1.13  2002/03/11 23:22:11  memson

  Revision 1.12  2002/03/03 11:31:38  memson

  began adding a class to hold additional properties (Owner and Tag for now).

  Revision 1.11  2002/02/19 17:49:53  memson

  Added half arsed support for multiple windows - not working quite right yet,
  but at least they seem to 'work' in so much as they don't wipe the app out.
  The secondary forms Canvas is screwed up a little.. that is a priority I
  guess.


  Revision 1.10  2002/02/18 19:28:34  memson
  No *major* changes, cleaned things up a lot. Created MEventPlugin and now
  use multiple inheritence to implement most of the event pointer storage.
  This makes things a lot neater and also ensures that I don't have to write
  the same code loads of time (mostly).

  Revision 1.9  2002/02/14 23:39:53  memson
  I've now added most of the events to the MButton, MCanvas, MMemo and MEdit..
  started a MPanel component. Again, the MCanvas is *not* tread safe!!!
  This means the calling thread must create it.

  NB. Before I add much more functionality, I'm going to look at other GUI
  libs to see if there are any controls worth borrowing.

  Next control will probably be a MLabel followed by a MMenuBar and MMenuItem.
  MForm will own a MMenuBar.

  Revision 1.8  2002/02/14 20:33:19  memson
  Got the MButton OnClick, OnMouseDown/Up, OnKeyDown/Up and OnMouseMoved
  events working. Started to add the required code to the rest of the classes
  (should be a cut/paste job).

  Revision 1.7  2002/02/14 14:00:16  memson
  fiddled a bit.. nothing major.

  Revision 1.6  2002/02/13 23:26:25  memson
  Got the MMemo working.
  Got the MEdit working, including OnClick event signal
  Got the MButton working with a signal
  Added a few get/set accessor routined for captions/text.

  Revision 1.5  2002/02/13 00:02:02  memson
  Got the MTextControl basic functionality - annoying fact is that a text
  control has to be added inside the thread that will display it!!!!

  Revision 1.4  2002/02/12 19:40:59  memson
  Removed the Sender param for the MButton::DoClick()

  Revision 1.3  2002/02/11 23:26:44  memson
  Revision 1.2  2002/01/17 20:32:54  memson

*/

#define BEGUI_EXPORTS 1

#define MENU_BAR_HEIGHT 36.0

#include "MList.h"
#include "BeGuiAPI.h"
#include <stdio.h>

//clik - control click
#define ClickMessage  'clik'
#define ChangeMessage 'chng'

#define TEXT_INSET 3.0

////////////////////////////////////////
//
//  API routines...
//
////////////////////////////////////////

MApplication *M_App = 0;

MApplication* MApplication_Create(void){
  M_App = new MApplication();
  return M_App;
}

void MApplication_Free(MApplication *app){
  app->Terminate();
}

void MApplication_Run(MApplication *app){
  app->Run();
}

MForm* MApplication_GetMainForm(MApplication *app){
  return app->GetMainForm();
}

MForm* MApplication_AddForm(MApplication *app, float left, float top, float right, float bottom, char *name, char *caption){
  BRect ClientArea(left, top, right, bottom);
  return app->AddForm(ClientArea, BString(name), false);
}

MForm* MApplication_AddForm_int32(MApplication *app, int32 left, int32 top, int32 right, int32 bottom, char *name, char *caption){
  BRect ClientArea(left, top, right, bottom);
  return app->AddForm(ClientArea, BString(name), false);
}

///

void MForm_AddChild(MForm *form, BControl *ctrl){
  form->Canvas()->AddChild(ctrl);
  ctrl->SetTarget(form);
}

MEdit* MForm_AddMEdit(MForm *form, float left, float top, float right, float bottom, char *caption){
  BRect btnRect(left, top, right, bottom);
  return form->Canvas()->AddEdit(btnRect, caption);
}

MEdit* MForm_AddMEdit_int32(MForm *form, int32 left, int32 top, int32 right, int32 bottom, char *caption){
  BRect btnRect(left, top, right, bottom);
  return form->Canvas()->AddEdit(btnRect, caption);
}

MMemo* MForm_AddMMemo(MForm *form, float left, float top, float right, float bottom, char *caption){
  BRect btnRect(left, top, right, bottom);
  return form->Canvas()->AddMemo(btnRect, caption);
}

MMemo* MForm_AddMMemo(MForm *form, int32 left, int32 top, int32 right, int32 bottom, char *caption){
  BRect btnRect(left, top, right, bottom);
  return form->Canvas()->AddMemo(btnRect, caption);
}

MMemo* MForm_AddMMemo_int32(MForm *form, int32 left, int32 top, int32 right, int32 bottom, char *caption){
  BRect btnRect(left, top, right, bottom);
  return form->Canvas()->AddMemo(btnRect, caption);
}

int32 MForm_getWidth(MForm *frm){
  return 0;
}

int32 MForm_getHeight(MForm *frm){
  return 0;
}

void MForm_setWidth(MForm *frm, int32 value){
  if (frm->LockLooper()) {
    frm->setWidth(value);
    frm->UnlockLooper();
  }
}

void MForm_setHeight(MForm *frm, int32 value){
   if (frm->LockLooper()) {
     frm->setHeight(value);
     frm->UnlockLooper();
   }
}

///

BButton* BButton_Create(float left, float top, float right, float bottom, char *caption, uint32 msg){
  BRect btnRect(left, top, right, bottom);
  
  return new BButton(btnRect, "testbutton", caption, new BMessage(msg));
}

MButton* MButton_Create(float left, float top, float right, float bottom, char *caption){
  BRect btnRect(left, top, right, bottom);  

  return new MButton( btnRect, caption );
}

MButton* MButton_Create_int32(int32 left, int32 top, int32 right, int32 bottom, char *caption){
  BRect btnRect(left, top, right, bottom);  

  return new MButton( btnRect, caption );
}

void MButton_AttachClickDispatcher(MButton *btn, base_Message msg){
  btn->AttachClickDispatcher(msg);  
}

char* MButton_getCaption(MButton *btn){
  return const_cast<char*>(btn->Label());
}

void MButton_setCaption(MButton *btn, char* caption){
  btn->SetLabel(caption);
}

void MButton_AttachMouseMovedDispatcher(MButton *btn, mouseMoved_Message msg){
  btn->AttachMouseMovedDispatcher(msg);
}

void MButton_AttachMouseDownDispatcher(MButton *btn, mouseAction_Message msg){
  btn->AttachMouseDownDispatcher(msg);
}

void MButton_AttachMouseUpDispatcher(MButton *btn, mouseAction_Message msg){
  btn->AttachMouseUpDispatcher(msg);
}

void MButton_AttachDrawDispatcher(MButton *btn, drawAction_Message msg){
  btn->AttachDrawDispatcher(msg);
}

void  MButton_AttachKeyDownDispatcher(MButton *btn, keyAction_Message msg){
  btn->AttachKeyDownDispatcher(msg);
}

void  MButton_AttachKeyUpDispatcher(MButton *btn, keyAction_Message msg){
  btn->AttachKeyUpDispatcher(msg);
}

///

MCheckBox* MCheckBox_Create(float left, float top, float right, float bottom, char *caption){
  BRect btnRect(left, top, right, bottom);  

  return new MCheckBox( btnRect, caption );
}

MCheckBox* MCheckBox_Create_int32(int32 left, int32 top, int32 right, int32 bottom, char *caption){
  BRect btnRect(left, top, right, bottom);  

  return new MCheckBox( btnRect, caption );
}

char* MCheckBox_getCaption(MCheckBox *cbx){
  return const_cast<char*>(cbx->Label());
}

void MCheckBox_setCaption(MCheckBox *cbx, char* caption){
  cbx->SetLabel(caption);
}

void MCheckBox_AttachMouseMovedDispatcher(MCheckBox *cbx, mouseMoved_Message msg){
  cbx->AttachMouseMovedDispatcher(msg);
}

void MCheckBox_AttachMouseDownDispatcher(MCheckBox *cbx, mouseAction_Message msg){
  cbx->AttachMouseDownDispatcher(msg);
}

void MCheckBox_AttachMouseUpDispatcher(MCheckBox *cbx, mouseAction_Message msg){
  cbx->AttachMouseUpDispatcher(msg);
}

void MCheckBox_AttachKeyDownDispatcher(MCheckBox *cbx, keyAction_Message msg){
  cbx->AttachKeyDownDispatcher(msg);
}

void MCheckBox_AttachKeyUpDispatcher(MCheckBox *cbx, keyAction_Message msg){
  cbx->AttachKeyUpDispatcher(msg);
}

void MCheckBox_AttachDrawDispatcher(MCheckBox *cbx, drawAction_Message msg){
  cbx->AttachDrawDispatcher(msg);
}

bool MCheckBox_Checked(MCheckBox *cbx){
  return cbx->Checked();
}

///

MRadioButton* MRadioButton_Create(float left, float top, float right, float bottom, char *caption){
  BRect btnRect(left, top, right, bottom);  

  return new MRadioButton( btnRect, caption );
}

MRadioButton* MRadioButton_Create_int32(int32 left, int32 top, int32 right, int32 bottom, char *caption){
  BRect btnRect(left, top, right, bottom);  

  return new MRadioButton( btnRect, caption );
}

char* MRadioButton_getCaption(MRadioButton *rbtn){
  return const_cast<char*>(rbtn->Label());
}

void MRadioButton_setCaption(MRadioButton *rbtn, char* caption){
  rbtn->SetLabel(caption);
}

void MRadioButton_AttachMouseMovedDispatcher(MRadioButton *rbtn, mouseMoved_Message msg){
  rbtn->AttachMouseMovedDispatcher(msg);
}

void MRadioButton_AttachMouseDownDispatcher(MRadioButton *rbtn, mouseAction_Message msg){
  rbtn->AttachMouseDownDispatcher(msg);
}

void MRadioButton_AttachMouseUpDispatcher(MRadioButton *rbtn, mouseAction_Message msg){
  rbtn->AttachMouseUpDispatcher(msg);
}

void MRadioButton_AttachKeyDownDispatcher(MRadioButton *rbtn, keyAction_Message msg){
  rbtn->AttachKeyDownDispatcher(msg);
}

void MRadioButton_AttachKeyUpDispatcher(MRadioButton *rbtn, keyAction_Message msg){
  rbtn->AttachKeyUpDispatcher(msg);
}

void MRadioButton_AttachDrawDispatcher(MRadioButton *rbtn, drawAction_Message msg){
  rbtn->AttachDrawDispatcher(msg);
}

bool MRadioButton_Checked(MRadioButton *rbtn){
  return rbtn->Checked();
}

///

void MForm_AttachMouseMovedDispatcher(MForm *form, mouseMoved_Message msg){
  form->Canvas()->AttachMouseMovedDispatcher(msg);
}

void MForm_AttachMouseDownDispatcher(MForm *form, mouseAction_Message msg){
  form->Canvas()->AttachMouseDownDispatcher(msg);
}

void MForm_AttachMouseUpDispatcher(MForm *form, mouseAction_Message msg){
  form->Canvas()->AttachMouseUpDispatcher(msg);
}

void MForm_AttachDrawDispatcher(MForm *form, drawAction_Message msg){
  form->Canvas()->AttachDrawDispatcher(msg);
}

void MForm_AttachKeyDownDispatcher(MForm *form, keyAction_Message msg){
  form->Canvas()->AttachKeyDownDispatcher(msg);
}

void MForm_AttachKeyUpDispatcher(MForm *form, keyAction_Message msg){
  form->Canvas()->AttachKeyUpDispatcher(msg);
}

void MForm_Show(MForm *frm){
  printf("about to lock\n");
  if (frm->LockLooper()) {
    frm->Show();
    printf("after show\n");
    frm->UnlockLooper();
    printf("unlocked\n");
  }
  else printf("lock failed\n");
}

void MForm_Hide(MForm *frm){
  frm->Hide();
}

///

void MEdit_AttachClickDispatcher(MEdit *edt, base_Message msg){
  edt->AttachClickDispatcher(msg);  
}

void MEdit_AttachMouseMovedDispatcher(MEdit *edt, mouseMoved_Message msg){
  edt->AttachMouseMovedDispatcher(msg);
}

void MEdit_AttachMouseDownDispatcher(MEdit *edt, mouseAction_Message msg){
  edt->AttachMouseDownDispatcher(msg);
}

void MEdit_AttachMouseUpDispatcher(MEdit *edt, mouseAction_Message msg){
  edt->AttachMouseUpDispatcher(msg);
}

void MEdit_AttachDrawDispatcher(MEdit *edt, drawAction_Message msg){
  edt->AttachDrawDispatcher(msg);
}

void MEdit_AttachKeyDownDispatcher(MEdit *edt, keyAction_Message msg){
  edt->AttachKeyDownDispatcher(msg);
}

void MEdit_AttachKeyUpDispatcher(MEdit *edt, keyAction_Message msg){
  edt->AttachKeyUpDispatcher(msg);
}

char* MEdit_getText(MEdit* edt){
  return const_cast<char*>(edt->Text());
}

void  MEdit_setText(MEdit* edt, char* text){
  if (edt->LockLooper()){
    edt->SetText(text);
    edt->UnlockLooper();
  }
}

///

void MMemo_AttachMouseMovedDispatcher(MMemo *memo, mouseMoved_Message msg){
  memo->AttachMouseMovedDispatcher(msg);
}

void MMemo_AttachMouseDownDispatcher(MMemo *memo, mouseAction_Message msg){
  memo->AttachMouseDownDispatcher(msg);
}

void MMemo_AttachMouseUpDispatcher(MMemo *memo, mouseAction_Message msg){
  memo->AttachMouseUpDispatcher(msg);
}

void MMemo_AttachDrawDispatcher(MMemo *memo, drawAction_Message msg){
  memo->AttachDrawDispatcher(msg);
}

void MMemo_AttachKeyDownDispatcher(MMemo *memo, keyAction_Message msg){
  memo->AttachKeyDownDispatcher(msg);
}

void MMemo_AttachKeyUpDispatcher(MMemo *memo, keyAction_Message msg){
  memo->AttachKeyDownDispatcher(msg);
}

const char* MMemo_Text(MMemo *memo){
  return memo->getTextView()->Text();
}

int32 MMemo_TextLength(MMemo *memo){
  return memo->getTextView()->TextLength();
}

void MMemo_SetText(MMemo *memo, char *text){
  if (memo->LockLooper()){
    memo->getTextView()->SetText(text);
    memo->UnlockLooper();
  }
}

///

MCheckBox* MForm_AddMCheckBox(MForm* frm, float left, float top, float right, float bottom, char *name){
  BRect cbxRect(left, top, right, bottom);  

  return frm->Canvas()->AddCheckBox( cbxRect, name );
}

MCheckBox* MForm_AddMCheckBox_int32(MForm* frm, int32 left, int32 top, int32 right, int32 bottom, char *name){
  BRect cbxRect(left, top, right, bottom);  

  return frm->Canvas()->AddCheckBox( cbxRect, name );
}

///

MPanel* MForm_AddMPanel(MForm* frm, float left, float top, float right, float bottom, char *name){
  BRect pnlRect(left, top, right, bottom);  

  return frm->Canvas()->AddPanel( pnlRect, name );
}

MPanel* MForm_AddMPanel_int32(MForm* frm, int32 left, int32 top, int32 right, int32 bottom, char *name){
  BRect pnlRect(left, top, right, bottom);  

  return frm->Canvas()->AddPanel( pnlRect, name );
}

void MPanel_AddChild(MPanel *panel, BControl* ctrl){
  panel->AddChild(ctrl);
}

///

void MForm_AddPopUpMenu(MForm *frm, MPopUpMenu *mni){
  if (frm->LockLooper()){
    frm->Canvas()->setPopUpMenu(mni);  
    frm->UnlockLooper();
  }
}

MPopUpMenu* MPopUpMenu_Create(const char *name){
  return new MPopUpMenu(name);
}

void MPopUpMenu_AddItem(MPopUpMenu *mnu, MMenuItem *itm){
  mnu->AddItem(dynamic_cast<BMenuItem*>(itm));
}

MMenuItem* MMenuItem_Create(const char *name)
{
  return new MMenuItem(name);
}

void MMenuItem_AttachMenuClickDispatcher(MMenuItem *mni, base_Message msg)
{
  mni->AttachMenuClickDispatcher(msg);
}

///

BMenuBar* BMenuBar_Create(MForm* frm, const char *name)
{
  //BRect frame;
  //frame.Set(0, MENU_BAR_HEIGHT, frm->Bounds().right, frm->Bounds().bottom + MENU_BAR_HEIGHT);
  BRect menuBarRect;
  menuBarRect.Set(0.0, 0.0, 32000.0, MENU_BAR_HEIGHT);
  BMenuBar *mb = new BMenuBar(menuBarRect, "mainmenubar");
  if (frm->LockLooper()){
    frm->ResizeBy(0.0, MENU_BAR_HEIGHT);
    frm->Canvas()->AddChild(mb);
    frm->UnlockLooper();
  }
  return mb;
}

void BMenuBar_AddItem(BMenuBar *mnu, BMenu *itm)
{
  mnu->AddItem(itm);
}

BMenu* BMenu_Create(const char *name)
{
  return new BMenu(name);
}

void BMenu_AddItem(BMenu *mnu, MMenuItem *itm)
{
  mnu->AddItem(itm);
}

/////////////////////////////////////////

//utility

uint32 _BASE_MESSAGE = 0;

uint32 GetNextMessage(void){
  if (_BASE_MESSAGE == 0){
    _BASE_MESSAGE = GetBaseMessage();
  }  
  return _BASE_MESSAGE++;
}

uint32 GetBaseMessage(void){ 
  return 'bgui'; 
}

void GenericAlert(const char *message){
  BAlert *alert;
  
  alert = new BAlert("Info", message, "OK");
  alert->Go();
}

#include <Beep.h>

status_t beapi_beep(){
  return beep();
}

status_t beapi_system_beep(const char * event_name){
  return system_beep(event_name);
}

status_t beapi_add_system_beep_event(const char * event_name, uint32 flags){
  return add_system_beep_event(event_name, flags);
}

/////////////////////////////////////////

//Test the API
int test(void)
{
  return 33;
}

//Private Local var for test
CTest* FTest;

//Init the test class
CTest* CTest_Create(void){
  FTest = new CTest();
  return FTest;
}

//GetValue
int CTest_GetValue(CTest* ptr){
  int ReturnValue;   
  ReturnValue = ptr->GetValue();
  return ReturnValue;
}

//SetValue
void CTest_SetValue(CTest* ptr, int val){
  ptr->SetValue(val);
}

//Destroy the variable
void CTest_Free(CTest* ptr){
  delete &ptr;
}


