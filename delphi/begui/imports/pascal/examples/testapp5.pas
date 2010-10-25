program testapp;

uses 
  beutils;
 
var
  i: status_t;
begin
   
  i := beep;
  
  i := system_beep(NEWEMAIL_SOUND);
end.