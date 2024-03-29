﻿////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using ForestLog.Tasks;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ForestLog.Infrastructure;

internal sealed class Logger : ILogger
{
    private readonly LogController controller;
    private readonly string facility;
    private readonly int scopeId;
    private readonly int parentScopeId;

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public Logger(LogController controller, string facility, int parentScopeId)
    {
        this.controller = controller;
        this.facility = facility;
        this.scopeId = this.controller.NewScopeId();
        this.parentScopeId = parentScopeId;
    }

    //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Get facility.
    /// </summary>
    public string Facility =>
        this.facility;

    /// <summary>
    /// For reference use only minimum output log level.
    /// </summary>
    public LogLevels MinimumOutputLogLevel
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [DebuggerStepThrough]
        get => this.controller.MinimumOutputLogLevel;
    }

    /// <summary>
    /// For reference use only current scope id.
    /// </summary>
    public int ScopeId
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [DebuggerStepThrough]
        get => this.scopeId;
    }

    /// <summary>
    /// For reference use only parent scope id.
    /// </summary>
    public int ParentScopeId
    {
#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [DebuggerStepThrough]
        get => this.parentScopeId;
    }

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public void Write(
        WaitingLogEntry logEntry) =>
        this.controller.Write(
            logEntry, this.facility, this.scopeId, this.parentScopeId);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public LoggerAwaitable WriteAsync(
        WaitingLogEntry logEntry,
        CancellationToken ct) =>
        this.controller.WriteAsync(
            logEntry, this.facility, this.scopeId, this.parentScopeId, ct);

    //////////////////////////////////////////////////////////////////////

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
    [DebuggerStepperBoundary]
#endif
    [DebuggerStepThrough]
    public ILogger NewScope() =>
        new Logger(this.controller, this.facility, this.scopeId);

    //////////////////////////////////////////////////////////////////////

    public override string ToString() =>
        $"Root logger: Facility={this.facility}, Id={this.scopeId}";
}
