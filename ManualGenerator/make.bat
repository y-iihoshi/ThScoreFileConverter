@ECHO OFF

pushd %~dp0

REM Command file for Sphinx documentation

if "%SPHINXBUILD%" == "" (
	set SPHINXBUILD=sphinx-build
)
set SOURCEDIR=.
set BUILDDIR=_build

%SPHINXBUILD% >NUL 2>NUL
if errorlevel 9009 (
	echo.
	echo.The 'sphinx-build' command was not found. Make sure you have Sphinx
	echo.installed, then set the SPHINXBUILD environment variable to point
	echo.to the full path of the 'sphinx-build' executable. Alternatively you
	echo.may add the Sphinx directory to PATH.
	echo.
	echo.If you don't have Sphinx installed, grab it from
	echo.http://sphinx-doc.org/
	exit /b 1
)

set SPHINXINTL=sphinx-intl
set SPHINXINTL_LANGUAGE=en,ja

set BUILDER=%1
if "%BUILDER%" == "" goto help
if "%BUILDER%" == "catalog" goto catalog
if "%BUILDER:~-6%" == "-langs" goto multilangs

%SPHINXBUILD% -M %BUILDER% %SOURCEDIR% %BUILDDIR% %SPHINXOPTS% %O%
goto end

:help
%SPHINXBUILD% -M help %SOURCEDIR% %BUILDDIR% %SPHINXOPTS% %O%
goto end

:catalog
%SPHINXINTL% update -p %BUILDDIR%\gettext -w 0
goto end

:multilangs
for %%l in (%SPHINXINTL_LANGUAGE%) do (
	%SPHINXBUILD% -M %BUILDER:~0,-6% %SOURCEDIR% %BUILDDIR%\%%l %SPHINXOPTS% %O% -Dlanguage=%%l
)

:end
popd
