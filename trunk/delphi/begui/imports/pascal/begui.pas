unit BeGUI;

{
$Header: /cvsroot/befpc/begui/begui/imports/pascal/begui.pas,v 1.3 2002/04/23 18:37:29 memson Exp $

$Revision: 1.3 $
  
$Log: begui.pas,v $
Revision 1.3  2002/04/23 18:37:29  memson
*** empty log message ***

Revision 1.2  2002/04/02 20:42:15  memson
updated for Eric

}

interface

uses 
  beutils;

type
  MApplicationH = pointer;
  MFormH = pointer;
  MButtonH = pointer;
  MEditH = pointer;
  MPanelH = pointer;
  MMemoH = pointer;

  MMenuBarH = pointer;
  MMenuItemH = pointer;
  MMenuH = pointer;

  MCheckBoxH  = pointer;
 
  uint32 = longword;
  int32  = longint;

var
  Application: MApplicationH;
type

   base_Message = procedure (sender:pointer; msg:uint32);cdecl;
   mouseMoved_Message = procedure (sender:pointer; x:double; y:double; code:uint32; msg:uint32);cdecl;
   mouseAction_Message = procedure (sender:pointer; x:double; y:double);cdecl;
   keyAction_Message = procedure (sender:pointer; bytes:Pchar; numBytes:int32);cdecl;
   drawAction_Message = procedure (sender:pointer; left:double; top:double; right:double; bottom:double);cdecl;

{$PACKRECORDS C}


////////////////////////////////////////////////////////////////////////////////////
// check Box
////////////////////////////////////////////////////////////////////////////////////

function MCheckBox_Create(left: double; top : double;right: double;bottom:double;caption:pchar):MCheckBoxH;cdecl; external LIBBEGUI;
function MCheckBox_Create_int32( left:int32; top:int32;right:int32; bottom:int32; caption:pchar):MCheckBoxH;cdecl; external LIBBEGUI;
function MForm_AddMCheckBox( frm:MFormH; left:double;  top:double;right:double; bottom:double; name:pchar):MCheckBoxH;cdecl; external LIBBEGUI;
function MForm_AddMCheckBox_int32( frm:MFormH; left:int32;  top:int32;right:int32; bottom:int32; name:pchar):MCheckBoxH;cdecl; external LIBBEGUI;


////////////////////////////////////////////////////////////////////////////////////
//  MenuBar
////////////////////////////////////////////////////////////////////////////////////


 function BMenuBar_Create( form : MFormh ; caption : pchar) : MMenuBarH; cdecl; external LIBBEGUI;
 function BMenu_Create(caption : Pchar):MmenuH ;cdecl; external LIBBEGUI;
 procedure BMenuBar_AddItem( menuBar : MMenuBarH; Menu : MMenuH);cdecl; external LIBBEGUI;
 procedure BMenu_AddItem(Menu : MMenuBarH; item : MMenuItemH );cdecl; external LIBBEGUI;

 function MMenuItem_Create(caption : pchar): MMenuItemh;cdecl; external LIBBEGUI;
 procedure MMenuItem_AttachMenuClickDispatcher(Menu: MMenuItemH; msg: mouseAction_Message);cdecl; external LIBBEGUI;


////////////////////////////////////////////////////////////////////////////////////
// Application
////////////////////////////////////////////////////////////////////////////////////

  function MApplication_Create: MApplicationH; cdecl; external LIBBEGUI;
  procedure MApplication_Free(app:MApplicationH); cdecl; external LIBBEGUI;
  procedure MApplication_Run(app:MApplicationH); cdecl; external LIBBEGUI;
  function MApplication_GetMainForm(app:MApplicationH):MFormH; cdecl; external LIBBEGUI;
  function MApplication_AddForm(app:MApplicationH; left:double; top:double; right:double; bottom:double;name:Pchar; caption:Pchar):MFormH; cdecl; external LIBBEGUI;
  function MApplication_AddForm_int32(app:MApplicationH; left:int32; top:int32; right:int32; bottom:int32;name:Pchar; caption:Pchar):MFormH; cdecl; external LIBBEGUI;




////////////////////////////////////////////////////////////////////////////////////
// Form
////////////////////////////////////////////////////////////////////////////////////
  function MForm_AddMEdit_int32(form:MFormH; left:int32; top:int32; right:int32; bottom:int32;  caption:Pchar):MEditH; external LIBBEGUI;
  function MForm_AddMPanel_int32(frm:MFormH; left:int32; top:int32; right:int32; bottom:int32;  name:Pchar):MPanelH;cdecl; external LIBBEGUI;
  procedure MForm_AddChild(form:MFormH; ctrl:pointer); cdecl; external LIBBEGUI;
  procedure MForm_AttachMouseMovedDispatcher(form:MFormH; msg:mouseMoved_Message);cdecl; external LIBBEGUI;
  procedure MForm_AttachMouseDownDispatcher(form:MFormH; msg:mouseAction_Message);cdecl; external LIBBEGUI;
  procedure MForm_AttachMouseUpDispatcher(form:MFormH; msg:mouseAction_Message);cdecl; external LIBBEGUI;
  procedure MForm_AttachKeyDownDispatcher(form:MFormH; msg:keyAction_Message);cdecl; external LIBBEGUI;
  procedure MForm_AttachKeyUpDispatcher(form:MFormH; msg:keyAction_Message);cdecl; external LIBBEGUI;
  procedure MForm_AttachDrawDispatcher(form:MFormH; msg:drawAction_Message);cdecl; external LIBBEGUI;
  procedure MForm_Show(frm:MFormH);cdecl; external LIBBEGUI;
  procedure MForm_Hide(frm:MFormH);cdecl; external LIBBEGUI;



////////////////////////////////////////////////////////////////////////////////////
//  button
////////////////////////////////////////////////////////////////////////////////////
  function MButton_Create(left:double; top:double; right:double; bottom:double; caption:Pchar):MButtonH;cdecl; external LIBBEGUI;
  function MButton_Create_int32(left:int32; top:int32; right:int32; bottom:int32; caption:Pchar):MButtonH; external LIBBEGUI;
  procedure MButton_AttachClickDispatcher(btn:MButtonH; msg:base_Message);cdecl; external LIBBEGUI;
  function MButton_getCaption(btn:MButtonH):pchar;cdecl; external LIBBEGUI;
  procedure MButton_setCaption(btn:MButtonH; caption:Pchar);cdecl; external LIBBEGUI;
  procedure MButton_AttachMouseMovedDispatcher(btn:MButtonH; msg:mouseMoved_Message);cdecl; external LIBBEGUI;
  procedure MButton_AttachMouseDownDispatcher(btn:MButtonH; msg:mouseAction_Message);cdecl; external LIBBEGUI;
  procedure MButton_AttachMouseUpDispatcher(btn:MButtonH; msg:mouseAction_Message);cdecl; external LIBBEGUI;
  procedure MButton_AttachKeyDownDispatcher(btn:MButtonH; msg:keyAction_Message);cdecl; external LIBBEGUI;
  procedure MButton_AttachKeyUpDispatcher(btn:MButtonH; msg:keyAction_Message);cdecl; external LIBBEGUI;
  procedure MButton_AttachDrawDispatcher(btn:MButtonH; msg:drawAction_Message);cdecl; external LIBBEGUI;



////////////////////////////////////////////////////////////////////////////////////
//  edit
////////////////////////////////////////////////////////////////////////////////////
  function MForm_AddMEdit(form:MFormH; left:double; top:double; right:double; bottom:double;caption:Pchar):MEditH;cdecl; external LIBBEGUI;
  function MEdit_getText(edt:MEditH):Pchar;cdecl; external LIBBEGUI;
  procedure MEdit_setText(edt:MEditH; text:Pchar);cdecl; external LIBBEGUI;
  procedure MEdit_AttachClickDispatcher(edt:MEditH; msg:base_Message);cdecl; external LIBBEGUI;
  procedure MEdit_AttachMouseMovedDispatcher(edt:MEditH; msg:mouseMoved_Message);cdecl; external LIBBEGUI;
  procedure MEdit_AttachMouseDownDispatcher(edt:MEditH; msg:mouseAction_Message);cdecl; external LIBBEGUI;
  procedure MEdit_AttachMouseUpDispatcher(edt:MEditH; msg:mouseAction_Message);cdecl; external LIBBEGUI;
  procedure MEdit_AttachKeyDownDispatcher(edt:MEditH; msg:keyAction_Message);cdecl; external LIBBEGUI;
  procedure MEdit_AttachKeyUpDispatcher(edt:MEditH; msg:keyAction_Message);cdecl; external LIBBEGUI;
  procedure MEdit_AttachDrawDispatcher(edt:MEditH; msg:drawAction_Message);cdecl; external LIBBEGUI;



////////////////////////////////////////////////////////////////////////////////////
//  memo
////////////////////////////////////////////////////////////////////////////////////
  function MForm_AddMMemo(form:MFormH; left:double; top:double; right:double; bottom:double; caption:Pchar):MMemoH;cdecl; external LIBBEGUI;
  function MForm_AddMMemo_int32(form:MFormH; left:int32; top:int32; right:int32; bottom:int32;  caption:Pchar):MMemoH; cdecl; external LIBBEGUI;
  procedure MMemo_AttachMouseMovedDispatcher(memo:MMemoH; msg:mouseMoved_Message);cdecl; external LIBBEGUI;
  procedure MMemo_AttachMouseDownDispatcher(memo:MMemoH; msg:mouseAction_Message);cdecl; external LIBBEGUI;
  procedure MMemo_AttachMouseUpDispatcher(memo:MMemoH; msg:mouseAction_Message);cdecl; external LIBBEGUI;
  procedure MMemo_AttachKeyDownDispatcher(memo:MMemoH; msg:keyAction_Message);cdecl; external LIBBEGUI;
  procedure MMemo_AttachKeyUpDispatcher(memo:MMemoH; msg:keyAction_Message);cdecl; external LIBBEGUI;
  procedure MMemo_AttachDrawDispatcher(memo:MMemoH; msg:drawAction_Message);cdecl; external LIBBEGUI;
  function MMemo_Text(memo:MMemoH) :pchar ;cdecl; external LIBBEGUI;
  function MMemo_TextLength(memo:MMemoH) : Integer ;cdecl; external LIBBEGUI;
  procedure MMemo_SetText(memo :MMemoH; text: Pchar);cdecl; external LIBBEGUI;


////////////////////////////////////////////////////////////////////////////////////
//  panel
////////////////////////////////////////////////////////////////////////////////////
  function MForm_AddMPanel(frm:MFormH; left:double; top:double; right:double; bottom:double;name:Pchar):MPanelH;cdecl; external LIBBEGUI;



  {needed?? }
  function GetBaseMessage:uint32;cdecl; external LIBBEGUI;

  function GetNextMessage:uint32;cdecl; external LIBBEGUI;

  procedure GenericAlert(message:Pchar);cdecl; external LIBBEGUI;

  procedure testfloat(f: integer);cdecl; external LIBBEGUI;

implementation

initialization
  Application := MApplication_create;
finalization
  MApplication_free(Application);
end.