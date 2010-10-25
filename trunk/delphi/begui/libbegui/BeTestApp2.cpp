/*
  $Header: /cvsroot/befpc/begui/begui/libbegui/BeTestApp2.cpp,v 1.2 2002/04/23 18:37:29 memson Exp $
  
  $Revision: 1.2 $
  
  $Log: BeTestApp2.cpp,v $
  Revision 1.2  2002/04/23 18:37:29  memson
  *** empty log message ***

  Revision 1.1  2002/04/12 23:32:56  memson

  Added quite a bit.

  Got basic file handling soeted out. Also got the FilePanel's working (see
  example project)

  Popup menu now only responds to a right click (at last!!)

  Revision 1.15  2002/03/26 13:28:51  memson
  added in combobox support - flawed at the moment.

  Revision 1.14  2002/03/14 22:31:41  memson
  Got the Menubar/menu/menuitem and popupmenu/menuitem both working... Had a
  bit of a slack night, so didn't really do much else.

  Revision 1.13  2002/03/14 00:12:38  memson
  Revised the event and property class hierarchies... hopefully makes more
  sense.

  Started to add in the Menu functionality. PopUpMenu is done and 'tested',
  but the BMenuBar/BMenu (mainmenu) stuff is there but untested.

  Added in an OnCreate and OnDestroy event to the events, plust a wierd
  offshoot class to handle the menuitem click.

  Revision 1.12  2002/03/12 23:12:37  memson
  hashed out some details - stopped child windows from screwing up app, and
  also added the close action to form and terminating functionality to the app.

  Revision 1.11  2002/03/12 18:39:16  memson

  Revision 1.10  2002/03/11 23:22:11  memson

  Revision 1.9  2002/02/19 17:49:54  memson
  Added half arsed support for multiple windows - not working quite right yet,
  but at least they seem to 'work' in so much as they don't wipe the app out.
  The secondary forms Canvas is screwed up a little.. that is a priority I
  guess.

  Revision 1.8  2002/02/14 23:39:53  memson
  I've now added most of the events to the MButton, MCanvas, MMemo and MEdit..
  started a MPanel component. Again, the MCanvas is *not* tread safe!!!
  This means the calling thread must create it.

  NB. Before I add much more functionality, I'm going to look at other GUI
  libs to see if there are any controls worth borrowing.

  Next control will probably be a MLabel followed by a MMenuBar and MMenuItem.
  MForm will own a MMenuBar.

  Revision 1.7  2002/02/14 20:33:19  memson
  Got the MButton OnClick, OnMouseDown/Up, OnKeyDown/Up and OnMouseMoved
  events working. Started to add the required code to the rest of the classes
  (should be a cut/paste job).

  Revision 1.6  2002/02/14 14:00:16  memson
  fiddled a bit.. nothing major.

  Revision 1.5  2002/02/13 23:26:25  memson
  Got the MMemo working.
  Got the MEdit working, including OnClick event signal
  Got the MButton working with a signal
  Added a few get/set accessor routined for captions/text.


  Revision 1.4  2002/02/13 00:02:02  memson
  Got the MTextControl basic functionality - annoying fact is that a text
  control has to be added inside the thread that will display it!!!!

  Revision 1.3  2002/02/11 23:26:44  memson
  Revision 1.2  2002/01/17 20:32:54  memson
  
*/

#include "BeGuiAPI.h"
#include "MFile.h"
#include <stdio.h>

void *frm2 = 0; //need to add a 'parent' property?? Does BeAPI have equiv?

/* button callback */
void callback(void* Sender, uint32 msg){
  if (msg == 'clik')
    if (Sender) {
      GenericAlert(MButton_getCaption((MButton*)Sender));
      MButton_setCaption((MButton*)Sender, "X");
    }
}

/* edit callback */
void callback2(void* Sender, uint32 msg){
  if (msg == 'clik')
    if (Sender) {
      GenericAlert( MEdit_getText( (MEdit*)Sender ) );
    }
}

void menuClick(void* Sender, uint32 msg){
  if (msg == 'clik')
    GenericAlert( "Menu click" );
}

/* test mousemove */
void mousemoved(void* Sender, float x, float y, uint32 code, uint32 msg){
  printf("mousemoved - x: %f, y: %f, code: %u\n", x, y, code);  
}

/* test mousedown */
void mousedown(void* Sender, float x, float y){
  printf("mousedown - x: %f, y: %f\n", x, y);  
}

/* test mouseup */
void mouseup(void* Sender, float x, float y){
  printf("mouseup - x: %f, y: %f\n", x, y);  
}

/* test button click */
void doclick(void* Sender, uint32 msg){
  if (msg == 'clik')
    if (Sender) {
      printf("%s clicked\n", MButton_getCaption((MButton*)Sender));
    }
    
  if (frm2 != NULL){
    MForm_Show((MForm*)frm2);
  }
  
  //dynamic_cast<MApplication*>(be_app)->Terminate();
}

/* test form click */
void doformclick(void* Sender, float x, float y){
  printf("mousedown - x: %f, y: %f\n", x, y);  
}


int main(void){
  /* variables */
  void *app;
  void *frm;
  void *btn, *btn2;
  void *edt, *edt2;
  void *pnl;
  void *mni, *mni2, *mnu;
  void *pm, *mb;
  void *cbx;
  void *btncbx;
  void *file;
 
  /* must create application first!! */ 
  app = MApplication_Create();
  
  /* application automatically creates main form 
     so call this to get a reference to it..  */
  frm = MApplication_GetMainForm((MApplication*)app);
  MForm_AttachMouseDownDispatcher((MForm*)frm, doformclick);
  
  /* create a second hidded form */
  frm2 = MApplication_AddForm((MApplication*)app, 100, 100, 100, 100, "testfrm", "testfrm");
  //MForm_Show((MForm*)frm2);
  
  MForm_setWidth((MForm*)frm, 250);
  MForm_setHeight((MForm*)frm, 250);
  
  /* create 2 buttons */
  btn = MButton_Create(5, 25, 30, 20, "test"); 
  btn2 = MButton_Create(5, 50, 30, 30, "test2"); 
  cbx = MCheckBox_Create(5, 5, 30, 50, "test cbx");
  
  /* add buttons to main form */
  MForm_AddChild((MForm*)frm, (BControl*)btn);
  MForm_AddChild((MForm*)frm, (BControl*)btn2);
  
  
  /* create a panel */
  pnl = MForm_AddMPanel((MForm*)frm, 5, 180, 200, 200, "panel1");
  MPanel_AddChild((MPanel*)pnl, (BControl*)cbx);
  
  pm = MPopUpMenu_Create("testmenu");
  mni = MMenuItem_Create("Test Item");
  MMenuItem_AttachMenuClickDispatcher((MMenuItem*)mni, menuClick);
  MPopUpMenu_AddItem((MPopUpMenu*)pm, (MMenuItem*)mni);
  MForm_AddPopUpMenu((MForm*)frm, (MPopUpMenu*)pm);
 
  
  mb = BMenuBar_Create((MForm*)frm, "testmenubar");
  mni2 = MMenuItem_Create("Test mb item");
  MMenuItem_AttachMenuClickDispatcher((MMenuItem*)mni2, menuClick);
  mnu = BMenu_Create("Form");
  BMenu_AddItem((BMenu*)mnu, (MMenuItem*)mni2);
  BMenuBar_AddItem((BMenuBar*)mb, (BMenu*)mnu);  
  
  /* set the button event callback routines */
  MButton_AttachClickDispatcher((MButton*)btn, callback);
  MButton_AttachClickDispatcher((MButton*)btn2, doclick);
  MButton_AttachMouseMovedDispatcher((MButton*)btn, mousemoved);
  MButton_AttachMouseUpDispatcher((MButton*)btn2, mouseup);
  MButton_AttachMouseDownDispatcher((MButton*)btn2, mousedown);
  
  /* add an edit - must be done in thread of main form */
  edt2 = MForm_AddMEdit((MForm*)frm, 30, 50, 100, 50, "test4");
 
  /* set the edit event callback routines */
  MEdit_AttachClickDispatcher((MEdit*)edt2, callback2);
    
  /* create a memo - like edit, must be done in main form's thread */ 
  edt  = MForm_AddMMemo((MForm*)frm, 30, 100, 150, 150, "test5");

  //MForm_Hide((MForm*)frm);
  //MForm_Show((MForm*)frm);
  
  /* start things rolling */
  MApplication_Run((MApplication*)app);
 
  /* clean up after execution */ 
  MApplication_Free((MApplication*)app); 
  
  printf("%d", test_func());
}
