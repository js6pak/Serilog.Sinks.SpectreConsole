using System.Collections.Generic;
using Serilog.Events;
using Spectre.Console.Rendering;

namespace Serilog.Sinks.SystemConsole.Output;

/// <summary>
/// Formats log events in a <see cref="IRenderable"/> representation.
/// </summary>
public interface ISpectreFormatter
{
    /// <summary>
    /// Format the log event into the output.
    /// </summary>
    /// <param name="logEvent">The event to format.</param>
    IEnumerable<IRenderable> Format(LogEvent logEvent);
}
