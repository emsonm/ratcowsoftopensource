@echo off
echo CAUTION: this will delete the file already created
pause

echo building file
..\MvcTool\bin\debug\mvctool Form1

echo cleaning up
del *.dll

echo DONE!
pause