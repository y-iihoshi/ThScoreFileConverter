set CONFIGURATION=%1
set OUTPUT=%2

set OPENCOVER=.\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe
set TARGET=%VSINSTALLDIR%\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe
set TARGET_ARGS=.\ThScoreFileConverterTests\bin\%CONFIGURATION%\ThScoreFileConverterTests.dll
set FILTER=+[*]* -[ThScoreFileConverterTests]*

if /i "%APPVEYOR%" == "true" (
    set TARGET_ARGS=/logger:Appveyor %TARGET_ARGS%
)

%OPENCOVER% ^
    -register:user ^
    -target:"%TARGET%" ^
    -targetargs:"%TARGET_ARGS%" ^
    -filter:"%FILTER%" ^
    -output:"%OUTPUT%"
