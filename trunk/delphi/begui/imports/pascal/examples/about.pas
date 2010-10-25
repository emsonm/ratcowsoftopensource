Unit About;

interface

uses 
  BeGUI;

var
  Form_About : MFormH;
  Button_Divi : MButtonH;
  Edit: MEditH;

procedure Create_About;

Implementation

procedure EventButton(sender: pointer; msg: uint32);
begin
  MForm_Hide(Form_About);
end;


procedure Create_About;
begin
  Form_About := MApplication_AddForm_int32(Application,250, 100, 500, 300, pchar('About'),pchar('About'));
  

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    // Initialisation for the edit
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    Edit := MForm_AddMEdit_int32(Form_About, 9, 10, 160, 20, pchar('Calculator v1.0'));
    MEdit_setText(Edit,pchar('Calculator v1.0'));


    Button_Divi := MButton_Create_int32(160,135,200,145, pchar('OK'));
    MForm_AddChild(Form_About, Button_Divi);
    MButton_AttachClickDispatcher(Button_Divi, base_Message(@EventButton));
end;

end.