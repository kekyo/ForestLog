# ForestLog

![ForestLog](Images/ForestLog.100.png)

ForestLog - A minimalist logger interface.

[![Project Status: WIP – Initial development is in progress, but there has not yet been a stable, usable release suitable for the public.](https://www.repostatus.org/badges/latest/wip.svg)](https://www.repostatus.org/#wip)

## NuGet

Minimum packages:

| Package  | NuGet                                                                                                                |
|:---------|:---------------------------------------------------------------------------------------------------------------------|
| ForestLog | [![NuGet ForestLog](https://img.shields.io/nuget/v/ForestLog.svg?style=flat)](https://www.nuget.org/packages/ForestLog) |
| ForestLog.JsonLines | [![NuGet ForestLog.JsonLines](https://img.shields.io/nuget/v/ForestLog.JsonLines.svg?style=flat)](https://www.nuget.org/packages/ForestLog.JsonLines) |

ASP.NET Core bridge:

| Package  | NuGet                                                                                                                |
|:---------|:---------------------------------------------------------------------------------------------------------------------|
| ForestLog.Extensions.Logging | [![NuGet ForestLog.Extensions.Logging](https://img.shields.io/nuget/v/ForestLog.Extensions.Logging.svg?style=flat)](https://www.nuget.org/packages/ForestLog.Extensions.Logging) |

----

## What is this?

* TODO: Still under construction...

A minimalist logger interface, formatted as [Json Lines (`*.jsonl`).](https://jsonlines.org/)

It provides the information required for logging with a simple interface and minimal configuration.
Eliminates complex configurations and maintenance labor.

### Operating Environment

Core interface library:

* .NET 7, 6, 5
* .NET Core 3.1, 3.0, 2.2, 2.1, 2.0
* .NET Standard 2.1, 2.0, 1.6, 1.3
* .NET Framework 4.8, 4.6.1, 4.5, 4.0, 3.5

ASP.NET Core bridge:

* .NET 7, 6, 5
* .NET Core 3.1
* ASP.NET Core 1.0 or upper

----

## Basic usage

Install [ForestLog](https://www.nuget.org/packages/ForestLog) and [ForestLog.JsonLines](https://www.nuget.org/packages/ForestLog.JsonLines) packages.

We need to create "Log controller" from the factory:

```csharp
using ForestLog;

// Construct log controller:
using var logController = LogController.Factory.CreateJsonLines(
    // Output base directory path.
    "logs",
    // Minimum output log level.
    LogLevels.Debug);
```

Then, create a logger interface and ready to output:

```csharp
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

// Write log entry with additional data:
logger.Information($"See additional data below",
    new {
        Amount = 123,
        Message = "ABC",
        NameOfProduct = "PAC-MAN quarter",
    });
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
    "memberName": "PurchaseProductAsync",
    "filePath": "D:\\Projects\\AwsomeItemSite\\AwsomeItemSite.cs",
    "line": 229,
    "managedThreadId": 16,
    "nativeThreadId": 11048,
    "taskId": -1,
    "processId": 43608
}
{
    "id": "31b4709f-f7f5-45b5-9381-75f64e23efce",
    "facility": "Unknown",
    "logLevel": "trace",
    "timestamp": "2022-12-06T09:27:04.5473678+09:00",
    "scopeId": 1,
    "message": "Always using string interpolation: 456",
    "memberName": "PurchaseProductAsync",
    "filePath": "D:\\Projects\\AwsomeItemSite\\AwsomeItemSite.cs",
    "line": 230,
    "managedThreadId": 16,
    "nativeThreadId": 11048,
    "taskId": -1,
    "processId": 43608
}
{
    "id": "5848c701-0190-453a-83b7-271023306d4a",
    "facility": "Unknown",
    "logLevel": "error",
    "timestamp": "2022-12-06T09:56:17.968195+09:00",
    "scopeId": 1,
    "message": "System.ApplicationException: Failed a operation.",
    "additionalData": {
        "name": "System.ApplicationException",
        "message": "Failed a operation."
    },
    "memberName": "PurchaseProductAsync",
    "filePath": "D:\\Projects\\AwsomeItemSite\\AwsomeItemSite.cs",
    "line": 238,
    "managedThreadId": 16,
    "nativeThreadId": 11048,
    "taskId": -1,
    "processId": 43608
}
{
    "id": "e453d20f-e1cb-4464-b189-833153237e5b",
    "facility": "Unknown",
    "logLevel": "information",
    "timestamp": "2022-12-08T10:07:00.2802106+09:00",
    "scopeId": 1,
    "message": "See additional data below",
    "additionalData": {
        "amount": 123,
        "message": "ABC",
        "nameOfProduct": "PAC-MAN quarter"
    },
    "memberName": "PurchaseProductAsync",
    "filePath": "D:\\Projects\\AwsomeItemSite\\AwsomeItemSite.cs",
    "line": 242,
    "managedThreadId": 16,
    "nativeThreadId": 11048,
    "taskId": -1,
    "processId": 43608
}
```

The log level values are:

```csharp
// The lower symbol name is the most important.
// This order affects `MinimumOutputLogLevel` limitation.
public enum LogLevels
{
    Debug,
    Trace,
    Information,
    Warning,
    Error,
    Fatal,
    Ignore,   // <-- Will ignore any log output.
}
```

## Annotates facility name

```csharp
// Create facility annoteted logger
var logger = logController.CreateLogger("DispatchController");

var unitCount = 5;
logger.Information($"Through the valid unit: Units={unitCount}");
```

Result:

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
And the time between entering and exiting the scope is then measured.

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

Result:

```json
{
    "logLevel": "trace",
    "scopeId": 123,      // <-- Same scope id
    "message": "Enter: Parent=42",   // <-- Parent logger scope id
    // ...
}
{
    "logLevel": "debug",
    "scopeId": 123,      // <-- Same scope id
    "message": "Output in child scope.",
    // ...
}
{
    "logLevel": "warning",
    "scopeId": 123,      // <-- Same scope id
    "message": "Same child scope.",
    // ...
}
{
    "logLevel": "trace",
    "scopeId": 123,      // <-- Same scope id
    "message": "Leave: Elapsed=00:00:00.00146248",
    // ...
}
```

The timestamp from `Enter` to `Leave` in the same `scopeId` can be used to calculate the time at tally time,
but elapsed time indicated in `Leave` message is even more precise.

Scope output can include arguments, return values and exception information:

```csharp
public string Scope(ILogger parentLogger, int a, double b, string c)
{
    // Using `new` operator with implicitly type `BlockScopeArguments`.
    return parentLogger.TraceScope(new(a, b, c), logger =>
    {
        return (a + b) + c;
    });
}
```

Result:

```json
{
    "logLevel": "trace",
    "scopeId": 456, 
    "message": "Enter: Parent=42",
    "additionalData": [
        111,
        222.333,
        "ABC"
    ],
    // ...
}
{
    "logLevel": "trace",
    "scopeId": 456, 
    "message": "Leave: Elapsed=00:00:00.00146248",
    "additionalData": "333.333ABC",
    // ...
}
```

Leave with exception:

```json
{
    "logLevel": "trace",
    "scopeId": 456, 
    "message": "Leave with exception: Elapsed=00:00:00.00146248",
    "additionalData": {
        "name": "System.ApplicationException",
        "message": "Application might has invalid state..."
    },
    // ...
}
```

## Configure maximum log size and rotation

Will switch log file when current log file size is exceed.

```csharp
using var logController = LogController.Factory.CreateJsonLines(
    "logs",
    LogLevels.Debug,
    // Size to next file.
    1 * 1024 * 1024  // bytes
    );
```

Result:

![Applied log size configuration](Images/logs_directory.png)

Enable log file rotation:

```csharp
using var logController = LogController.Factory.CreateJsonLines(
    "logs",
    LogLevels.Debug,
    1 * 1024 * 1024,
    // Maximum log files.
    10
    );
```

## Suspend and resume

In an environment such as a smartphone,
log output must be suspended and resumed as the application transitions between states.

The following example will correspond to an application transition in Xamarin Android:

```csharp
public sealed class MainActivity
{
    private readonly ILogController logController =
        LogController.Factory.CreateJsonLines(...);

    // ...

    protected override void OnPause()
    {
        this.logController.Suspend();
        base.OnPause();
    }

    protected override void OnResume()
    {
        base.OnResume();
        this.logController.Resume();
    }
}
```

* `Suspend()` method writes all queued log entries into the log files (will block while completed).
  * After that, any logging request will be ignored when before `Resume()` is called.
* `Resume()` method releases the above.

## Programmatically retreive log entries

Event to monitor log outputted in real time:

```csharp
logController.Arrived += (s, e) =>
{
    // This thread context is worker thread.
    // So you have to dispatch UI thread when using GUI frameworks.
    Console.WriteLine(e.LogEntry.ToString());
};
```

Or, filter by predicates from all logs recorded (including outputted to files):

```csharp
LogEntry[] importantLogs = await logController.QueryLogEntriesAsync(
    // Maximum number of log entries.
    100,
    // Filter function.
    logEntry => logEntry.LogLevel >= LogLevels.Warning);
```

## ASP.NET Core bridge configuration

Install [ForestLog.Extensions.Logging](https://www.nuget.org/packages/ForestLog.Extensions.Logging) package,
and configure using with `AddForestLog()` method extension:

```csharp
using var logController = LogController.Factory.CreateJsonLines(
    /* ... */);

var builder = WebApplication.CreateBuilder();

builder.WebHost.
    ConfigureLogging(builder => builder.AddForestLog(logController)).
    UseUrls("http://localhost/");

var webApplication = builder.Build();

// ...
```

* Or, you can use `builder.Services.AddForestLog()` directly.
* Yes, it is implemented for `Microsoft.Extensions.Logging` interfaces.
  So you can apply this package to ASP.NET Core, Entity Framework Core and any other projects.

----

## Uses builtin awaitable value task (Advanced topic)

ForestLog has its own awaitable type, the `LoggerAwaitable` type.
This structure is a value-type likes the `ValueTask` type and allowing low-cost asynchronous operations.
It also defines an inter-conversion operator between the `Task` and `ValueTask` type,
allowing seamless use as follows:

```csharp
// Implicitly conversion from `Task`
async Task AsyncOperation()
{
    // ...
}

LoggerAwaitable awaitable = AsyncOperation();
await awaitable;
```

```csharp
// Implicitly conversion from `ValueTask`
async ValueTask AsyncOperation()
{
    // ...
}

LoggerAwaitable awaitable = AsyncOperation();
await awaitable;
```

```csharp
// Implicitly conversion from `Task<T>`
async Task<int> AsyncOperationWithResult()
{
    // ...
}

LoggerAwaitable<int> awaitable = AsyncOperationWithResult();
var result = await awaitable;
```

```csharp
// async-await operation
async LoggerAwaitable AsyncOperation()
{
    await Task.Delay(100);
}

await AsyncOperation();
```

```csharp
// async-await operation with result
async LoggerAwaitable<int> AsyncOperationWithResult()
{
    await Task.Delay(100);
    return 123;
}

var result = await AsyncOperationWithResult();
```

These `LoggerAwaitable` types are defined for the following reasons:

* Elimination of dependencies on assemblies containing `ValueTask` types.
* Elimination of complications due to inter-conversion between `Task` and `ValueTask` types.

For example, using the `LoggerAwaitable` type,
you can easily (simply) write and reduce asynchronous operation cost the following:

```csharp
// `TraceScopeAsync` method receives `Func<ILogger, LoggerAwaitable<int>>` delegate type
// and returns `LoggerAwaitable<int>` type.
public Task<int> ComplextOperationAsync() =>
    this.logger.TraceScopeAsync(async logger =>
    {
        // ...

        return result;
    });
```

Note: In `netcoreapp2.1` or later and `netstandard2.1`,
the `ValueTask` is not required any external dependencies.
So we can use `ValueTask` conversion naturally on these environments.

----

## License

Apache-v2.
