using System;
using System.IO;
using Serilog.Events;

namespace Serilog.Sinks.SystemConsole.Rendering;

internal static class Extensions
{
    public static string Render(this ScalarValue scalar, string? format = null, IFormatProvider? formatProvider = null)
    {
        var writer = new StringWriter();
        scalar.Render(writer, format, formatProvider);
        return writer.ToString();
    }
}
