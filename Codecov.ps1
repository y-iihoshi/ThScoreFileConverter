[string]$inputFile = $args[0]

$codecov = (Resolve-Path "$env:USERPROFILE\.nuget\packages\codecov\*\tools\codecov.exe").ToString()

& $codecov -f $inputFile -t $env:CODECOV_TOKEN
