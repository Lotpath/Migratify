@echo off

SET TARGET="All"
IF NOT [%1]==[] (set TARGET="%1")
SET BUILDMODE="Release"
IF NOT [%2]==[] (set BUILDMODE="%2")

SET PWRSHELL=PowerShell.exe -NoLogo -NonInteractive -NoProfile -ExecutionPolicy RemoteSigned
SET ThisDirectory=%~dp0
SET BuildScriptPath=%ThisDirectory%build.ps1

%PWRSHELL% -Command "& %BuildScriptPath% -Target %TARGET% -configuration %BUILDMODE%"