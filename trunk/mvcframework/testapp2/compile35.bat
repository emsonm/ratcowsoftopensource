@echo off
echo CAUTION: this will delete the file already created
pause

echo building file
..\MvcTool\bin\debug\mvctool -p -v Form1

echo cleaning up
del temp.dll

echo DONE!
pause