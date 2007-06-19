PSetup.exe /uionlyifneeded;
if %ERRORLEVEL% == 0 msiexec /i ContentExtractor.msi;