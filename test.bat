@echo off

SET dotnet="%ProgramFiles%\dotnet\dotnet.exe"

echo Running test ...
%dotnet% test "Combo.Core.Tests"

pause