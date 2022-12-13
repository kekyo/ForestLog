////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ForestLog;

/// <summary>
/// ForestLog log entry entity.
/// </summary>
/// <remarks>This is an immutable type of log entry information.</remarks>
public class LogEntry
{
    public Guid Id { get; }
    public string Facility { get; }
    public LogLevels LogLevel { get; }
    public DateTimeOffset Timestamp { get; }
    public int ScopeId { get; }
    public string Message { get; }
    [DefaultValue(null)]
    public object? AdditionalData { get; }
    public string MemberName { get; }
    public string FilePath { get; }
    public int Line { get; }
    public int ManagedThreadId { get; }
    public int NativeThreadId { get; }
    public int TaskId { get; }
    public int ProcessId { get; }

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public LogEntry(
        Guid id,
        string facility,
        LogLevels logLevel,
        DateTimeOffset timestamp,
        int scopeId,
        string message,
        object? additionalData,
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
