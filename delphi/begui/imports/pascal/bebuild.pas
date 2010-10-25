unit BeBuild;
interface

{$PACKRECORDS C}

  {                                                                             
  /
  /	File:			BeBuild.h
  /
  /	Description:	Import/export macros
  /
  /	Copyright 1993-98, Be Incorporated
  /
                                                                                 }
//{$include <ProductFeatures.h>}

const
  B_BEOS_VERSION_4 = $0400;
  B_BEOS_VERSION_4_5 = $0450;
  B_BEOS_VERSION_5 = $0500;
  B_BEOS_VERSION_5_0_3 = $0503;
  B_BEOS_VERSION_5_0_4 = $0504;
  B_BEOS_VERSION_DANO = $0510;
  B_BEOS_VERSION_MAUI = B_BEOS_VERSION_5;
  B_BEOS_VERSION = B_BEOS_VERSION_5_0_3;

  { Originally, it wasn't possible to unset _R5_COMPATIBLE_, so make the
     default behaviour the same.  }

{$ifdef powerc}
const
  _PR2_COMPATIBLE_ = 1;
  _PR3_COMPATIBLE_ = 1;
  _R4_COMPATIBLE_ = 1;
  _R4_5_COMPATIBLE_ = 1;
  _R5_COMPATIBLE_ = 1;
  _R5_0_4_COMPATIBLE_ = 1;
{$else}
const
  _PR2_COMPATIBLE_ = 0;
  _PR3_COMPATIBLE_ = 0;
  _R4_COMPATIBLE_ = 1;
  _R4_5_COMPATIBLE_ = 1;
  _R5_COMPATIBLE_ = 1;
  _R5_0_4_COMPATIBLE_ = 1;
{$endif}

{ was #define dname(params) para_def_expr }
{ argument types are unknown }
{ return type might be wrong }   
function _UNUSED(x : longint) : longint;

{ was #define dname def_expr }
{ return type might be wrong }
function _PACKED : longint;

implementation

{ was #define dname(params) para_def_expr }
{ argument types are unknown }
{ return type might be wrong }   
function _UNUSED(x : longint) : longint;
begin
  _UNUSED:=x;
end;

{ was #define dname def_expr }
{ return type might be wrong }
function _PACKED : longint;
begin
  _PACKED:=__attribute__(packed);
end;

end.
