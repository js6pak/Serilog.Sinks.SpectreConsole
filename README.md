# Serilog.Sinks.SpectreConsole
[![NuGet Version](http://img.shields.io/nuget/v/js6pak.Serilog.Sinks.SpectreConsole.svg?style=flat)](https://www.nuget.org/packages/js6pak.Serilog.Sinks.SpectreConsole/) 

A yet another Spectre.Console sink for Serilog but this time its a 1/1 port of Serilog.Sinks.Console so you don't lose out on any features.

### Getting started

To use the console sink, first install the [NuGet package](https://nuget.org/packages/js6pak.Serilog.Sinks.SpectreConsole):

```shell
dotnet add package js6pak.Serilog.Sinks.SpectreConsole
```

Then enable the sink using `WriteTo.SpectreConsole()`:

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.SpectreConsole()
    .CreateLogger();
    
Log.Information("Hello, world!");
```

Log events will be printed to `STDOUT`:

```
[12:50:51 INF] Hello, world!
```

![log output screenshot](https://github.com/js6pak/Serilog.Sinks.SpectreConsole/assets/35262707/38d248f6-13a2-42c5-ada0-ccb67f6e1ff3)
