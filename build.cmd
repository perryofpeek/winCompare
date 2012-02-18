echo "Compile" 
C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe .\WinCompare.sln /t:Rebuild /p:Configuration=Release

echo "run unit tests"
.\tools\nunit\nunit-console.exe .\UnitTests\bin\Release\UnitTests.dll

echo "copy to output folder"
copy .\WinCompare\bin\Release\winCompare.exe .\output