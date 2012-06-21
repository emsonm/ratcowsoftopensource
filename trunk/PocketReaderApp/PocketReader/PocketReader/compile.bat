@echo off
echo CAUTION: this will delete the file already created
rem pause

echo building file
.\tools\mvctool -R -p -v MainForm
move /Y *controller.cs .\controllers

echo cleaning up
del *.dll

echo DONE!
rem pause