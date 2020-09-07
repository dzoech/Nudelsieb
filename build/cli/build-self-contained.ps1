Param(
    [Parameter(Mandatory=$true)]
    [String]
    $Version,
    [Parameter()]
    [String]
    $OutputPath="./publish",
    [Parameter()]
    [Bool]
    $Publish=$false
)

Write-Host "Building version $Version"

rmdir ./chocolatey/tools/win-x64/* | Out-Null
dotnet publish --configuration Release --runtime win-x64 --output ./chocolatey/tools/win-x64 --self-contained true -p:AssemblyVersion=$Version -p:PublishReadyToRun=true -p:PublishTrimmed=true -p:DebugType=None --verbosity minimal ./../../src/Nudelsieb/Nudelsieb.Cli/Nudelsieb.Cli.csproj

choco pack --version $Version --outputdirectory $OutputPath ./chocolatey/nudelsieb-cli.nuspec

choco uninstall nudelsieb-cli | Out-Null
choco install -s $OutputPath nudelsieb-cli # use -dv for debug and verbose output

if ($Publish) {
    mkdir -f $OutputPath # -f to suppress error when already existing
    choco push "./$OutputPath/nudelsieb-cli.$($Version).nupkg" --source https://push.chocolatey.org
}