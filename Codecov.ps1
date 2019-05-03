[string]$input = $args[0]

$codecov = (Resolve-Path "$env:USERPROFILE\.nuget\packages\codecov\*\tools\codecov.exe").ToString()

$codecov -f $input -t $env:CODECOV_TOKEN
