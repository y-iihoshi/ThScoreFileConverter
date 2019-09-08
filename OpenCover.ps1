[string]$output = $args[0]

$opencover = (Resolve-Path "$env:USERPROFILE\.nuget\packages\opencover\*\tools\OpenCover.Console.exe").ToString()
$target = "$env:VSINSTALLDIR\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
$targetArgs = (Resolve-Path -Relative `
    ".\ThScoreFileConverterTests\bin\$env:CONFIGURATION\*\ThScoreFileConverterTests.dll").ToString()

$filter = "+[*]* -[ThScoreFileConverterTests]*"

if ($env:APPVEYOR -ieq "true")
{
    $targetArgs = "/logger:Appveyor $targetArgs"
}

& $opencover `
    -register:Path32 `
    -target:$target `
    -targetargs:$targetArgs `
    -filter:$filter `
    -output:$output
