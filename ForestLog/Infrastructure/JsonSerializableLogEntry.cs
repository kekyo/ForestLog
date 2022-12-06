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

internal sealed class JsonSerializableLogEntry
{
    public readonly Guid Id;
    public readonly string Facility;
    public readonly LogLevels LogLevel;
    public readonly DateTimeOffset Timestamp;
    public readonly int ScopeId;
    public readonly string Message;
    public readonly string? ExceptionType;
    public readonly string? ExceptionMessage;
    public readonly JToken? AdditionalData;
    public readonly string MemberName;
    public readonly string FilePath;
    public readonly int Line;
    public readonly int ManagedThreadId;
    public readonly int NativeThreadId;
    public readonly int TaskId;
    public readonly int ProcessId;

    //////////////////////////////////////////////////////////////////////

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
        int processId)
    {
        this.Id = id;
        this.Facility = facility;
        this.LogLevel = logLevel;
        this.Timestamp = timestamp;
        this.ScopeId = scopeId;
        this.Message = message;
        this.ExceptionType = exceptionType;
        this.ExceptionMessage = exceptionMessage;
        this.AdditionalData = additionalData;
        this.MemberName = memberName;
        this.FilePath = filePath;
        this.Line = line;
        this.ManagedThreadId = managedThreadId;
        this.NativeThreadId = nativeThreadId;
        this.TaskId = taskId;
        this.ProcessId = processId;
    }

    public override string ToString() =>
        $"[{this.Timestamp:yyyy/MM/dd HH:mm:ss.fff}]: {this.Facility}: {this.LogLevel}: [{this.ScopeId}]: {this.Message}";
}
