@echo off

echo ----Do not close this window!----

start powershell Get-Content log.txt -Wait

:loop
tasklist /FI "IMAGENAME eq powershell.exe" /NH | find /I /N "powershell.exe" >NUL

if NOT "%ERRORLEVEL%"=="0" (
	echo Closing server
  	
  	taskkill /IM Build.exe
  	goto stop
)
goto loop

:stop
pause
exit