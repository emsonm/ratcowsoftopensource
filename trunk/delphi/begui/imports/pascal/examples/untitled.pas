program untitled;

uses
  begui;
  
type
  PTest = ^TTest;
  TTest = object
    handle: MButtonH;
    owner: MFormH;
    procedure test;
    procedure AddToForm(Form: MFormH);
    constructor init;
    destructor done; virtual;
  end;
  
  constructor TTest.init;
  begin
    write('init');
    handle := MButton_Create_int32(170,20,190,30, pchar('C'));
    writeln('.. after button create');
  end;
  
  destructor TTest.done;
  begin
    write('done');
    handle := nil;
    writeln('.. after done');
  end;
  
  procedure TTest.test;
  begin
    writeln('test');
  end;
  
  procedure TTest.AddToForm(Form: MFormH);
  begin
    owner := Form;
    MForm_AddChild(owner, handle);
  end;
  
var
  t: ptest;
  MainForm: MFormH;
begin
  new(t, init);
  
  MainForm := MApplication_GetMainForm(Application);
  
  t^.test;
  
  t^.AddToForm(MainForm);
  
  t^.test;
  
  MApplication_Run(application);
  
  dispose(t, done);
 
end.