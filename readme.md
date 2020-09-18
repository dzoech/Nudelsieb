[![Build Status](https://dev.azure.com/zoechbauer/Nudelsieb/_apis/build/status/dzoech.Nudelsieb?branchName=master)](https://dev.azure.com/zoechbauer/Nudelsieb/_build/latest?definitionId=2&branchName=master)

*Nudelsieb* is a service that enables one to quickly write down thoughts and ideas before having forgotten them already.

*Nudelsieb* is work in progress.

# Installation

You can install the CLI via the Windows package manager [Chocolatey](https://chocolatey.org/packages/nudelsieb-cli):

`choco install nudelsieb-cli`

# Usage


After the installation with Chocolatey you can use the CLI with the these aliases: `nudelsieb` or the short version `nds`.

A random thought that occurred to you is called a *neuron*. To add a new neuron, simply run

```
nudelsieb add "My random thought I want to remember"
```

If you want to order your thought into *groups*, run 
```
nudelsieb add "My random thoughts I want to remember" --group "work" --group "support"
```

To retrieve or remember your thoughts, run
```
nudelsieb get --group "work"
```

# Architecture

The following architecture sketch illustrates the big picture:
![Architecture sketch from first brainstorming session](misc/brainstorming/brainstormin-v1.jpeg "Architecture sketch")

See the Swagger specification of our REST API at https://nudelsieb.zoechbauer.dev/swagger.

# Setup instructions for local development
Install [Azure Cosmos Emulator](https://aka.ms/cosmosdb-emulator) (currently, no Docker image for Linux is available). Per default the emulator is listening on localhost:8081.

# Todos
- [X] Setup deployment pipeline for WebApi service
  - [ ] Separate build and deployment/release into separate pipelines
  - [ ] Automate releasing chocolatey package
- [ ] Allow to switch between different endpoints (cloud, localhost) in the CLI (`nudelsieb config endpoints switch`)
- [ ] Compare cli version with REST API version
  - [ ] Set assembly and package version in build pipeline
  - [ ] Warn when mismatching
- [X] Query neurons by group name
- [ ] Cli: Support #myGroup in message text to be converted into groups without using --group 
- [ ] Create some seed demo data
- [ ] Implement registration process from the CLI 
  - [ ] utilizing Azure AD
  - [ ] utilizing GitHub as social provider for Azure AD
- [ ] Cli: implement command to get upcoming reminders
- [ ] Cli: implement config resolver service that gets required values on demand (instead of loading them all on application start)