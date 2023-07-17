using System.Collections.Generic;
using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Serilog.Sinks.SpectreConsole.Tests;

internal static class Extensions
{
    public static string Render(this IRenderable renderable)
    {
        var stringBuilder = new StringBuilder();

        foreach (var segment in renderable.GetSegments(AnsiConsole.Console))
        {
            stringBuilder.Append(segment.Text);
        }

        return stringBuilder.ToString();
    }

    public static string Render(this IEnumerable<IRenderable> renderables)
    {
        var stringBuilder = new StringBuilder();

        foreach (var renderable in renderables)
        {
            stringBuilder.Append(renderable.Render());
        }

        return stringBuilder.ToString();
    }
}
