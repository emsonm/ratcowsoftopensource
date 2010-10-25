program TespApp4;

uses BeGUI,About,Strings;


Type TButton = Class(TObject)
         private
              	MButton : pointer;
        public
		Top,Left,Width,heitgh : Integer;
             	Constructor Create(Form : MFormh;aTop,aLeft,aWidth,aheitgh : Integer);
		//Property OnClick :base_Message Write Click_Action;
                   Click_action :base_Message;
End;
    	PButton = ^TButton;

Constructor TButton.Create(Form : MFormh;aTop,aLeft,aWidth,aheitgh : Integer);
Begin
  // GenericAlert(pchar('create'));
    MButton:=MButton_Create_int32(170,20,190,30, pchar('C'));
    MForm_AddChild(Form, MButton);
End;

Var But : TButton;

procedure MenuClick(sender: pointer;  x:double; y:double);
begin
  if sender <> nil then
    GenericAlert(pchar('click'));
end;

procedure MenuClick_Quit(sender: pointer;  x:double; y:double);
Begin
  MApplication_Free(Application);    
End;

procedure MenuClick_About(sender: pointer;  x:double; y:double);
Begin
  // Display Help
    MForm_Show(Form_About);

End;


Var
  Button: Array[1..10] Of MButtonH;
  Button_Clear : MButtonH;
  Button_Point : MButtonH;
  Button_Egal : MButtonH;
  Button_Plus : MButtonH;
  Button_Minus : MButtonH;
  Button_Multi : MButtonH;
  Button_Divi : MButtonH;
  Edit: MEditH;
  Mem_Edit : String[20];
  Mem_Op  : String[20];
  Mem_Operande : integer;
  
  Check1 : MCheckBoxH;

Const
    Pos_X : Array[1..3] of integer=(20,60,100);
    Pos_Y : Array[1..4] of integer=(45,70,95,120);
    weith = 30;
    heigth = 10;

procedure EventTest(sender: pointer; msg: uint32);
begin
  if sender <> nil then
    GenericAlert(MButton_getCaption(sender));
end;

Procedure EventButton(sender: pointer; msg: uint32);
var 
  Test: array[0..20] of char;
   value: char;
   value_real : real;
   value_real2: real;
   value_result: real;
   er : Integer;
Begin
  If sender <> nil Then
  Begin
     value := strPas(MButton_getCaption(sender))[1];

     // For digit
     If ( value ='1') Or ( value ='2') Or ( value ='3') or ( value ='4')
     or ( value ='5') Or ( value ='6') Or ( value ='7') or ( value ='8') or (value='9') or (value='0')Then
    Begin 
       //=C2=A0Add Value to the sting
      Mem_Edit:=Mem_Edit+strPas(MButton_getCaption(sender));
      StrPCopy(test,Mem_Edit);
      MEdit_setText(Edit,Test);
    End;
    Val(Mem_Edit,value_real,er);
    // Clear
    if ( value='C') Then
   Begin
      Mem_Op:='';
      Mem_Operande:=0;
      Mem_Edit:='';
      MEdit_setText(Edit,nil);
   End;
   
   // dot
   If (Value='.') Then
   Begin
      If (Frac(value_real) = 0) Then 
      Begin
        Mem_Edit:=Mem_Edit+'.';
        StrPCopy(test,Mem_Edit);
        MEdit_setText(Edit,Test);
      End;
   End;
   
   // Egal
   If (Value='=') Then
   Begin
      If (Mem_Op = '' ) Then
      Begin
          Str(value_real:6:2,Mem_Edit);     
      End Else
      Begin
              Val(Mem_Op,value_real2,er);
              Case Mem_Operande of
                  1 :  Value_result:=value_real2+value_real;    
                  2 :  Value_result:=value_real2-value_real;    
                  3 :  Value_result:=value_real2*value_real;    
                  4 :  Value_result:=value_real2/value_real;    
              End;
              Str(value_result:6:2,Mem_Edit);     
      End;
      StrPCopy(test,Mem_Edit);
      MEdit_setText(Edit,Test);
   End;

   // Plus
   If (Value='+') Then
   Begin
        Mem_Op:=Mem_Edit;
        Mem_Operande:=1;
        Mem_Edit:='';
        StrPCopy(test,Mem_Edit);
        MEdit_setText(Edit,Test);
   End;

   // Minus
   If (Value='-') Then
   Begin
        Mem_Op:=Mem_Edit;
        Mem_Operande:=2;
        Mem_Edit:='';
        StrPCopy(test,Mem_Edit);
        MEdit_setText(Edit,Test);
   End;

   // Multi
   If (Value='*') Then
   Begin
        Mem_Op:=Mem_Edit;
        Mem_Operande:=3;
        Mem_Edit:='';
        StrPCopy(test,Mem_Edit);
        MEdit_setText(Edit,Test);
   End;
   
   // Division
   If (Value='/') Then
   Begin
        Mem_Op:=Mem_Edit;
        Mem_Operande:=4;
        Mem_Edit:='';
        StrPCopy(test,Mem_Edit);
        MEdit_setText(Edit,Test);
   End;
   
  End;
End;

procedure Create_Calculator(Calculator : MFormH);
Begin
  Mem_Edit:='';
  Mem_Op:='';
  { create the form of the calculator }
  { ??? - MApplication_AddForm don't take the value of size and position }
  
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    // Initialisation for the digit number
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    Button[1] := MButton_Create_int32(Pos_X[1], Pos_Y[1], Pos_X[1]+weith, Pos_Y[1]+heigth, pchar('1'));
    MForm_AddChild(Calculator, Button[1]);
    MButton_AttachClickDispatcher(Button[1], base_Message(@EventButton));

    Button[2] := MButton_Create_int32(Pos_X[2],Pos_Y[1], Pos_X[2]+weith, Pos_Y[1]+heigth, pchar('2'));
    MForm_AddChild(Calculator, Button[2]);
    MButton_AttachClickDispatcher(Button[2], base_Message(@EventButton));

    Button[3] := MButton_Create_int32(Pos_X[3],Pos_Y[1], Pos_X[3]+weith, Pos_Y[1]+heigth, pchar('3'));
    MForm_AddChild(Calculator, Button[3]);
    MButton_AttachClickDispatcher(Button[3], base_Message(@EventButton));

    Button[4] := MButton_Create_int32(Pos_X[1],Pos_Y[2], Pos_X[1]+weith, Pos_Y[2]+heigth, pchar('4'));
    MForm_AddChild(Calculator, Button[4]);
    MButton_AttachClickDispatcher(Button[4], base_Message(@EventButton));

    Button[5] := MButton_Create_int32(Pos_X[2], Pos_Y[2], Pos_X[2]+weith, Pos_Y[2]+heigth, pchar('5'));
    MForm_AddChild(Calculator, Button[5]);
    MButton_AttachClickDispatcher(Button[5], base_Message(@EventButton));

    Button[6] := MButton_Create_int32(Pos_X[3],Pos_Y[2], Pos_X[3]+weith, Pos_Y[2]+heigth, pchar('6'));
    MForm_AddChild(Calculator, Button[6]);
    MButton_AttachClickDispatcher(Button[6], base_Message(@EventButton));

    Button[7] := MButton_Create_int32(Pos_X[1],Pos_Y[3], Pos_X[1]+weith, Pos_Y[3]+heigth, pchar('7'));
    MForm_AddChild(Calculator, Button[7]);
    MButton_AttachClickDispatcher(Button[7], base_Message(@EventButton));

    Button[8] := MButton_Create_int32(Pos_X[2],Pos_Y[3], Pos_X[2]+weith, Pos_Y[3]+heigth, pchar('8'));
    MForm_AddChild(Calculator, Button[8]);
    MButton_AttachClickDispatcher(Button[8], base_Message(@EventButton));

    Button[9] := MButton_Create_int32(Pos_X[3],Pos_Y[3], Pos_X[3]+weith, Pos_Y[3]+heigth, pchar('9'));
    MForm_AddChild(Calculator, Button[9]);
    MButton_AttachClickDispatcher(Button[9], base_Message(@EventButton));

    Button[10] := MButton_Create_int32(Pos_X[1],Pos_Y[4], Pos_X[1]+weith, Pos_Y[4]+heigth, pchar('0'));
    MForm_AddChild(Calculator, Button[10]);
    MButton_AttachClickDispatcher(Button[10], base_Message(@EventButton));

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    // Initialisation for the edit
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    Edit := MForm_AddMEdit_int32(Calculator, 9, 20, 160, 30, pchar('toto'));


    //////////////////////////////////////////////////////////////////////////////////////////////////////
    // Initialisation for operation button
    //////////////////////////////////////////////////////////////////////////////////////////////////////

    Button_Clear:=MButton_Create_int32(170,20,190,30, pchar('C'));
    MForm_AddChild(Calculator, Button_clear);
    MButton_AttachClickDispatcher(Button_Clear, base_Message(@EventButton));

    Button_Egal:=MButton_Create_int32(170,45,190,55, pchar('='));
    MForm_AddChild(Calculator, Button_Egal);
    MButton_AttachClickDispatcher(Button_Egal, base_Message(@EventButton));

    Button_Plus:=MButton_Create_int32(170,70,190,75, pchar('+'));
    MForm_AddChild(Calculator, Button_Plus);
    MButton_AttachClickDispatcher(Button_Plus, base_Message(@EventButton));

    Button_Minus:=MButton_Create_int32(170,95,190,105, pchar('-'));
    MForm_AddChild(Calculator, Button_Minus);
    MButton_AttachClickDispatcher(Button_Minus, base_Message(@EventButton));

    Button_Multi:=MButton_Create_int32(170,120,190,130, pchar('*'));
    MForm_AddChild(Calculator, Button_Multi);
    MButton_AttachClickDispatcher(Button_Multi, base_Message(@EventButton));

    Button_Divi := MButton_Create_int32(170,145,190,155, pchar('/'));
    MForm_AddChild(Calculator, Button_Divi);
    MButton_AttachClickDispatcher(Button_Divi, base_Message(@EventButton));

    Button_Point := MButton_Create_int32(Pos_X[2],Pos_Y[4], Pos_X[2]+weith, Pos_Y[4]+heigth, pchar('.'));
    MForm_AddChild(Calculator, Button_Point);
    MButton_AttachClickDispatcher(Button_Point, base_Message(@EventButton));

End;

//poo

var
  MainForm: MFormH;
  
  mb : MMenuBarH;
  Menu_About,
  Menu_Quit : MMenuItemH;
 
  mnu : MMenuH;
  mnu_Help : MMenuH;

begin
 
  MainForm := MApplication_GetMainForm(Application);

 Check1:=MCheckBox_Create_int32(250,100,260,110,pchar('toto'));
 MForm_AddChild(MainForm, Check1);
  Check1:=MForm_AddMCheckBox_int32( MainForm,250,200,260,210,pchar('toto'));
// MForm_AddChild(MainForm, Check1);
 

  mb := BMenuBar_Create(MainForm, pchar('main'));
  Menu_Quit := MMenuItem_Create(pchar('Close'));
  MMenuItem_AttachMenuClickDispatcher(Menu_Quit, @menuClick_Quit);

  mnu := BMenu_Create(pchar('Calculator'));
  BMenu_AddItem(mnu, Menu_Quit);
  BMenuBar_AddItem(mb, mnu);  

  mnu_Help := BMenu_Create(pchar('?'));
  Menu_About := MMenuItem_Create(pchar('About...'));
  BMenu_AddItem(Mnu_Help, Menu_About);
  MMenuItem_AttachMenuClickDispatcher(Menu_About, @menuClick_About);
  BMenuBar_AddItem(mb, mnu_Help);  

  // test Object
 //but := TButton.Create(MainForm,20,20,30,30);

  //Create_Calculator(MainForm);
  Create_About;

  MApplication_Run(Application);
end.