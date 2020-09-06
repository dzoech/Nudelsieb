Param(
    [Parameter(Mandatory=$true)]
    [String]
    $Version
)

Write-Host "Building version $Version"

dotnet publish --configuration Release --runtime win-x64 --output ./chocolatey/tools/win-x64 --self-contained true -p:PublishTrimmed=true ./../../src/Nudelsieb/Nudelsieb.Cli/Nudelsieb.Cli.csproj

choco pack --version $Version .\chocolatey\nudelsieb-cli.nuspec

choco uninstall nudelsieb-cli

choco install -dv -s . nudelsieb-cli

# choco push "nudelsieb-cli.$($Version).nupkg" --source https://push.chocolatey.org/