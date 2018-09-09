@echo off

echo ----Do not close this window!----

start Watchdog.bat
Build.exe -batchmode -nographics -ip 127.0.0.1 -port 7777 -server -map.size 2500 2500 -logfile log.txt 

echo Closing Server