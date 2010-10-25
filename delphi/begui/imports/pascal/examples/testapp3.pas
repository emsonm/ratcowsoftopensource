program TespApp3;

uses BeGUI, strings;

Var
  Calculator : MFormH;
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
  
  Calculator_Visible : Boolean;

Const
    Pos_X : Array[1..3] of integer=(10,50,90);
    Pos_Y : Array[1..4] of integer=(35,60,85,110);
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

procedure Create_Calculator;
var
  Edit1: MEditH;
  No_Button : Integer;
Begin
  Mem_Edit:='';
  Mem_Op:='';
  Calculator_Visible:=false;
  { create the form of the calculator }
  { ??? - MApplication_AddForm don't take the value of size and position }
  Calculator := MApplication_AddForm_int32(Application,250, 100, 500, 300, pchar('Calculator'),pchar('Calculator'));
  
  { create the 9 digits of the calculator }
  { it's make error if i used a array of MButtonH
  for No_Button:=1 to 9 Do
  Begin
    Button[No_Button] := MButton_Create_int32(10+(No_Button*10), 10, 50+(No_Button*10), 20, pchar('1'));
    {MButton_AttachClickDispatcher(Button1, base_Message(@EventTest));}
    MForm_AddChild(Calculator, Button[No_Button]);
  End;
  }
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
    Edit := MForm_AddMEdit_int32(Calculator, 9, 10, 160, 20, pchar('toto'));


    //////////////////////////////////////////////////////////////////////////////////////////////////////
    // Initialisation for operation button
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    Button_Clear:=MButton_Create_int32(170,10,190,20, pchar('C'));
    MForm_AddChild(Calculator, Button_clear);
    MButton_AttachClickDispatcher(Button_Clear, base_Message(@EventButton));

    Button_Egal:=MButton_Create_int32(170,35,190,45, pchar('='));
    MForm_AddChild(Calculator, Button_Egal);
    MButton_AttachClickDispatcher(Button_Egal, base_Message(@EventButton));

    Button_Plus:=MButton_Create_int32(170,60,190,65, pchar('+'));
    MForm_AddChild(Calculator, Button_Plus);
    MButton_AttachClickDispatcher(Button_Plus, base_Message(@EventButton));

    Button_Minus:=MButton_Create_int32(170,85,190,95, pchar('-'));
    MForm_AddChild(Calculator, Button_Minus);
    MButton_AttachClickDispatcher(Button_Minus, base_Message(@EventButton));

    Button_Multi:=MButton_Create_int32(170,110,190,120, pchar('*'));
    MForm_AddChild(Calculator, Button_Multi);
    MButton_AttachClickDispatcher(Button_Multi, base_Message(@EventButton));

    Button_Divi:=MButton_Create_int32(170,135,190,145, pchar('/'));
    MForm_AddChild(Calculator, Button_Divi);
    MButton_AttachClickDispatcher(Button_Divi, base_Message(@EventButton));

    Button_Point := MButton_Create_int32(Pos_X[2],Pos_Y[4], Pos_X[2]+weith, Pos_Y[4]+heigth, pchar('.'));
    MForm_AddChild(Calculator, Button_Point);
    MButton_AttachClickDispatcher(Button_Point, base_Message(@EventButton));

End;

procedure Show_Calculator;
Begin
  MForm_Show(Calculator);
  Calculator_Visible:=true;
End;

procedure Hide_Calculator;
Begin
  MForm_Hide(Calculator);
  Calculator_Visible:=false;
End;

Procedure End_Calculator;
Begin
End;

procedure Launch(sender: pointer; msg: uint32);
begin
  if sender <> nil then
  Begin
    If Not Calculator_Visible Then Show_Calculator
                                            Else Hide_Calculator;
  End;
end;



var
  TestMsg: array[0..10] of char;

  Button1: MButtonH;
  Launch_calc: MButtonH;
  Edit1: MEditH;
  MainForm: MFormH;
  i,a: integer;


begin
  TestMsg := 'Test'#0;
 
  MainForm := MApplication_GetMainForm(Application);

  Button1 := MButton_Create_int32(10, 10, 50, 20, @TestMsg);
  MButton_AttachClickDispatcher(Button1, base_Message(@EventTest));
  MForm_AddChild(MainForm, Button1);

  Launch_calc := MButton_Create_int32(10, 30, 100, 40, pchar('Calculator'));
  MButton_AttachClickDispatcher(Launch_calc, base_Message(@launch));
  MForm_AddChild(MainForm, Launch_calc);


   Edit1 := MForm_AddMEdit_int32(MainForm, 30, 100, 150, 150, @TestMsg);


  { test for MDI}
  Create_Calculator;

  MApplication_Run(Application);
end.

