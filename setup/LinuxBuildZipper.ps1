$parentFolder = Split-Path -parent $PSScriptRoot
$folder = "$parentFolder\src\Randomizer.CrossPlatform\bin\Release\net7.0\linux-x64\publish"
if (-not (Test-Path $folder))
{
    $folder = "$parentFolder\src\Randomizer.CrossPlatform\bin\Release\net7.0\publish\linux-x64"
}
$version = "0.0.0"
if (Test-Path "$folder\Randomizer.CrossPlatform.dll") {
    $version = (Get-Item "$folder\Randomizer.CrossPlatform.dll").VersionInfo.ProductVersion
}
else {
    $version = (Get-Item "$folder\SMZ3CasRandomizer.dll").VersionInfo.ProductVersion
}

if (Test-Path -LiteralPath "$folder\Sprites") {
    Remove-Item -LiteralPath "$folder\Sprites" -Recurse
}

Copy-Item "$parentFolder\src\Randomizer.Sprites\" -Destination "$folder\Sprites" -Recurse
Remove-Item "$folder\Sprites\bin" -Recurse
Remove-Item "$folder\Sprites\obj" -Recurse
Get-ChildItem -Exclude *.png,*.rdc,*.ips,*.gif -Recurse -Path "$folder\Sprites" | Where-Object { !$_.PSisContainer } | ForEach-Object {
    Remove-Item -Path $_.FullName -Recurse
}

Get-ChildItem -Filter "Randomizer.CrossPlatform*" -Path "$folder" | ForEach-Object {
    $newFileName = $_.Name -replace "Randomizer.CrossPlatform", "SMZ3CasRandomizer"
    Copy-Item -Path $_.FullName -Destination "$folder\$newFileName"
}

$fullVersion = "SMZ3CasRandomizerLinux_$version"
$outputFile = "$PSScriptRoot\Output\$fullVersion.tar.gz"
if (Test-Path $outputFile) {
    Remove-Item $outputFile -Force
}
if (-not (Test-Path $outputFile)) {
    Set-Location $folder
    tar -cvzf $outputFile *
}
Set-Location $PSScriptRoot