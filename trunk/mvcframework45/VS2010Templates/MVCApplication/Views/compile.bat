@echo off

echo building file

for %%f in (*.Designer.cs) do call :processfile %%f

echo cleaning up
del *.dll

echo DONE!

goto:eof

:processfile

 rem get the filename to process
 set file=%1

 rem get the file substring - the second one removes a spurious trailing space..
 set file=%file:~0,-12% 
 set file=%file:~0,-1%

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
 ..\tools\mvctool -R -p -v -D %file%

 if not exist %controllerPath% copy %localControllerPath% ..\Controllers
 copy %localControllerDesignerPath% ..\Controllers
 del %localPath%
goto:eof





