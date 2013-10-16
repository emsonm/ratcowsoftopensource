@echo off

rem This handles compilation of a single file. Makes life easier.

echo building file

call :processfile %1

echo cleaning up
del *.dll
del *.resources

echo DONE!

goto:eof

:processfile

 rem get the filename to process
 set file=%1

 @echo processing %file%
 
 rem next we build some paths
 set controllerPath=..\Controllers\%file%Controller.cs
 @echo %controllerPath%
 set controllerDesignerPath=..\Controllers\%file%Controller.Designer.cs
 @echo %controllerDesignerPath%
 set localPath=.\%file%Controller*.cs
 @echo %localPath%
 set localControllerPath=.\%file%Controller.cs
 @echo %localControllerPath%
 set localControllerDesignerPath=.\%file%Controller.Designer.cs
 @echo %localControllerDesignerPath%

 rem compile the files
 ..\tools\mvctool -R -p -v -D %file% -i="..\Libraries\RatCow.MvcFramework.Mapping.dll,..\Libraries\RatCow.MvcFramework.dll" 

 if not exist %controllerPath% copy %localControllerPath% ..\Controllers
 copy %localControllerDesignerPath% ..\Controllers
 del %localPath%

goto:eof