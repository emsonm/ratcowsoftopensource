#ifndef BEGUICLASSES_H
#define BEGUICLASSES_H

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
#include <MenuItem.h>
#include <PopUpMenu.h>
#include <Menu.h>
#include <CheckBox.h>
#include <RadioButton.h>
#include <kernel/OS.h>
#include "MList.h"

////////////////////////////////////////////////////////////////////

enum MCloseAction {caNone=0, caHide, caFree};

typedef void (*base_Message)(void* sender, uint32 msg); //callback event 
typedef void (*genericEvent_Message)(void* sender);
typedef void (*mouseMoved_Message)(void* sender, float x, float y, uint32 code, uint32 msg); 
typedef void (*mouseAction_Message)(void* sender, float x, float y); 
typedef void (*keyAction_Message)(void* sender, const char *bytes, int32 numBytes); 
typedef void (*drawAction_Message)(void* sender, float left, float top, float right, float bottom); 
typedef void (*closeAction_Message)(void* sender, MCloseAction &closeAction);
typedef void (*closeQueryAction_Message)(void* sender, bool &canClose);
 

//Classes...

class CTest {
private:
  	int FValue;
public:
	CTest(void);
	int GetValue(void);
	void SetValue(int);
};

///////////////////////////////////////////////

class MGUIListItem : public MListItem {
public:
  MGUIListItem(BString name, void *item);
  virtual BString getName(void);
  virtual void setName(BString name);
private:
  BString fName;
};

// GUI Item store....

class MGUIList : public MList {
public:
  MGUIList() : MList(){ /*nothing*/ };
  virtual MGUIListItem* GetItem(int32 index);
};

///////////////////////////////////////////////

class MPropertyPlugin{
protected:
  BArchivable *FOwner;
  uint32 FTag;
public:
  MPropertyPlugin(BArchivable *AOwner);  
  MPropertyPlugin();
  virtual ~MPropertyPlugin();  
  virtual void setTag(uint32 ATag);
  virtual uint32 getTag(void); 
  virtual BArchivable* getOwner(void);
  virtual void setOwner(BArchivable *AOwner);
};

class MPopUpMenu; //forward

class MMenuPropertyPlugin: public MPropertyPlugin{
protected:
  MPopUpMenu *fPopUpMenu;  
public:
  MMenuPropertyPlugin(BArchivable *AOwner);
  virtual ~MMenuPropertyPlugin();
  virtual void setPopUpMenu(MPopUpMenu *menu);
  virtual MPopUpMenu* getPopUpMenu(void);
};

///////////////////////////////////////////////

class MEventPlugin{
protected:
  genericEvent_Message fOnCreate;
  genericEvent_Message fOnDestroy;
public:
  MEventPlugin();
  virtual ~MEventPlugin();
  //event stuff: Dispatchers
  virtual void AttachCreateDispatcher(genericEvent_Message value);
  virtual void AttachDestroyDispatcher(genericEvent_Message value);
  //event stuff: Events
  virtual void DoCreate(BControl *self);
  virtual void DoDestroy(BControl *self);
};

class MMenuItem; //forward

class MMenuEventPlugin{
protected:
  base_Message fOnMenuClick;
public:
  MMenuEventPlugin();
  virtual ~MMenuEventPlugin();
  //
  virtual void DoMenuClick(MMenuItem *self);
  //
  virtual void AttachMenuClickDispatcher(base_Message value);

};

class MBasicEventPlugin: public MEventPlugin{
protected:
  base_Message fOnClick;
  base_Message fOnChange;
public:
  MBasicEventPlugin();
  virtual ~MBasicEventPlugin();
  //
  virtual void DoClick(BControl *self);
  virtual void DoChange(BControl *self);
  //
  virtual void AttachClickDispatcher(base_Message value);
  virtual void AttachChangeDispatcher(base_Message value);
};

class MControlEventPlugin: public MBasicEventPlugin{
protected:
  mouseMoved_Message fOnMouseMove;
  mouseAction_Message fOnMouseUp;
  mouseAction_Message fOnMouseDown;
  keyAction_Message fOnKeyUp;
  keyAction_Message fOnKeyDown;
  keyAction_Message fOnKeyPress;
  drawAction_Message fOnDraw;
public:
  MControlEventPlugin();
  virtual ~MControlEventPlugin();
  //event stuff: Dispatchers
  virtual void AttachMouseDownDispatcher(mouseAction_Message value);
  virtual void AttachMouseUpDispatcher(mouseAction_Message value);
  virtual void AttachMouseMovedDispatcher(mouseMoved_Message value);
  virtual void AttachKeyDownDispatcher(keyAction_Message value);
  virtual void AttachKeyUpDispatcher(keyAction_Message value);
  virtual void AttachKeyPressDispatcher(keyAction_Message value);
  virtual void AttachDrawDispatcher(drawAction_Message value);  
  //event stuff: Events
  virtual void DoKeyPress(BControl *self, const char *bytes, int32 numBytes);
};

class MFormEventPlugin: public MEventPlugin{
protected:
  closeAction_Message fOnClose;
public:
  MFormEventPlugin();
  virtual ~MFormEventPlugin();
  virtual void AttachCloseDispatcher(closeAction_Message value);
  virtual void DoClose(BView *self, MCloseAction &closeAction);
};

///////////////////////////////////////////////

class MMemo : public BScrollView, 
              public MControlEventPlugin,
              public MPropertyPlugin{
private:
  BTextView* fTextArea;
public:
  MMemo(char *name);
  MMemo(BRect frame, char *name);
  BTextView* getTextView(void);
  virtual void AttachedToWindow();
  //event stuff: Events
  virtual void KeyDown(const char *bytes, int32 numBytes); 
  virtual void KeyUp(const char *bytes, int32 numBytes); 
  virtual void MouseDown(BPoint pt); 
  virtual void MouseUp(BPoint pt); 
  virtual void MouseMoved(BPoint pt, uint32 code, const BMessage *msg);
  virtual void Draw(BRect updateArea);
};


class MEdit : public BTextControl, 
              public MControlEventPlugin,
              public MPropertyPlugin{
public:
  MEdit(char *name);
  MEdit(BRect frame, char *name);
  virtual ~MEdit();
  //event stuff: Events
  virtual void KeyDown(const char *bytes, int32 numBytes); 
  virtual void KeyUp(const char *bytes, int32 numBytes); 
  virtual void MouseDown(BPoint pt); 
  virtual void MouseUp(BPoint pt); 
  virtual void MouseMoved(BPoint pt, uint32 code, const BMessage *msg);
  virtual void Draw(BRect updateArea);
};

class MButton : public BButton, 
                public MControlEventPlugin,
                public MPropertyPlugin{
private:
  uint32 fMessage;
public:
  MButton(char *name);
  MButton(BRect frame, char *name);
  virtual ~MButton();
  //event stuff: Events
  virtual void KeyDown(const char *bytes, int32 numBytes); 
  virtual void KeyUp(const char *bytes, int32 numBytes); 
  virtual void MouseDown(BPoint pt); 
  virtual void MouseUp(BPoint pt); 
  virtual void MouseMoved(BPoint pt, uint32 code, const BMessage *msg);
  virtual void Draw(BRect updateArea);
};

class MCheckBox : public BCheckBox,
                  public MControlEventPlugin,
                  public MPropertyPlugin{
public:
  MCheckBox(BRect frame, char *name);                    
  virtual ~MCheckBox();
  virtual void KeyDown(const char *bytes, int32 numBytes); 
  virtual void KeyUp(const char *bytes, int32 numBytes); 
  virtual void MouseDown(BPoint pt); 
  virtual void MouseUp(BPoint pt); 
  virtual void MouseMoved(BPoint pt, uint32 code, const BMessage *msg);
  virtual void Draw(BRect updateArea);
  virtual bool Checked(void);
  
};     

class MRadioButton : public BRadioButton,
                     public MControlEventPlugin,
                     public MPropertyPlugin{
public:
  MRadioButton(BRect frame, char *name);                    
  virtual ~MRadioButton();
  virtual void KeyDown(const char *bytes, int32 numBytes); 
  virtual void KeyUp(const char *bytes, int32 numBytes); 
  virtual void MouseDown(BPoint pt); 
  virtual void MouseUp(BPoint pt); 
  virtual void MouseMoved(BPoint pt, uint32 code, const BMessage *msg);
  virtual void Draw(BRect updateArea);
  virtual bool Checked(void);
  
};             

///////////////////////////////////////////////

class MMenuItem: public BMenuItem,
                 public MMenuEventPlugin{
public:
  MMenuItem(const char *label);
  virtual ~MMenuItem();   
};

class MPopUpMenu: public BPopUpMenu{
public:
  MPopUpMenu(const char *name);
  virtual ~MPopUpMenu(){};
};

///////////////////////////////////////////////

class MPanel; //forward
class MComboBox;

class MCanvas : public BView, 
                public MControlEventPlugin,
                public MMenuPropertyPlugin{
public:
  MCanvas(BRect ClientArea, char *name);
  virtual ~MCanvas();
  virtual void AttachedToWindow();
  virtual MEdit* AddEdit(BRect ClientArea, char *name);
  virtual MMemo* AddMemo(BRect ClientArea, char *name);
  virtual MPanel* AddPanel(BRect ClientArea, char *name);
  virtual MCheckBox* AddCheckBox(BRect ClientArea, char *name);
  //event stuff: Events
  virtual void KeyDown(const char *bytes, int32 numBytes); 
  virtual void KeyUp(const char *bytes, int32 numBytes); 
  virtual void MouseDown(BPoint pt); 
  virtual void MouseUp(BPoint pt); 
  virtual void MouseMoved(BPoint pt, uint32 code, const BMessage *msg);
  virtual void Draw(BRect updateArea);
};

class MPanel : public MCanvas{
public:
  MPanel(BRect ClientArea, char *name);
  virtual void Draw(BRect updateArea);
};

class MForm : public BWindow,
              public MFormEventPlugin{
protected:
  BRect fClientArea;
  MCanvas *fCanvas;
  MCloseAction fCloseAction;
private:
  void UpdateBounds(void);
public:
  MForm(BRect ClientArea);
  virtual ~MForm();
  virtual bool QuitRequested();
  virtual float getWidth(void);
  virtual float getHeight(void);
  virtual void  setWidth(float w);
  virtual void  setHeight(float h);
  virtual float getLeft(void);
  virtual float getTop(void);
  virtual float getBottom(void);
  virtual float getRight(void); 
  virtual MCanvas* Canvas(void);
  virtual void MessageReceived(BMessage *message);
  virtual MCloseAction getCloseAction(void);
  virtual void setCloseAction(MCloseAction ca);
};

class MApplication : public BApplication,
                     public MEventPlugin{
protected:
  MForm *fMainForm, *fecker;
  MGUIList *fForms; 
  bool fTerminating;
public:
  MApplication(const char* signature = "application/x-vnd.beguiapp");
  virtual ~MApplication();
  virtual thread_id Run(void); 
  virtual void Terminate(void);
  virtual void RefsReceived(BMessage *message);
  virtual void MessageReceived(BMessage *message);
  virtual MForm* AddForm(BRect ClientArea, BString name, bool showForm = false);
  virtual MForm* GetMainForm(void){ return fMainForm; };
  virtual bool Terminating(void){ return fTerminating; };
  virtual void setTerminating(bool Value){ fTerminating = Value; };
}; 
 
#endif

