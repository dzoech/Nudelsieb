[![Build Status](https://dev.azure.com/zoechbauer/Nudelsieb/_apis/build/status/dzoech.Nudelsieb?branchName=master)](https://dev.azure.com/zoechbauer/Nudelsieb/_build/latest?definitionId=2&branchName=master)

*Nudelsieb* is a service that enables one to quickly write down thoughts and ideas before having 
forgotten them already. It focuses on supporting developers managing their tasks by providing a 
simple command line interface to add new records called *Neurons*. Neurons can be organized into 
groups, and allow to schedule reminders. 
Reminders are pushed as notifications to the mobile 
app. The main goal of the mobile app to facilitate easy rescheduling of reminders. Currently, only
Android is supported.

*Nudelsieb* is work in progress.

# Installation

[![Chocolatey Version](https://img.shields.io/chocolatey/v/nudelsieb-cli)](https://chocolatey.org/packages/nudelsieb-cli)
[![Chocolatey](https://img.shields.io/chocolatey/dt/nudelsieb-cli)](https://chocolatey.org/packages/nudelsieb-cli)

You can install the CLI via the Windows package manager [Chocolatey](https://chocolatey.org/packages/nudelsieb-cli):

`choco install nudelsieb-cli`

# Usage

After the installation with Chocolatey you can use the CLI with the these aliases: `nudelsieb` or the short version `nds`.

```
Usage: nudelsieb [command] [options]

Options:
  -v|--version  Show version information.
  -?|-h|--help  Show help information.

Commands:
  add
  config
  list
  login
  reminder

Run 'nudelsieb [command] -?|-h|--help' for more information about a command.
```

## Getting started

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

# Setup instructions for Azure environments
Azure Service Bus requires a connection string with *Listen* and *Send* permissions, and a queue named `reminder-push-notifications`.
