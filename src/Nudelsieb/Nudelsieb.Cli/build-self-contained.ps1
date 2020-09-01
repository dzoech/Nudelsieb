$version = "0.0.1" # todo: use input parameter

dotnet build -c Release -r win-x64 -o ./chocolatey/tools/win-x64

choco pack --version $version .\chocolatey\nudelsieb-cli.nuspec

choco push "nudelsieb-cli.$($version).nupkg" --source https://push.chocolatey.org/