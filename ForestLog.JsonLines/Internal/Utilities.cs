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
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics;
using System.Text;

namespace ForestLog.Internal;

internal static class Utilities
{
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
        var defaultNamingStrategy = new CamelCaseNamingStrategy();
        JsonSerializer = new JsonSerializer
        {
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            NullValueHandling = NullValueHandling.Include,
            Formatting = Formatting.None,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            ContractResolver = new DefaultContractResolver
            { NamingStrategy = defaultNamingStrategy, },
        };
        JsonSerializer.Converters.Add(new StringEnumConverter(defaultNamingStrategy));
    }
}
