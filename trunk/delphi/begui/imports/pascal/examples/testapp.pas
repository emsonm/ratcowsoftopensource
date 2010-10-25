program TespApp;

uses BeGUI;

var
  TestMsg: array[0..10] of char;

  Button1: MButtonH;
  Button2: MButtonH;
  Edit1: MEditH;
  Memo : MMemoH;
  MainForm: MFormH;
  a: integer;

procedure EventTest(sender: pointer; msg: uint32);
begin
  if sender <> nil then
    GenericAlert(MButton_getCaption(sender));
end;

procedure Eventlength(sender: pointer; msg: uint32);
Var test : Pchar;
begin
  if sender <> nil then
  Begin
     MMemo_SetText(memo,pchar('Free Pascal Compiler v0.6.0'));
     GenericAlert(MMemo_Text(memo));
    End;
end;


begin
  TestMsg := 'Test'#0;
 
  MainForm := MApplication_GetMainForm(Application);

  Button1 := MButton_Create_int32(10, 10, 50, 20, @TestMsg);
  MButton_AttachClickDispatcher(Button1, base_Message(@EventTest));

  MForm_AddChild(MainForm, Button1);

  Edit1 := MForm_AddMEdit_int32(MainForm, 30, 100, 150, 150, @TestMsg);
 
  Memo := MForm_AddMMemo_int32(MainForm,30,200,200,400,pchar('Memo'));

  Button2 := MButton_Create_int32(100, 120, 150, 140, pchar('Change'));
  MButton_AttachClickDispatcher(Button2, base_Message(@Eventlength));
  MForm_AddChild(MainForm, Button2);

  MApplication_Run(Application);
end.