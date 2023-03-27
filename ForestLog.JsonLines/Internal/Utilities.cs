////////////////////////////////////////////////////////////////////////////
//
// ForestLog - A minimalist logger interface.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ForestLog.Internal;

internal static class Utilities
{
    private static readonly char[] splitChars = new[] { '\r', '\n' };

    public static readonly int ProcessId =
#if NET5_0_OR_GREATER
        Environment.ProcessId;
#else
        Process.GetCurrentProcess().Id;
#endif

    public static readonly Encoding UTF8 = new UTF8Encoding(false, false);
    public static readonly JsonSerializer JsonSerializer;

    static Utilities()
    {
        JsonSerializer = new JsonSerializer
        {
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            Formatting = Formatting.None,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };
        JsonSerializer.Converters.Add(
            new StringEnumConverter(new CamelCaseNamingStrategy()));
    }

    public static JObject CreateExceptionObject(Exception ex, HashSet<Exception> agged)
    {
        var exo = new JObject();

        exo["name"] = ex.GetType().FullName ?? "unknown";
        exo["message"] = ex.Message;
        exo["stackFrames"] = new JArray(ex.StackTrace?.
            Split(splitChars, StringSplitOptions.RemoveEmptyEntries).
            Select(line => line.Trim()).
            ToArray() ??
            CoreUtilities.Empty<string>());

        static JObject CreateStopObject(string message)
        {
            var so = new JObject();
            so["name"] = "ExceptionObject";
            so["message"] = message;
            return so;
        }

        try
        {
            exo["innerExceptions"] = new JArray(ex switch
            {
                AggregateException aex => aex.InnerExceptions.
                    Select(iex => agged.Add(iex) ?
                        CreateExceptionObject(iex, agged) :
                        CreateStopObject($"(Recursive reference: {iex.GetType().FullName})")).
                    ToArray(),
                _ when ex.InnerException is { } iex =>
                    new[] { iex }.
                    Select(iex => agged.Add(iex) ?
                        CreateExceptionObject(iex, agged) :
                        CreateStopObject($"(Recursive reference: {iex.GetType().FullName})")).
                    ToArray(),
                _ => CoreUtilities.Empty<JObject>(),
            });
        }
        catch (Exception ex2)
        {
            exo["innerExceptions"] = new JArray(new[] {
                CreateStopObject(
                    $"(Could not get inner exceptions: {ex2.GetType().FullName}: {ex2.Message})"),
            });
        }

        return exo;
    }
}
