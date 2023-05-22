////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

namespace ForestLog

open ForestLog.Infrastructure
open System
open System.ComponentModel
open System.Diagnostics
open System.Runtime.CompilerServices
open System.Runtime.InteropServices

/// <summary>
/// ForestLog logger interface extension.
/// </summary>
[<AutoOpen>]
module public LoggerExtension =

    type public ILogger with

        /// <summary>
        /// Write a log entry.
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="message">Message (Mostly string interpolation)</param>
        /// <param name="additionalData">Additional data object when need to write</param>
        [<EditorBrowsable(EditorBrowsableState.Advanced)>]
        [<DebuggerStepperBoundary>]
        [<DebuggerStepThrough>]
        member logger.log(
            logLevel: LogLevels,
            message: IFormattable,
            [<Optional; DefaultParameterValue(null)>] additionalData: obj,
            [<CallerMemberName; Optional; DefaultParameterValue(null)>] memberName: string,
            [<CallerFilePath; Optional; DefaultParameterValue(null)>] filePath: string,
            [<CallerLineNumber; Optional; DefaultParameterValue(0)>] line: int) =
            match logger with
            | null -> ()
            | _ -> logger.Write(new WaitingLogEntry(
                logLevel,
                message,
                additionalData,
                null,
                memberName, filePath, line));

        /// <summary>
        /// Write a log entry.
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="ex">Exception</param>
        /// <param name="additionalData">Additional data object when need to write</param>
        [<EditorBrowsable(EditorBrowsableState.Advanced)>]
        [<DebuggerStepperBoundary>]
        [<DebuggerStepThrough>]
        member logger.log(
            logLevel: LogLevels,
            ex: Exception,
            [<CallerMemberName; Optional; DefaultParameterValue(null)>] memberName: string,
            [<CallerFilePath; Optional; DefaultParameterValue(null)>] filePath: string,
            [<CallerLineNumber; Optional; DefaultParameterValue(0)>] line: int) =
            match logger with
            | null -> ()
            | _ -> logger.Write(new WaitingLogEntry(
                logLevel,
                null,
                null,
                ex,
                memberName, filePath, line));

        /// <summary>
        /// Write a log entry.
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="ex">Exception</param>
        /// <param name="message">Message (Mostly string interpolation)</param>
        /// <param name="additionalData">Additional data object when need to write</param>
        [<EditorBrowsable(EditorBrowsableState.Advanced)>]
        [<DebuggerStepperBoundary>]
        [<DebuggerStepThrough>]
        member logger.log(
            logLevel: LogLevels,
            ex: Exception,
            message: IFormattable,
            [<CallerMemberName; Optional; DefaultParameterValue(null)>] memberName: string,
            [<CallerFilePath; Optional; DefaultParameterValue(null)>] filePath: string,
            [<CallerLineNumber; Optional; DefaultParameterValue(0)>] line: int) =
            match logger with
            | null -> ()
            | _ -> logger.Write(new WaitingLogEntry(
                logLevel,
                message,
                null,
                ex,
                memberName, filePath, line));

        //////////////////////////////////////////////////////////////////////
