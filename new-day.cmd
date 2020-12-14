@echo off
if "%1x" == "x" goto Error
mkdir day%1
copy day-template.cs day%1\day%1.cs
sed -i 's/Day00/Day%1/g' day%1/day%1.cs
touch day%1/day%1-input-test.txt
goto Done

:Error
echo Error - Day number not supplied
echo Usage eg: new-day 12
goto End

:Done
echo Done

:End
