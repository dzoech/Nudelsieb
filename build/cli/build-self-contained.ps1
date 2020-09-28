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

rmdir $PSScriptRoot/chocolatey/tools/win-x64/* -Force -Recurse -ErrorAction SilentlyContinue
dotnet publish --configuration Release --runtime win-x64 --output $PSScriptRoot/chocolatey/tools/win-x64 --self-contained true -p:Version=$Version -p:PublishReadyToRun=true -p:PublishReadyToRunShowWarnings=true  -p:PublishTrimmed=true -p:DebugType=None --verbosity minimal $PSScriptRoot/../../src/Nudelsieb/Nudelsieb.Cli/Nudelsieb.Cli.csproj

choco pack --version $Version --outputdirectory $OutputPath $PSScriptRoot/chocolatey/nudelsieb-cli.nuspec

choco uninstall nudelsieb-cli | Out-Null

mkdir -f $OutputPath # -f to suppress error when already existing

if ($Verbose) {
    # using -dv for debug and verbose output
    choco install -s -dv $OutputPath nudelsieb-cli 
} else {
    choco install -s $OutputPath nudelsieb-cli
}

if ($Publish) {
    # only for publishing from local dev environment
    choco push "$OutputPath/nudelsieb-cli.$($Version).nupkg" --source https://push.chocolatey.org
}