////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Tasks;
using System;
using System.ComponentModel;
using System.Threading;

namespace ForestLog;

public interface ILogger
{
    int ScopeId { get; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    void Write(
        LogLevels logLevel,
        IFormattable message,
        Exception? ex,
        object? additionalData,
        string memberName,
        string filePath,
        int line);

    [EditorBrowsable(EditorBrowsableState.Never)]
    LoggerAwaitable WriteAsync(
        LogLevels logLevel,
        IFormattable message,
        Exception? ex,
        object? additionalData,
        string memberName,
        string filePath,
        int line,
        CancellationToken ct);

    [EditorBrowsable(EditorBrowsableState.Never)]
    ILogger NewScope();
}
