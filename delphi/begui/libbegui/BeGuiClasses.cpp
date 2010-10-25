/*
  $Header: /cvsroot/befpc/begui/begui/libbegui/BeGuiClasses.cpp,v 1.4 2002/04/27 08:29:51 memson Exp $
  
  $Revision: 1.4 $
  
  $Log: BeGuiClasses.cpp,v $
  Revision 1.4  2002/04/27 08:29:51  memson

  updated

  Revision 1.3  2002/04/23 18:37:29  memson
  *** empty log message ***

  Revision 1.2  2002/04/12 23:32:56  memson

  Added quite a bit.

  Got basic file handling soeted out. Also got the FilePanel's working (see
  example project)

  Popup menu now only responds to a right click (at last!!)

  Revision 1.1.1.1  2002/03/31 10:36:16  memson

  initial import into sourceforge

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

  Revision 1.6  2002/03/12 23:12:37  memson

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
  Removed the Sebder param for the MButton::DoClick()

  Revision 1.3  2002/02/11 23:26:44  memson
  Revision 1.2  2002/01/17 20:32:54  memson

*/

#define BEGUI_EXPORTS 1
#include "MList.h"
#include "MFile.h"
#include "BeGuiClasses.h"
#include <stdio.h>
#include <storage/Path.h>
#include "debug.h"


//clik - control click
#define ClickMessage  'clik'
#define ChangeMessage 'chng'

#define TEXT_INSET 3.0

////////////////////////////////////////
//
//  Classes...
//
////////////////////////////////////////

////////////////////////////////////////
// List classes...

//List item for storing GUI classes...

MGUIListItem::MGUIListItem(BString name, void *item):
  MListItem(item)
{
  fName = name;
}

BString MGUIListItem::getName(void){
  return fName;
}

void MGUIListItem::setName(BString name){
  fName = name;
}

// GUI Item store....

MGUIListItem* MGUIList::GetItem(int32 index){
  return (MGUIListItem*) GetItem(index);
}

//////////////////////////////////////////

MPropertyPlugin::MPropertyPlugin(BArchivable *AOwner)
{
  FOwner = AOwner;
}

MPropertyPlugin::MPropertyPlugin()
{
  FOwner = 0;
}

MPropertyPlugin::~MPropertyPlugin()
{
  FOwner = 0;
}

void MPropertyPlugin::setTag(uint32 ATag)
{
  FTag = ATag;
}

uint32 MPropertyPlugin::getTag(void)
{
  return FTag;
}

BArchivable* MPropertyPlugin::getOwner(void)
{
  return FOwner;
}

void MPropertyPlugin::setOwner(BArchivable *AOwner)
{
  if (AOwner != FOwner){
    //change over
  }
  
  FOwner = AOwner;
}

//////////////////////////////////////////
// Property Plugin Class...

MMenuPropertyPlugin::MMenuPropertyPlugin(BArchivable *AOwner):
  MPropertyPlugin(AOwner)
{
  fPopUpMenu = 0;  
}

MMenuPropertyPlugin::~MMenuPropertyPlugin()
{
  //this control will be owned by a BView..BView
  fPopUpMenu = 0; 
}

void MMenuPropertyPlugin::setPopUpMenu(MPopUpMenu *menu)
{
  fPopUpMenu = menu;  
}

MPopUpMenu* MMenuPropertyPlugin::getPopUpMenu(void)
{
  return fPopUpMenu;
}

//////////////////////////////////////////
// EventPlugin Class... 

MEventPlugin::MEventPlugin()
{
  fOnCreate = 0;
  fOnDestroy = 0;
}

MEventPlugin::~MEventPlugin()
{
  fOnCreate = 0;
  fOnDestroy = 0;
}

void MEventPlugin::AttachCreateDispatcher(genericEvent_Message value)
{
  fOnCreate = value;
}

void MEventPlugin::AttachDestroyDispatcher(genericEvent_Message value)
{
  fOnDestroy = value;
}

void MEventPlugin::DoCreate(BControl *self)
{
  if (fOnCreate != NULL){
    fOnCreate( self );
  }
}

void MEventPlugin::DoDestroy(BControl *self)
{
  if (fOnDestroy != NULL){
    fOnDestroy( self );
  }
}

//////////////////////////////////////////
//

MMenuEventPlugin::MMenuEventPlugin()
{
  fOnMenuClick = 0;
}

MMenuEventPlugin::~MMenuEventPlugin()
{
  fOnMenuClick = 0;
}

void MMenuEventPlugin::DoMenuClick(MMenuItem *self)
{
  if (fOnMenuClick != NULL){
    fOnMenuClick( self, ClickMessage );
  }  
}

void MMenuEventPlugin::AttachMenuClickDispatcher(base_Message value)
{
  fOnMenuClick = value;
}

//////////////////////////////////////////
//BasicEventPlugin Class...

MBasicEventPlugin::MBasicEventPlugin():
  MEventPlugin()  
{
  fOnClick = 0;
  fOnChange = 0;
}

MBasicEventPlugin::~MBasicEventPlugin()
{
  fOnClick = 0;
  fOnChange = 0;
}

void MBasicEventPlugin::AttachClickDispatcher(base_Message value)
{
  fOnClick = value;
}

void MBasicEventPlugin::AttachChangeDispatcher(base_Message value)
{
  fOnChange = value;
}

void MBasicEventPlugin::DoChange(BControl *self)
{
  if (fOnChange != NULL){
    fOnChange( self, ChangeMessage );
  }
}

void MBasicEventPlugin::DoClick(BControl *self)
{
  if (fOnClick != NULL){
    fOnClick( dynamic_cast<BControl*>(this), ClickMessage );
  }
}

//////////////////////////////////////////
//ControlEventPlugin Class...

MControlEventPlugin::MControlEventPlugin():
  MBasicEventPlugin()
{
  fOnMouseMove = 0;
  fOnMouseUp = 0;
  fOnMouseDown = 0;
  fOnKeyUp = 0;
  fOnKeyDown = 0;
  fOnDraw = 0;
  fOnKeyPress = 0;
}

MControlEventPlugin::~MControlEventPlugin()
{
  fOnMouseMove = 0;
  fOnMouseUp = 0;
  fOnMouseDown = 0;
  fOnKeyUp = 0;
  fOnKeyDown = 0;
  fOnDraw = 0;
  fOnKeyPress = 0;  
}

void MControlEventPlugin::AttachDrawDispatcher(drawAction_Message value)
{
  fOnDraw = value;
}

void MControlEventPlugin::AttachMouseDownDispatcher(mouseAction_Message value)
{
  fOnMouseDown = value;
}

void MControlEventPlugin::AttachMouseUpDispatcher(mouseAction_Message value)
{
  fOnMouseUp = value;
}

void MControlEventPlugin::AttachMouseMovedDispatcher(mouseMoved_Message value)
{
  fOnMouseMove = value;
}

void MControlEventPlugin::AttachKeyDownDispatcher(keyAction_Message value)
{
  fOnKeyDown = value;
}

void MControlEventPlugin::AttachKeyUpDispatcher(keyAction_Message value)
{
  fOnKeyUp = value;
}

void MControlEventPlugin::AttachKeyPressDispatcher(keyAction_Message value)
{
  fOnKeyPress = value;
}

void MControlEventPlugin::DoKeyPress(BControl *self, const char *bytes, int32 numBytes)
{
  if (fOnKeyPress != NULL){
    fOnKeyPress( self, bytes, numBytes );
  }
}

//////////////////////////////////////////
// Form Event Property

MFormEventPlugin::MFormEventPlugin():
  MEventPlugin()
{
  fOnClose = 0;
}

MFormEventPlugin::~MFormEventPlugin()
{
  fOnClose = 0;
}

void MFormEventPlugin::AttachCloseDispatcher(closeAction_Message value)
{
  fOnClose = value;
}

void MFormEventPlugin::DoClose(BView *self, MCloseAction &closeAction)
{
  if (fOnClose != NULL){
    fOnClose( self, closeAction );
  }
}

//////////////////////////////////////////
//Panel Class...

MPanel::MPanel(BRect ClientArea, char *name):
  MCanvas(ClientArea, name)
{
  SetViewColor(200, 200, 200);
}

void MPanel::Draw(BRect updateArea)
{
  MCanvas::Draw(updateArea);
}


//////////////////////////////////////////
//Memo Class...

MMemo::MMemo(char *name):
  BScrollView(name, 
              fTextArea = new BTextView(BRect(0, 0, 50, 50),
                            "TextArea",
                            BRect(0, 0, 
                                  (50 - TEXT_INSET), 
                                  (50 - TEXT_INSET)),
                            B_FOLLOW_ALL, 
                            B_WILL_DRAW), 
              B_FOLLOW_ALL,
              true, true, 
              B_FANCY_BORDER),
  MControlEventPlugin(),
  MPropertyPlugin(NULL)
{
  fTextArea->SetStylable(true);
}

MMemo::MMemo(BRect frame, char *name):
  BScrollView(name, 
              fTextArea = new BTextView(
                            BRect(frame.left, 
                                  frame.top, 
                                  (frame.right  - B_V_SCROLL_BAR_WIDTH), 
                                  (frame.bottom - B_H_SCROLL_BAR_HEIGHT)),
                            "TextArea",
                            BRect(0, 0,  
                                  (frame.right  - frame.left - TEXT_INSET - B_V_SCROLL_BAR_WIDTH), 
                                  (frame.bottom - frame.top  - TEXT_INSET - B_H_SCROLL_BAR_HEIGHT)),
                            B_FOLLOW_ALL, 
                            B_WILL_DRAW), 
              B_FOLLOW_ALL,
              true, true, 
              B_FANCY_BORDER),
  MControlEventPlugin(),
  MPropertyPlugin(NULL)
{
  fTextArea->SetStylable(true);
}

BTextView* MMemo::getTextView(void)
{
  return fTextArea;
}

void MMemo::AttachedToWindow()
{
  fTextArea->SetFont(be_bold_font);
  fTextArea->SetFontSize(12);
  
  fTextArea->MakeFocus();
}

void MMemo::KeyDown(const char *bytes, int32 numBytes)
{
  BScrollView::KeyDown(bytes, numBytes);
  
  if (fOnKeyDown != NULL){
    fOnKeyDown( this, bytes, numBytes );
  }
} 

void MMemo::KeyUp(const char *bytes, int32 numBytes)  
{
  BScrollView::KeyUp(bytes, numBytes);
  
  if (fOnKeyUp != NULL){
    fOnKeyUp( this, bytes, numBytes );
  }
  DoChange(dynamic_cast<BControl*>(this)); //simulate a 'change' event...
}

void MMemo::MouseDown(BPoint pt)
{
  BScrollView::MouseDown(pt);
  
  if (fOnMouseDown != NULL){
    fOnMouseDown( this, pt.x, pt.y );
  }
}

void MMemo::MouseUp(BPoint pt)
{
  BScrollView::MouseUp(pt);
  
  if (fOnMouseUp != NULL){
    fOnMouseUp( this, pt.x, pt.y );
  }
  DoClick(dynamic_cast<BControl*>(this)); //simulate a click event
}

void MMemo::MouseMoved(BPoint pt, uint32 code, const BMessage *msg)
{
  BScrollView::MouseMoved(pt, code, msg);
  
  if (fOnMouseMove != NULL){
    fOnMouseMove( this, pt.x, pt.y, code, 0 );
  }
}

void MMemo::Draw(BRect updateArea)
{
  BScrollView::Draw(updateArea);
  
  if (fOnDraw != NULL){
    fOnDraw( this, updateArea.left, updateArea.top, updateArea.right, updateArea.bottom );
  }
 
}

//////////////////////////////////////////
// MComboBox

MCheckBox::MCheckBox(BRect frame, char *name):
  BCheckBox(frame, name, name, new BMessage(ClickMessage)),
  MControlEventPlugin(),
  MPropertyPlugin(NULL)
{
  DoCreate(dynamic_cast<BControl*>(this));
}

MCheckBox::~MCheckBox()
{
  DoDestroy(dynamic_cast<BControl*>(this));
}

void MCheckBox::KeyDown(const char *bytes, int32 numBytes)
{
  BCheckBox::KeyDown(bytes, numBytes);
  
  if (fOnKeyDown != NULL){
    fOnKeyDown( this, bytes, numBytes );
  }
} 

void MCheckBox::KeyUp(const char *bytes, int32 numBytes) 
{
  BCheckBox::KeyUp(bytes, numBytes);
  
  if (fOnKeyUp != NULL){
    fOnKeyUp( this, bytes, numBytes );
  }
  DoChange(dynamic_cast<BControl*>(this)); //simulate a 'change' event...
}

void MCheckBox::MouseDown(BPoint pt)
{
  BCheckBox::MouseDown(pt);
  
  if (fOnMouseDown != NULL){
    fOnMouseDown( this, pt.x, pt.y );
  }
}

void MCheckBox::MouseUp(BPoint pt)
{
  BCheckBox::MouseUp(pt);
  
  if (fOnMouseUp != NULL){
    fOnMouseUp( this, pt.x, pt.y );
  }
  DoClick(dynamic_cast<BControl*>(this)); //simulate a click event
}

void MCheckBox::MouseMoved(BPoint pt, uint32 code, const BMessage *msg)
{
  BCheckBox::MouseMoved(pt, code, msg);
  
  if (fOnMouseMove != NULL){
    fOnMouseMove( this, pt.x, pt.y, code, 0 );
  }
}

void MCheckBox::Draw(BRect updateArea)
{
  BCheckBox::Draw(updateArea);
  
  if (fOnDraw != NULL){
    fOnDraw( this, updateArea.left, updateArea.top, updateArea.right, updateArea.bottom );
  }
 
}

bool MCheckBox::Checked(void)
{
  if (this->Value() == B_CONTROL_ON)
    return true;
  else
    return false;
}

//////////////////////////////////////////
// MRadioButton

MRadioButton::MRadioButton(BRect frame, char *name):
  BRadioButton(frame, name, name, new BMessage(ClickMessage)),
  MControlEventPlugin(),
  MPropertyPlugin(NULL)
{
  DoCreate(dynamic_cast<BControl*>(this));
}

MRadioButton::~MRadioButton()
{
  DoDestroy(dynamic_cast<BControl*>(this));
}

void MRadioButton::KeyDown(const char *bytes, int32 numBytes)
{
  BRadioButton::KeyDown(bytes, numBytes);
  
  if (fOnKeyDown != NULL){
    fOnKeyDown( this, bytes, numBytes );
  }
} 

void MRadioButton::KeyUp(const char *bytes, int32 numBytes) 
{
  BRadioButton::KeyUp(bytes, numBytes);
  
  if (fOnKeyUp != NULL){
    fOnKeyUp( this, bytes, numBytes );
  }
  DoChange(dynamic_cast<BControl*>(this)); //simulate a 'change' event...
}

void MRadioButton::MouseDown(BPoint pt)
{
  BRadioButton::MouseDown(pt);
  
  if (fOnMouseDown != NULL){
    fOnMouseDown( this, pt.x, pt.y );
  }
}

void MRadioButton::MouseUp(BPoint pt)
{
  BRadioButton::MouseUp(pt);
  
  if (fOnMouseUp != NULL){
    fOnMouseUp( this, pt.x, pt.y );
  }
  DoClick(dynamic_cast<BControl*>(this)); //simulate a click event
}

void MRadioButton::MouseMoved(BPoint pt, uint32 code, const BMessage *msg)
{
  BRadioButton::MouseMoved(pt, code, msg);
  
  if (fOnMouseMove != NULL){
    fOnMouseMove( this, pt.x, pt.y, code, 0 );
  }
}

void MRadioButton::Draw(BRect updateArea)
{
  BRadioButton::Draw(updateArea);
  
  if (fOnDraw != NULL){
    fOnDraw( this, updateArea.left, updateArea.top, updateArea.right, updateArea.bottom );
  }
 
}

bool MRadioButton::Checked(void)
{
  if (this->Value() == B_CONTROL_ON)
    return true;
  else
    return false;
}


//////////////////////////////////////////
//Edit Class...

MEdit::MEdit(char *name):
  BTextControl(BRect(0, 0, 25, 50), name, NULL, "", new BMessage(ChangeMessage)),
  MControlEventPlugin(),
  MPropertyPlugin(NULL)
{
  DoCreate( dynamic_cast<BControl*>(this) );
}

MEdit::MEdit(BRect frame, char *name):
  BTextControl(frame, name, NULL, "", new BMessage(ChangeMessage)),
  MControlEventPlugin(),
  MPropertyPlugin(NULL)
{
  DoCreate( dynamic_cast<BControl*>(this) );
}

MEdit::~MEdit()
{
  DoDestroy( dynamic_cast<BControl*>(this) );
}

void MEdit::KeyDown(const char *bytes, int32 numBytes)
{
  BTextControl::KeyDown(bytes, numBytes);
  
  if (fOnKeyDown != NULL){
    fOnKeyDown( this, bytes, numBytes );
  }
} 

void MEdit::KeyUp(const char *bytes, int32 numBytes)  
{
  BTextControl::KeyUp(bytes, numBytes);
  
  if (fOnKeyUp != NULL){
    fOnKeyUp( this, bytes, numBytes );
  }
}

void MEdit::MouseDown(BPoint pt)
{
  BTextControl::MouseDown(pt);
  
  if (fOnMouseDown != NULL){
    fOnMouseDown( this, pt.x, pt.y );
  }
}

void MEdit::MouseUp(BPoint pt)
{
  BTextControl::MouseUp(pt);
  
  if (fOnMouseUp != NULL){
    fOnMouseUp( this, pt.x, pt.y );
  }
  DoClick(this); //simulate click event
}

void MEdit::MouseMoved(BPoint pt, uint32 code, const BMessage *msg)
{
  BTextControl::MouseMoved(pt, code, msg);
  
  if (fOnMouseMove != NULL){
    fOnMouseMove( this, pt.x, pt.y, code, 0 );
  }
}

void MEdit::Draw(BRect updateArea)
{
  BTextControl::Draw(updateArea);
  
  if (fOnDraw != NULL){
    fOnDraw( this, updateArea.left, updateArea.top, updateArea.right, updateArea.bottom );
  }
 
}

//////////////////////////////////////////
//Button Class...

MButton::MButton(char *name):
  BButton(BRect(0, 0, 25, 50), name, name, new BMessage(ClickMessage)),
  MControlEventPlugin(),
  MPropertyPlugin(NULL)
{
  DoCreate(dynamic_cast<BControl*>(this));
}

MButton::MButton(BRect frame, char *name):
  BButton(frame, name, name, new BMessage(ClickMessage)),
  MControlEventPlugin(),
  MPropertyPlugin(NULL)
{
  DoCreate(dynamic_cast<BControl*>(this));
}

MButton::~MButton()
{
  DoDestroy(dynamic_cast<BControl*>(this));
}

void MButton::KeyDown(const char *bytes, int32 numBytes)
{
  BButton::KeyDown(bytes, numBytes);
  
  if (fOnKeyDown != NULL){
    fOnKeyDown( this, bytes, numBytes );
  }
} 

void MButton::KeyUp(const char *bytes, int32 numBytes)  
{
  BButton::KeyUp(bytes, numBytes);
  
  if (fOnKeyUp != NULL){
    fOnKeyUp( this, bytes, numBytes );
  }
}

void MButton::MouseDown(BPoint pt)
{
  BButton::MouseDown(pt);
  
  if (fOnMouseDown != NULL){
    fOnMouseDown( this, pt.x, pt.y );
  }
}

void MButton::MouseUp(BPoint pt)
{
  BButton::MouseUp(pt);
  
  if (fOnMouseUp != NULL){
    fOnMouseUp( this, pt.x, pt.y );
  }
}

void MButton::MouseMoved(BPoint pt, uint32 code, const BMessage *msg)
{
  BButton::MouseMoved(pt, code, msg);
  
  if (fOnMouseMove != NULL){
    fOnMouseMove( this, pt.x, pt.y, code, 0 );
  }
}

void MButton::Draw(BRect updateArea)
{
  BButton::Draw(updateArea);
  
  if (fOnDraw != NULL){
    fOnDraw( this, updateArea.left, updateArea.top, updateArea.right, updateArea.bottom );
  } 
}

//////////////////////////////////////////
//Application Class...

MApplication::MApplication(const char* signature): 
    BApplication(signature),
    MEventPlugin()
{
  BRect tmpRect;
  tmpRect.Set(50, 50, 1000, 500);
  BString formname = "main";
  
  fMainForm = new MForm(tmpRect);
  //fMainForm->Show();  
  
  fForms = new MGUIList();
  fForms->AddItem(new MGUIListItem(formname, fMainForm));
  
  fTerminating = false; //controls the act of shutting down

  DoCreate( dynamic_cast<BControl*>(this) );
}

MApplication::~MApplication()
{
  DoDestroy( dynamic_cast<BControl*>(this) );
}

thread_id MApplication::Run(void)
{
  if (fMainForm != NULL) fMainForm->Show();
  
  return BApplication::Run();
  
}

void MApplication::RefsReceived(BMessage *message)
{
  SendText("MApplication::RefsReceived");
  
  if (message->what == SAVE_PANEL_MESSAGE){
    printf("\nSave\n");
  }
  
  if (message->what == OPEN_PANEL_MESSAGE){
    printf("\nOpen\n");
  }
}

void MApplication::MessageReceived(BMessage *message)
{
  void *pointer;
  MFilePanel *filepanel;
  entry_ref ref;
  const char *savename;
  char *openname;
  BPath path;
  BEntry entry;
  BString tmp;
  status_t err = B_OK;
  uint32 type;
  int32 count;
  
  SendText("MApplication::MessageRecvd");
  
  if (message->what == SAVE_PANEL_MESSAGE){
    SendText("MApplication::MessageRecvd - save panel");
    if ( (err = message->FindRef("directory", &ref)) != B_OK ) {
      //printf("failed to find dir, error %d\n", err);
      return;
    }
    if ( (err = message->FindString("name", &savename)) != B_OK ){
      //printf("failed to find filename, error %d\n", err);
      return;    
    }
    
    if ( (err = entry.SetTo(&ref)) != B_OK ){
      //printf("failed to create entr from path, error %d\n", err);
      return;
    }
    
    entry.GetPath(&path);
    path.Append(savename);
    
    printf( "%s\n", path.Path() );
    
    if (message->FindPointer("source", &pointer) == B_OK){
      //printf("works1\n");
      if ((filepanel =  reinterpret_cast<MFilePanel*>(pointer)) != NULL){
        //printf("works2\n");
        filepanel->DoExecute( path );
      }  
    }
  }
  
  else if (message->what == OPEN_PANEL_MESSAGE){
    SendText("MApplication::MessageRecvd - open panel");
    
	message->GetInfo("refs", &type, &count);
	if (type != B_REF_TYPE) {
	  SendText("MApplication::MessageRecvd - no refs found");
	  return;
	}
	
	for (int32 i = --count; i >= 0; --i) {
   	  if ((err = message->FindRef("refs", i, &ref)) == B_OK) {
        if (message->FindPointer("source", &pointer) == B_OK){
          SendText("MApplication::MessageRecvd - works 1");
          
          if ( (err = entry.SetTo(&ref)) != B_OK ){
            SendText("MApplication::MessageRecvd - failed to create entr from path, error");
            return;
    	  }
    	  
    	  entry.GetPath(&path);
    	  
          if ((filepanel =  reinterpret_cast<MFilePanel*>(pointer)) != NULL){
            SendText("MApplication::MessageRecvd - works2");
            BString s;
            s << "MApplication::MessageRecvd - " << "before event ... " << path.Path();
            SendText( s.String() );
            if ( LockLooper() ){
              if (fMainForm->LockLooper()){
                SendText("right befire call for file event");
                filepanel->DoExecute( path );
                SendText("just after call for file event");
                fMainForm->UnlockLooper();
              }
              UnlockLooper();
            }
            
          }
        }
        BString s;
        s.SetTo("MApplication::MessageRecvd - after file open event code");
        SendText( s.String() );
   	  }
   	  else {
   	    printf("processing ref %d there was an error %d", i, err);
   	    return;
   	  }
   	}
  }
  else {
    printf("MApplication::messagereceived\n");
    BApplication::MessageReceived(message);
  }
}

void MApplication::Terminate(void)
{  
  //if main form is dead, we must not call this because...
  if (!fTerminating)
  { 
    //...fTerminating is set by fMainForm as it exits.
    fMainForm->QuitRequested();
  }
}

MForm* MApplication::AddForm(BRect ClientArea, BString name, bool showForm = false)
{
  MForm *form = new MForm(ClientArea);
  fForms->AddItem(new MGUIListItem(name, form)); 
  form->Lock();
  form->Run();
  if (showForm) form->Show();
  form->Unlock();
  return form;
}


////////////////////////////////////////
//Form Class...

MForm::MForm(BRect ClientArea): 
    BWindow(ClientArea, "Form", B_TITLED_WINDOW, B_ASYNCHRONOUS_CONTROLS),
    MFormEventPlugin()
{
  fClientArea = Bounds();
  
  fClientArea.OffsetTo(B_ORIGIN); //reset position 
  fCanvas = new MCanvas(fClientArea, "Canvas"); 
  AddChild(fCanvas);
  
  fCloseAction = caHide;
  
  DoCreate(dynamic_cast<BControl*>(this)); 
}

MForm::~MForm()
{
  DoDestroy(dynamic_cast<BControl*>(this));
}

void MForm::MessageReceived(BMessage *message){
  void *pointer;
  BControl *control;
  MButton *btn;
  MMenuItem *mni;
  MEdit *edt;
  
  if (message->FindPointer("source", &pointer) == B_OK)
  {
    if ((control =  reinterpret_cast<BControl*>(pointer)) != NULL)
    {
      if ((btn = dynamic_cast<MButton*>(control)) != NULL){
        btn->DoClick(btn);
      }  
      else if ((edt = dynamic_cast<MEdit*>(control)) != NULL){
        edt->DoChange(edt); //this is a funny event!!
      }
      else if ((mni = dynamic_cast<MMenuItem*>(control)) != NULL){
        mni->DoMenuClick(mni); 
      }
    }  
    else 
    { 
      printf("MForm::messagereceived\n");
      BWindow::MessageReceived(message);
    }
  } 
  else 
  { 
    BWindow::MessageReceived(message);
  }
}

bool MForm::QuitRequested()
{
  MForm *frm;
  
  //handle the close event
  DoClose(dynamic_cast<BControl*>(this), fCloseAction);
  
  if (dynamic_cast<MApplication*>(be_app) != NULL)
  {
    //when terminating, just exit
    if ( dynamic_cast<MApplication*>(be_app)->Terminating() )
    {
      return(true);
    }
    
    //main form
    frm = dynamic_cast<MApplication*>(be_app)->GetMainForm();
    if (frm == this)
    {
      printf("killing app");
      frm->Lock();
      be_app->PostMessage(B_QUIT_REQUESTED);
      frm->Unlock();
      dynamic_cast<MApplication*>(be_app)->setTerminating( true );
    }  
    //sub forms
    else
    {
      //add 'canclose' here
      switch (fCloseAction) {
        case caNone:
          //ignore the action
          return(false);
        case caHide:
          //hide self
          this->Lock(); //must lock window before calling hide
          this->Hide();
          this->Unlock(); //must remember to unlock
          return(false);
        case caFree:
          //do default.. just remove window
          return(true);
        default:
          return(true);  
      }
    }  
  }
  else
  {
    be_app->PostMessage(B_QUIT_REQUESTED);
  }
  return(true);
}

void MForm::UpdateBounds(void)
{
  //fClientArea = this->Bounds();
}

void  MForm::setWidth(float w)
{
  float h = fClientArea.Height();
  
  fClientArea.Set(fClientArea.left, fClientArea.top, fClientArea.left + w, fClientArea.top + h);
  
  this->ResizeTo(w, h);
  
  //UpdateBounds();
  
}

void  MForm::setHeight(float h)
{
  float w = fClientArea.Width();
  
  fClientArea.Set(fClientArea.left, fClientArea.top, fClientArea.left + w, fClientArea.top + h);
  
  this->ResizeTo(w, h);
  
  //UpdateBounds();
}

float MForm::getWidth(void)
{
  return fClientArea.Width();
}

float MForm::getHeight(void)
{
  return fClientArea.Height();
}

float MForm::getLeft(void)
{
  return fClientArea.left;
}

float MForm::getTop(void)
{
  return fClientArea.top;
}

float MForm::getBottom(void)
{
  return fClientArea.bottom;
}

float MForm::getRight(void)
{
  return fClientArea.right;
}

MCanvas* MForm::Canvas(void)
{
  return fCanvas;
}

MCloseAction MForm::getCloseAction(void)
{
  return fCloseAction;
}

void MForm::setCloseAction(MCloseAction ca)
{
  fCloseAction = ca;
}

////////////////////////////////////////
//Canvas class...

MCanvas::MCanvas(BRect ClientArea, char *name):
  BView(ClientArea, name, B_FOLLOW_ALL, 0),
  MControlEventPlugin(),
  MMenuPropertyPlugin(NULL)
{
  SetViewColor(150, 150, 150);
  DoCreate(dynamic_cast<BControl*>(this));
}

MCanvas::~MCanvas()
{
  DoDestroy(dynamic_cast<BControl*>(this));
}

void MCanvas::AttachedToWindow()
{
}

MEdit* MCanvas::AddEdit(BRect ClientArea, char *name)
{
  MEdit* tmp = new MEdit(ClientArea, name);
  AddChild(tmp);
  return tmp;  
}

MMemo* MCanvas::AddMemo(BRect ClientArea, char *name)
{
  MMemo* tmp = new MMemo(ClientArea, name);
  AddChild(tmp);
  return tmp; 
}

MPanel* MCanvas::AddPanel(BRect ClientArea, char *name)
{
  MPanel* tmp = new MPanel(ClientArea, name);
  AddChild(tmp);
  return tmp;
}

MCheckBox* MCanvas::AddCheckBox(BRect ClientArea, char *name)
{
  MCheckBox* tmp = new MCheckBox(ClientArea, name);
  AddChild(tmp);
  return tmp;
}

void MCanvas::KeyDown(const char *bytes, int32 numBytes)
{
  BView::KeyDown(bytes, numBytes);
  
  if (fOnKeyDown != NULL){
    fOnKeyDown( this, bytes, numBytes );
  }
} 

void MCanvas::KeyUp(const char *bytes, int32 numBytes)  
{
  BView::KeyUp(bytes, numBytes);
  
  if (fOnKeyUp != NULL){
    fOnKeyUp( this, bytes, numBytes );
  }
}

void MCanvas::MouseDown(BPoint pt)
{
  BPoint point;
  int32 buttons = 0;

  BView::MouseDown(pt);
  
  if (fOnMouseDown != NULL){
    fOnMouseDown( this, pt.x, pt.y );
  }
  
  Looper()->CurrentMessage()->FindInt32("buttons", &buttons);
  
  
  if (buttons == B_SECONDARY_MOUSE_BUTTON) {
    if (fPopUpMenu != NULL){
      point = pt;
      ConvertToScreen(&point);
      BMenuItem *mni = fPopUpMenu->Go(point, true);
    
      if (mni){
        MMenuItem *mmni = dynamic_cast<MMenuItem*>(mni);
        if(mmni){
          mmni->DoMenuClick(mmni);
        } 
      }
    } 
  }
}

void MCanvas::MouseUp(BPoint pt)
{
  BView::MouseUp(pt);
  
  if (fOnMouseUp != NULL){
    fOnMouseUp( this, pt.x, pt.y );
  }

  DoClick(dynamic_cast<BControl*>(this)); //simulate a 'click'
}

void MCanvas::MouseMoved(BPoint pt, uint32 code, const BMessage *msg)
{
  BView::MouseMoved(pt, code, msg);
  
  if (fOnMouseMove != NULL){
    fOnMouseMove( this, pt.x, pt.y, code, 0 );
  }
}

void MCanvas::Draw(BRect updateArea)
{
  BView::Draw(updateArea);
  
  if (fOnDraw != NULL){
    fOnDraw( this, updateArea.left, updateArea.top, updateArea.right, updateArea.bottom );
  }
 
}

/////////////////////////////////////////
//MenuItem Class...

MMenuItem::MMenuItem(const char *label):
  BMenuItem(label, new BMessage(ClickMessage)),
  MMenuEventPlugin(){}

MMenuItem::~MMenuItem(){}

/////////////////////////////////////////
//PopUpMenu Class...

MPopUpMenu::MPopUpMenu(const char *name):
  BPopUpMenu(name, false, false)
{
  //MMenuItem *mnu = new MMenuItem("test");
  //AddItem(mnu);
}

/////////////////////////////////////////
//Test the Library

CTest::CTest(void){
  FValue = 33;
}

//GetTest
int CTest::GetValue(void){
  return FValue;
}

//SetTest
void CTest::SetValue(int NewVal){
  FValue = NewVal;
}

