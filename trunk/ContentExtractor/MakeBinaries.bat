@echo off
set TargetDir=_Binaries/%2
set OutputDir=%1

rd /s/q "%TargetDir%"
md "%TargetDir%"

copy %OutputDir%"\*.*" "%TargetDir%\"
del "%TargetDir%\*.vshost.exe"
del "%TargetDir%\*.xml"
del "%TargetDir%\*.pdb"
