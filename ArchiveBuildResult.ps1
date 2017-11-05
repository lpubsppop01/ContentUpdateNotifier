$configulationName = "Release"
$platformName = "Any CPU"
if ($args.Length -gt 0) {
    $configulationName = $args[0]
    if ($configulationName -ine "Publish") {
        return
    }
    if ($args.Length -gt 1) {
        $platformName = $args[1]
    }
}

$solutionDirPath = Split-Path $MyInvocation.MyCommand.Path -Parent
$workDirName = "lpubsppop01.ContentUpdateNotifier_" + $configulationName + "_" + ($platformName -replace " ", "_")
$workDirPath = Join-Path $solutionDirPath $workDirName
$workBinDirPath = Join-Path $workDirPath "bin"

if (!(Test-Path -LiteralPath $workBinDirPath)) {
    New-Item -ItemType Directory $workBinDirPath
}

Copy-Item (Join-Path $solutionDirPath "README.md") $workDirPath
Copy-Item (Join-Path $solutionDirPath "LICENSE.txt") $workDirPath

$srcBinDirPath = Join-Path $solutionDirPath "ContentUpdateNotifier\bin\$configulationName"
Copy-Item (Join-Path $srcBinDirPath "lpubsppop01.ContentUpdateNotifier.exe") $workBinDirPath
Copy-Item (Join-Path $srcBinDirPath "lpubsppop01.ContentUpdateNotifier.exe.config") $workBinDirPath
Copy-Item (Join-Path $srcBinDirPath "Microsoft.WindowsAPICodePack.dll") $workBinDirPath
Copy-Item (Join-Path $srcBinDirPath "Microsoft.WindowsAPICodePack.Shell.dll") $workBinDirPath
Copy-Item (Join-Path $srcBinDirPath "Microsoft.WindowsAPICodePack.ShellExtensions.dll") $workBinDirPath
Copy-Item (Join-Path $srcBinDirPath "System.Data.SQLite.dll") $workBinDirPath
Copy-Item (Join-Path $srcBinDirPath "System.Data.SQLite.dll.config") $workBinDirPath
Copy-Item -Recurse (Join-Path $srcBinDirPath "x86") $workBinDirPath
Copy-Item -Recurse (Join-Path $srcBinDirPath "x64") $workBinDirPath
Copy-Item -Recurse (Join-Path $srcBinDirPath "_Images") $workBinDirPath

$archiveFilename = $workDirName + ".zip"
$archiveFilePath = Join-Path $solutionDirPath $archiveFilename
if (Test-Path -LiteralPath $archiveFilePath) {
    Remove-Item $archiveFilePath
}
7z a $archiveFilePath $workDirPath\*

Remove-Item -Recurse $workDirPath
