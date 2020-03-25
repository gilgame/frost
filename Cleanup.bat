@echo off

for /D /R %%i in (*) do (
    set "del="
    if "%%~ni"=="bin" set del=1
    if "%%~ni"=="obj" set del=1
    if defined del (
        echo "Removing %%i"
        rmdir /S /Q %%i
    )
)
