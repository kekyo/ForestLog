////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.CompilerServices;

namespace ForestLog.Internal;

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
        string message,
        string? exceptionType,
        string? exceptionMessage,
        JToken? additionalData,
        string memberName,
        string filePath,
        int line,
        int managedThreadId,
        int nativeThreadId,
        int taskId,
        int processId) :
        base(
            id, facility, logLevel, timestamp, scopeId,
            message, exceptionType, exceptionMessage, additionalData,
            memberName, filePath, line,
            managedThreadId, nativeThreadId, taskId, processId)
    {
    }
}
