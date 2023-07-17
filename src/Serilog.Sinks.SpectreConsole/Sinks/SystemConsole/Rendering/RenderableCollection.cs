using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Parsing;
using Spectre.Console.Rendering;

namespace Serilog.Sinks.SystemConsole.Rendering;

internal class RenderableCollection : Renderable
{
    private readonly List<IRenderable> _items;
    private readonly Alignment? _alignment;

    public RenderableCollection(params IRenderable[] items) : this((IEnumerable<IRenderable>)items)
    {
    }

    public RenderableCollection(Alignment? alignment, params IRenderable[] items) : this(items, alignment)
    {
    }

    public RenderableCollection(IEnumerable<IRenderable> items, Alignment? alignment = null)
    {
        if (items is null) throw new ArgumentNullException(nameof(items));

        _items = items.ToList();
        _alignment = alignment;
    }

    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        var measurements = _items.Select(c => c.Measure(options, maxWidth)).ToArray();
        return new Measurement(measurements.Min(c => c.Min), measurements.Max(c => c.Max));
    }

    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        return _alignment is { } alignment
            ? RenderWithPadding(options, maxWidth, alignment)
            : RenderWithoutPadding(options, maxWidth);
    }

    private IEnumerable<Segment> RenderWithoutPadding(RenderOptions options, int maxWidth)
    {
        foreach (var renderable in _items)
        {
            foreach (var segment in renderable.Render(options, maxWidth))
            {
                yield return segment;
            }
        }
    }

    private IEnumerable<Segment> RenderWithPadding(RenderOptions options, int maxWidth, Alignment alignment)
    {
        var segments = new List<Segment>();
        var length = 0;

        foreach (var renderable in _items)
        {
            foreach (var segment in renderable.Render(options, maxWidth))
            {
                segments.Add(segment);
                length += segment.Text.Length;
            }
        }

        var pad = alignment.Width - length;

        if (alignment.Direction == AlignmentDirection.Right)
            yield return Segment.Padding(pad);

        foreach (var segment in segments)
        {
            yield return segment;
        }

        if (alignment.Direction == AlignmentDirection.Left)
            yield return Segment.Padding(pad);
    }
}
