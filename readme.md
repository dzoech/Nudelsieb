[![Build Status](https://zoechbauer.visualstudio.com/Nudelsieb/_apis/build/status/dzoech.Nudelsieb?branchName=master)](https://zoechbauer.visualstudio.com/Nudelsieb/_build/latest?definitionId=2&branchName=master)

https://zoechbauer.visualstudio.com/Nudelsieb/_apis/build/status/dzoech.Nudelsieb?branchName=master

*Nudelsieb* is a service that enables one to quickly write down thoughts and ideas before having forgotten them already.

*Nudelsieb* is work in progress.

The following architecture sketch illustrates the big picture:
![Architecture sketch from first brainstorming session](misc/brainstorming/brainstormin-v1.jpeg "Architecture sketch")

# Setup instructions for local development
Install [Azure Cosmos Emulator](https://aka.ms/cosmosdb-emulator) (currently, no Docker image for Linux is available). Per default the emulator is listening on localhost:8081.

# Todos
- [ ] Query neurons by group name
- [ ] Cli: Support #myGroup in message text to be converted into groups without using --group 
- [ ] Create some seed demo data
- [ ] Setup deployment pipeline for WebApi service
- [ ] Allow to switch between different endpoints (cloud, localhost) in the CLI
- [ ] Implement registration process from the CLI 
  - [ ] utilizing Azure AD
  - [ ] utilizing GitHub 
- [ ] Cli: implement command to get upcoming reminders
