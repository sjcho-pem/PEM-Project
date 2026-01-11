set YEAR=%date:~2,2%
set MONTH=%date:~5,2%
set DAY=%date:~8,2%
set USERPATH=%USERPROFILE%

set Date = %YEAR%%MONTH%%DAY%
set Release_Path=..\H_MarsSSD\bin\x64\Release
set Update_Path=Update_H_MarsSSD_%Date%\Program_H_MarsSSD

set TARGET='C:\H_MarsSSD\H_MarsSSD_%Date%\H_MarsSSD.exe'
set SHORTCUT='Update_H_MarsSSD_%Date%\H_MarsSSD.lnk'
set PWS=powershell.exe -ExecutionPolicy Bypass -NoLogo -NonInteractive -NoProfile

md %Update_Path%
xcopy %Release_Path% %Update_Path%  /e /y

echo xcopy Program_H_MarsSSD C:\H_MarsSSD\H_MarsSSD_%Date%\  /e /y>Update_H_MarsSSD_%Date%\Update.bat
echo xcopy H_MarsSSD.lnk %%USERPROFILE%%\Desktop /y>>Update_H_MarsSSD_%Date%\Update.bat
%PWS% -Command "$ws = New-Object -ComObject WScript.Shell; $s = $ws.CreateShortcut(%SHORTCUT%); $S.TargetPath = %TARGET%; $S.Save()"