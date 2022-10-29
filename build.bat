echo off

Rem Release the entire solution
dotnet publish -c Release

Rem Clear target folder
rmdir "%cd%\bin" /s /q
mkdir "%cd%\bin"

Rem Build all the projects
CALL :CreateZip Informant %~1

%SystemRoot%\explorer.exe "%cd%\bin"
EXIT /B %ERRORLEVEL%


:CreateZip
set projectName=%~1

Rem Create a folder that contains everything that should be in the ZIP
set projectFolder=%cd%\%projectName%
set zipFolder=%projectFolder%\bin\ReleaseZip
set outputFolder=%zipFolder%\%projectName%
if exist "%zipFolder%" rmdir /s /q "%zipFolder%"
if not exist "%outputFolder%" mkdir "%outputFolder%"

xcopy /y %projectFolder%\bin\Release\net5.0\publish\%projectName%.dll %outputFolder%
xcopy /y %projectFolder%\bin\Release\net5.0\publish\%projectName%.pdb %outputFolder%
xcopy /y %projectFolder%\manifest.json %outputFolder%
xcopy /y %projectFolder%\assets\ %outputFolder%\assets\
xcopy /y %projectFolder%\i18n\ %outputFolder%\i18n\
xcopy /y %cd%\LICENSE %outputFolder%

Rem Make a HTML file out of the README
Rem If this stops working, maybe run "choco install pandoc" on the PowerShell again?
pandoc %cd%\README.md -t html -o %outputFolder%\Readme.html -s --metadata title="Slothsoft Informant"

Rem Replace the image URLs of the HTML
powershell -Command "((gc %outputFolder%\Readme.html -encoding utf8) -replace 'readme/dev', 'https://github.com/slothsoft/stardew-informant/blob/main/readme/dev' -replace 'readme/screen', 'https://github.com/slothsoft/stardew-informant/raw/main/readme/screen') | Out-File -encoding utf8 %outputFolder%\Readme.html"

Rem Now zip the entire folder
"C:\Program Files\7-Zip\7z.exe" a %cd%\bin\%projectName%-%2.zip %zipFolder%/*
EXIT /B 0