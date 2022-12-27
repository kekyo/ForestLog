////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ForestLog.Internal;

[DebuggerStepThrough]
internal sealed class JsonSerializableLogEntry : LogEntry
{
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [JsonConstructor]
    public JsonSerializableLogEntry(
        Guid id,
        string facility,
        LogLevels logLevel,
        DateTimeOffset timestamp,
        int scopeId,
        int parentScopeId,
        string message,
        JToken? additionalData,
        string memberName,
        string filePath,
        int line,
        int managedThreadId,
        int nativeThreadId,
        int taskId,
        int processId) :
        base(
            id, facility, logLevel, timestamp, scopeId, parentScopeId,
            message, additionalData,
            memberName, filePath, line,
            managedThreadId, nativeThreadId, taskId, processId)
    {
    }

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public JsonSerializableLogEntry(WaitingLogEntry waitingLogEntry) :
        base(
            waitingLogEntry,
            waitingLogEntry.Message.ToString(null, CultureInfo.InvariantCulture),
            waitingLogEntry.AdditionalData is { } ad ? JToken.FromObject(ad) : null,
            Utilities.ProcessId)
    {
    }
}
