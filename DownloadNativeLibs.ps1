$ArchiveName = "alpha-2.zip"
$BaseUrl = "https://s3.eu-west-2.amazonaws.com/safe-app-csharp/"

Function Cleanup () {
    Remove-Item $Archive -Force -Recurse -ErrorAction SilentlyContinue
    Remove-Item $UnpackedDir -Force -Recurse -ErrorAction SilentlyContinue
    Exit
}

# Download and unpack the archive to %Temp%
$Archive = "$env:Temp\$ArchiveName"
Write-Host "Downloading $BaseUrl$ArchiveName"
(New-Object System.Net.WebClient).DownloadFile("$BaseUrl$ArchiveName", "$Archive")
$UnpackedDir = "$env:Temp\" + [io.path]::GetFileNameWithoutExtension($Archive)
Remove-Item $UnpackedDir -Force -Recurse -ErrorAction SilentlyContinue
Expand-Archive $Archive -DestinationPath $UnpackedDir

# Check if either of the target folders exist, and if so ask the user for permission to delete them
$AndroidFolder = "$PSScriptRoot\SafeApp.AppBindings.Android"
$IosFolder = "$PSScriptRoot\SafeApp.AppBindings.iOS"
If ((Test-Path $AndroidFolder\lib -pathType container) -Or (Test-Path $IosFolder\lib -pathType container)) {
    $Yes = New-Object System.Management.Automation.Host.ChoiceDescription "&Yes", `
        "Deletes '$AndroidFolder\lib' and '$IosFolder\lib'."
    $No = New-Object System.Management.Automation.Host.ChoiceDescription "&No", `
        "Aborts the script."
    $Options = [System.Management.Automation.Host.ChoiceDescription[]]($Yes, $No)
    $Result = $host.ui.PromptForChoice("At least one target folder already exists.",
        "OK to delete it/them?", $Options, 1)
    Switch ($Result) {
        0 {
            Remove-Item $AndroidFolder\lib -Force -Recurse -ErrorAction SilentlyContinue
            Remove-Item $IosFolder\lib -Force -Recurse -ErrorAction SilentlyContinue
        }
        1 {
            Write-Host "Aborting."
            Cleanup
        }
    }
}

# Create the target folders and move the unpacked contents to them
New-Item -ItemType Directory -Force -Path $AndroidFolder | Out-Null
New-Item -ItemType Directory -Force -Path $IosFolder | Out-Null
Move-Item $UnpackedDir\android\lib $AndroidFolder
Move-Item $UnpackedDir\ios\lib $IosFolder
Cleanup
