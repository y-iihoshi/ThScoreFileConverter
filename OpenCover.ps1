[string]$configuration = $args[0]
[string]$output = $args[1]

$opencover = (Resolve-Path ".\packages\OpenCover.*\tools\OpenCover.Console.exe").ToString()
$target = "$env:VSINSTALLDIR\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
$targetArgs = ".\ThScoreFileConverterTests\bin\$configuration\ThScoreFileConverterTests.dll"
$filter = "+[*]* -[ThScoreFileConverterTests]*"

if ($env:APPVEYOR -ieq "true")
{
    $targetArgs = "/logger:Appveyor $targetArgs"
}

& $opencover `
    -register:user `
    -target:$target `
    -targetargs:$targetArgs `
    -filter:$filter `
    -output:$output
