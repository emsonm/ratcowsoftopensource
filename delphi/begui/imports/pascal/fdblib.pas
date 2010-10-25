unit fdblib;

interface

procedure SendText(text: pchar);cdecl; external 'fdb';
procedure SendText(text : string);

procedure force;

implementation

procedure SendText(text : string);
var
  local : string;
begin
  local := text + #0;
  SendText(@local[1]);
end;

procedure force;
begin
  writeln('force');
end;

initialization
  SendText('----------------------');
  SendText('App start');

finalization
  SendText('App end');

end.
