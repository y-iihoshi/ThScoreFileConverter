@echo off

set configuration=%~1
if "%configuration%" == "" (
    exit /b 1
)

set target_framework=%~2
if "%target_framework%" == "" (
    exit /b 1
)

xcopy ThScoreFileConverter\bin\%configuration%\%target_framework% publish\%target_framework% /e /i /q

for %%l in (en ja) do (
    xcopy ManualGenerator\_build\%%l\html publish\%target_framework%\doc\%%l /e /i /q
)

xcopy TemplateGenerator\Templates publish\%target_framework%\template /e /i /q

del publish\%target_framework%\template\*.tt* /q
