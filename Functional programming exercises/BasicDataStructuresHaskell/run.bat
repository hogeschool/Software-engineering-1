del "data.exe"
cls
cd %~dp0%
ghc -o data script_language.hs data.hs
data