# ForestLog

![ForestLog](Images/ForestLog.100.png)

ForestLog - A minimalist logger interface.

[![Project Status: WIP – Initial development is in progress, but there has not yet been a stable, usable release suitable for the public.](https://www.repostatus.org/badges/latest/wip.svg)](https://www.repostatus.org/#wip)

## NuGet

| Package  | NuGet                                                                                                                |
|:---------|:---------------------------------------------------------------------------------------------------------------------|
| ForestLog | [![NuGet ForestLog](https://img.shields.io/nuget/v/ForestLog.svg?style=flat)](https://www.nuget.org/packages/ForestLog) |
| ForestLog.Core | [![NuGet ForestLog.Core](https://img.shields.io/nuget/v/ForestLog.Core.svg?style=flat)](https://www.nuget.org/packages/ForestLog.Core) |

----

## What is this?

* TODO: Still under construction...

A minimalist logger interface, formatted as Json Lines (`*.jsonl`).

It provides the information required for logging with a simple interface and minimal configuration.
Eliminates complex configurations and maintenance labor.

### Operating Environment

The following platforms are supported by the package.

* NET 7, 6, 5
* NET Core 3.1, 3.0, 2.2, 2.1, 2.0
* NET Standard 2.1, 2.0, 1.6, 1.3
* NET Framework 4.8, 4.6.1, 4.5, 4.0, 3.5

----

## Basic usage

Install [ForestLog](https://www.nuget.org/packages/ForestLog) package.

```csharp
using ForestLog;

// Construct controller:
using var logController = LoggerFactory.CreateJsonLinesLogger(
    // Minimum output log level.
    LogLevels.Debug,
    // Output base directory path.
    "logs");

// Create logger:
ILogger logger = logController.CreateLogger();

// Write log entries:
var arg1 = 123;
var arg2 = 456;
logger.Debug($"Always using string interpolation: {arg1}");
logger.Trace($"Always using string interpolation: {arg2}");

try
{
    throw new ApplicationException("Failed a operation.");
}
catch (Exception ex)
{
    logger.Error(ex);
}
```

Output to (pseudo json formatted from jsonl):

```json
{
    "id": "0a913e2e-4ba7-4606-b703-2c9eccc9d217",
    "facility": "Unknown",
    "logLevel": "debug",
    "timestamp": "2022-12-06T09:27:04.5451256+09:00",
    "scopeId": 1,
    "message": "Always using string interpolation: 123",
    "exceptionType": null,
    "exceptionMessage": null,
    "additionalData": null,
    "memberName": "ReadMultipleMessage3",
    "filePath": "D:\\Projects\\ForestLog\\ForestLog.Tests\\JsonLineLoggerTests.cs",
    "line": 229,
    "managedThreadId": 16,
    "nativeThreadId": 11048,
    "taskId": -1,
    "processId": 55000
}
{
    "id": "31b4709f-f7f5-45b5-9381-75f64e23efce",
    "facility": "Unknown",
    "logLevel": "trace",
    "timestamp": "2022-12-06T09:27:04.5473678+09:00",
    "scopeId": 1,
    "message": "Always using string interpolation: 456",
    "exceptionType": null,
    "exceptionMessage": null,
    "additionalData": null,
    "memberName": "ReadMultipleMessage3",
    "filePath": "D:\\Projects\\ForestLog\\ForestLog.Tests\\JsonLineLoggerTests.cs",
    "line": 231,
    "managedThreadId": 16,
    "nativeThreadId": 11048,
    "taskId": -1,
    "processId": 55000
}
{
    "id": "5848c701-0190-453a-83b7-271023306d4a",
    "facility": "Unknown",
    "logLevel": "error",
    "timestamp": "2022-12-06T09:56:17.968195+09:00",
    "scopeId": 1,
    "message": "System.ApplicationException: Failed a operation.",
    "exceptionType": "System.ApplicationException",
    "exceptionMessage": "Failed a operation.",
    "additionalData": null,
    "memberName": "TraceException",
    "filePath": "D:\\Projects\\ForestLog\\ForestLog.Tests\\JsonLineLoggerTests.cs",
    "line": 179,
    "managedThreadId": 16,
    "nativeThreadId": 11048,
    "taskId": -1,
    "processId": 55000
}
```

## Annotates facility name


```csharp
// Create facility annoteted logger
var logger = logController.CreateLogger("DispatchController");

var unitCount = 5;
logger.Information($"Through the valid unit: Units={unitCount}");
```

Causes:

```json
{
    "facility": "DispatchController",
    "logLevel": "information",
    "message": "Through the valid unit: Units=5",
    // ...
}
```

## Awaited for exactly output

Normally, ForestLog outputs all log entries in the background context.
The use of an Awaitable method ensures that the log entries are actually output to a file.

```csharp
public async Task OutputAsync(ILogger logger)
{
    // We need to wait exactly output critical logs:
    await logger.InformationAsync($"Awaited to exactly output.");
}
```

## Scoped output

The scoped output features will apply log entry relations with `scopeId` identity on log key.

```csharp
public void Scope(ILogger parentLogger)
{
    parentLogger.TraceScope(logger =>
    {
        logger.Debug($"Output in child scope.");
        logger.Warinig($"Same child scope.");
    });
}

public Task ScopeAsync(ILogger parentLogger)
{
    return parentLogger.TraceScopeAsync(async logger =>
    {
        logger.Debug($"Output in child scope.");
        logger.Warinig($"Same child scope.");
    });
}
```

Causes:

```json
{
    "logLevel": "trace",
    "scopeId": 123, 
    "message": "Enter.",
    // ...
}
{
    "logLevel": "debug",
    "scopeId": 123, 
    "message": "Output in child scope.",
    // ...
}
{
    "logLevel": "warning",
    "scopeId": 123,
    "message": "Same child scope.",
    // ...
}
{
    "logLevel": "trace",
    "scopeId": 123, 
    "message": "Leave.",
    // ...
}
```

## Configure maximum log size and rotation

TODO:

## Crawling logs

TODO:

----

## Uses builtin awaitable value task (Advanced topic)

TODO:

----

## License

Apache-v2.
