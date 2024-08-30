using DrawingProgram;
using System.Diagnostics;

public static class UpdateInstaller
{
    private static string updaterBatContent = @"@echo off
SETLOCAL

SET sourceDir=update\publish
SET targetDir=%~dp0

SLEEP 5

xcopy ""%sourceDir%\*"" ""%targetDir%"" /E /Y /C /H /R

IF ERRORLEVEL 0 (
    rmdir /S /Q ""%~dp0update""
    ""%~dp0FinalProjTwo.exe""
    del ""%~f0""
) ELSE (
    echo Error occurred during the copy process.
    pause
)";

    public static void Update()
    {
        string batFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Updater.bat");

        File.WriteAllText(batFilePath, updaterBatContent);

        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = batFilePath,
        };

        Process.Start(processStartInfo);

        Environment.Exit(0);
    }
}