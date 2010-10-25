unit beutils;

interface

{$PACKRECORDS C}

const
  LIBROOT = 'root';
  LIBBE = 'be';
  LIBBEGUI = 'begui';
  
  _BEEP_FLAGS = 0;

{----- From types.h -------------------------------------------}
type
  blkcnt_t = int64;
  blksize_t = longint;
  fsblkcnt_t = int64;
  fsfilcnt_t = int64;
  ino_t = int64;
  cnt_t = longint;
  mode_t = dword;
  nlink_t = longint;
  dev_t = longint;
  off_t = int64;
  pid_t = longint;
  uid_t = dword;
  gid_t = dword;
  umode_t = dword;
  daddr_t = longint;

  { bsd  }
  u_char = byte;
  u_short = word;
  u_int = dword;
  u_long = dword;

  { sysv  }
  unchar = byte;
  ushort = word;
  uint = dword;
  ulong = dword;
  caddr_t = char;

{--------------------------------------------------------------}
{----- From fcntl.h (posix)  ----------------------------------}

const
  F_DUPFD = $0001;
  F_GETFD = $0002;
  F_SETFD = $0004;
  F_GETFL = $0008;
  F_SETFL = $0010;
  F_GETLK = $0020;
  F_RDLCK = $0040;
  F_SETLK = $0080;
  F_SETLKW = $0100;
  F_UNLCK = $0200;
  F_WRLCK = $0400;

const
  O_RDONLY = 0;         { read only  }
  O_WRONLY = 1;         { write only  }
  O_RDWR = 2;           { read and write  }
  O_RWMASK = 3;         { Mask to get open mode  }
  O_CLOEXEC = $0040;    { close fd on exec  }
  O_NONBLOCK = $0080;   { non blocking io  }
  O_EXCL = $0100;       { exclusive creat  }
  O_CREAT = $0200;      { create and open file  }
  O_TRUNC = $0400;      { open with truncation  }
  O_APPEND = $0800;     { to end of file  }
  O_NOCTTY = $1000;     { currently unsupported  }
  O_NOTRAVERSE = $2000; { do not traverse leaf link  }
  O_ACCMODE = $0003;    { currently unsupported  }
  O_TEXT = $4000;       { CR-LF translation	 }
  O_BINARY = $8000;     { no translation	 }

const
  FD_CLOEXEC = 1;

const
  { #define O_DSYNC XXXdbg  }
  { #define O_RSYNC XXXdbg  }
  { #define O_SYNC  XXXdbg  }
  S_IREAD = $0100;  { owner may read  }
  S_IWRITE = $0080; { owner may write  }

type
  flock = record
    l_type : smallint;
    l_whence : smallint;
    l_start : off_t;
    l_len : off_t;
    l_pid : pid_t;
  end;

function creat(path:Pchar; mode:mode_t):longint;cdecl; external LIBROOT;
//function open(pathname:Pchar; oflags:longint; args:array of const):longint; cdecl; external LIBROOT;
function open(pathname:Pchar; oflags:longint):longint; cdecl; external LIBROOT;
{ the third argument is the permissions of the created file when O_CREAT is passed in oflags  }
//function fcntl(fd:longint; op:longint; args: array of const):longint; cdecl; external LIBROOT;
function fcntl(fd:longint; op:longint):longint; cdecl; external LIBROOT;

{------------------------------------------------------------- }
{----- Shorthand type formats -------------------------------- }
type
  int8 = char;
  uint8 = byte; 
  int16 = smallint;
  uint16 = word; 
  int32 = longint;
  int64 = comp;
  uint32 = dword;
  uint64 = qword;
  uchar = byte;
  unichar = word;

  //Volatile types unsupported
  //--------------------------
  //typedef volatile signed char vint8;
  //typedef volatile unsigned char vuint8;
  //typedef volatile short vint16;
  //typedef volatile unsigned short vuint16;
  //typedef volatile long vint32;
  //typedef volatile unsigned long vuint32;
  //typedef volatile long long vint64;
  //typedef volatile unsigned long long vuint64;
  //typedef volatile long vlong;
  //typedef volatile int vint;
  //typedef volatile short vshort;
  //typedef volatile char vchar;
  //typedef volatile unsigned long vulong;
  //typedef volatile unsigned int vuint;
  //typedef volatile unsigned short vushort;
  //typedef volatile unsigned char vuchar;

{------------------------------------------------------------- }
{----- Descriptive formats ----------------------------------- }
type
  status_t = int32;
  bigtime_t = comp;
  type_code = uint32;
  perform_code = uint32;

{----------------------------------------------------- }
{----- min and max comparisons ----------------------- }
{----- min() and max() won't work in C++ ------------- }
{ was #define dname(params) para_def_expr }
{ argument types are unknown }
{ return type might be wrong }   

function min_c(a,b : longint) : longint;

{ was #define dname(params) para_def_expr }
{ argument types are unknown }
{ return type might be wrong }   

function max_c(a,b : longint) : longint;

{ was #define dname(params) para_def_expr }
{ argument types are unknown }
{ return type might be wrong }   

function min(a,b : longint) : longint;

{ was #define dname(params) para_def_expr }
{ argument types are unknown }
{ return type might be wrong }   

function max(a,b : longint) : longint;

{----------------------------------------------------- }
{----- Grandfathering -------------------------------- }
type
  bool = byte;

const
  bool_false = 0;
  bool_true = 1;

const
  NULL = 0;

{----LIMITS-------------------------------- }

const
  NAME_MAX = 256;
  SYMLINK_MAX = 16;
  MAXPATHLEN = 1024; //guess...

const
  B_FILE_NAME_LENGTH = NAME_MAX;
  B_PATH_NAME_LENGTH = MAXPATHLEN;
  B_ATTR_NAME_LENGTH = B_FILE_NAME_LENGTH - 1;
  B_MIME_TYPE_LENGTH = B_ATTR_NAME_LENGTH - 15;
  B_MAX_SYMLINKS = SYMLINK_MAX;
    
{----FILE OPEN MODES-------------------------------- }
  
const  
  B_READ_ONLY = O_RDONLY;    { read only  }
  B_WRITE_ONLY = O_WRONLY;   { write only  }
  B_READ_WRITE = O_RDWR;     { read and write  }
  B_FAIL_IF_EXISTS = O_EXCL; { exclusive create  }
  B_CREATE_FILE = O_CREAT;   { create the file  }
  B_ERASE_FILE = O_TRUNC;    { erase the file's data  }
  B_OPEN_AT_END = O_APPEND;  { point to the end of the data  }
       
{--------------------------------------------------------------- }
{----- Atomic functions; old value is returned ----------------- }

//function atomic_add(value:Pvint32; addvalue:int32):int32;cdecl; external LIBROOT;
//function atomic_and(value:Pvint32; andvalue:int32):int32;cdecl; external LIBROOT;
//function atomic_or(value:Pvint32; orvalue:int32):int32;cdecl; external LIBROOT;

{----- Other stuff --------------------------------------------- }
function get_stack_frame:pointer;cdecl; external LIBROOT;

{----- From SupportDef.h ----------------------------------------}
{ unlock function, NULL if lock failed }
{ error if "unlock_func" is NULL }
{ locked object if "unlock_func" is non-NULL }
type
  lock_status_t = record
    unlock_func : procedure (data:pointer);cdecl;
    value : record
      case longint of
        0 : ( error : status_t );
        1 : ( data : pointer );
    end;
  end;

const
  BEEP_SOUND = 'Beep';
  STARTUP_SOUND = 'Startup';
  NEWEMAIL_SOUND = 'New E-mail';
    
{----- From Beep.h ----------------------------------------------}
//because of the was libbe was compiled, FPC can't link to the exported
//functions from it!! Had to wrap these to get things to work.
function beep: status_t; cdecl; external LIBBEGUI name 'beapi_beep';
function system_beep(event_name:Pchar): status_t; cdecl; external LIBBEGUI name 'beapi_system_beep';
function add_system_beep_event(event_name:Pchar; flags:uint32):status_t; cdecl; external LIBBEGUI name 'beapi_add_system_beep_event';

//posix

type
   utsname = record
     sysname : array[0..31] of char;
     nodename : array[0..31] of char;
     release : array[0..31] of char;
     version : array[0..31] of char;
     machine : array[0..31] of char;
   end;

function uname(var name:utsname):longint; cdecl; external LIBROOT;



implementation

{ was #define dname(params) para_def_expr }
{ argument types are unknown }
{ return type might be wrong }   
function min_c(a,b : longint) : longint;
var
  if_local1 : longint;
  (* result types are not known *)
begin
  if b > a then
    if_local1:=b
  else if_local1:=a;

  min_c := if_local1;
end;

{ was #define dname(params) para_def_expr }
{ argument types are unknown }
{ return type might be wrong }   
function max_c(a,b : longint) : longint;
var
  if_local1 : longint;
  (* result types are not known *)
begin
  if b > a then
    if_local1:=a
  else if_local1:=b;
  
  max_c := if_local1;
end;

{ was #define dname(params) para_def_expr }
{ argument types are unknown }
{ return type might be wrong }   
function min(a,b : longint) : longint;
var
  if_local1 : longint;
  (* result types are not known *)
begin
  if b > a then
    if_local1:=b
  else if_local1:=a;

  min:=if_local1;
end;
   
function max(a,b : longint) : longint;
var
  if_local1 : longint;
  (* result types are not known *)
begin
  if b > a then
    if_local1:=a
  else if_local1:=b;
  
  max:=if_local1;
end;

end.