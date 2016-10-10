@echo off

SET dotnet="%ProgramFiles%\dotnet\dotnet.exe"
SET opencover=%USERPROFILE%\.nuget\packages\OpenCover\4.6.519\tools\OpenCover.Console.exe
SET reportgenerator=%USERPROFILE%\.nuget\packages\ReportGenerator\2.4.5\tools\ReportGenerator.exe

SET targetargs="test"
SET filter="+[*]Combo.* -[*.Tests]* -[xunit.*]*"  
SET coveragefile=%cd%\coverage.xml
SET coveragedir=%cd%\Coverage

echo Test with coverage ...

REM Run code coverage analysis  
%opencover% -oldstyle -register:user -target:%dotnet% -targetargs:"test Combo.Core.Tests" -output:%coveragefile% -filter:%filter% -skipautoprops -hideskipped:All

echo Generating report ...
REM Generate the report 
%reportgenerator% "-targetdir:%coveragedir%" "-reporttypes:Html;Badges" "-reports:%coveragefile%" -verbosity:Error

REM Open the report  
start "report" "%coveragedir%\index.htm"

pause