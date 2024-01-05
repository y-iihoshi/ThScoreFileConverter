#Requires -Version 7.2

param(
    [Parameter(Mandatory)]
    [String] $Configuration,
    [Parameter(Mandatory)]
    [String] $TargetFramework,
    [String] $OutputDir
)

if ([string]::IsNullOrEmpty($OutputDir)) {
    $OutputDir = Join-Path 'publish' $Configuration $TargetFramework
}

$Arguments = @{
    Path = Join-Path 'ThScoreFileConverter\bin' $Configuration $TargetFramework
    Destination = $OutputDir
    Recurse = $True
}
Copy-Item @Arguments

foreach ($Lang in ('en', 'ja')) {
    $Arguments = @{
        Path = Join-Path 'ManualGenerator\_build' $Lang 'html'
        Destination = Join-Path $OutputDir 'doc' $Lang
        Recurse = $True
    }
    Copy-Item @Arguments
}

$Arguments = @{
    Path = 'TemplateGenerator\Templates'
    Destination = Join-Path $OutputDir 'template'
    Exclude = '*.tt*'
    Recurse = $True
}
Copy-Item @Arguments

$Arguments = @{
    Path = 'TemplateGenerator\output\*'
    Destination = Join-Path $OutputDir 'template'
    Recurse = $True
}
if (Test-Path $Arguments.Path) {
    Copy-Item @Arguments
}
