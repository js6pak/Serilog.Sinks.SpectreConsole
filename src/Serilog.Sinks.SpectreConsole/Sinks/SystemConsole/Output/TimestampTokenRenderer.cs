// Copyright 2017 Serilog Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.SystemConsole.Themes;
using Spectre.Console;
using Spectre.Console.Rendering;
using Padding = Serilog.Sinks.SystemConsole.Rendering.Padding;

namespace Serilog.Sinks.SystemConsole.Output
{
    class TimestampTokenRenderer : OutputTemplateTokenRenderer
    {
        readonly ConsoleTheme _theme;
        readonly PropertyToken _token;
        readonly IFormatProvider? _formatProvider;

        public TimestampTokenRenderer(ConsoleTheme theme, PropertyToken token, IFormatProvider? formatProvider)
        {
            _theme = theme;
            _token = token;
            _formatProvider = formatProvider;
        }

        public override IEnumerable<IRenderable> Render(LogEvent logEvent)
        {
            var sv = new DateTimeOffsetValue(logEvent.Timestamp);

            var buffer = new StringWriter();
            sv.Render(buffer, _token.Format, _formatProvider);
            var str = buffer.ToString();
            yield return new Text(Padding.Apply(str, _token.Alignment), _theme.ToSpectre(ConsoleThemeStyle.SecondaryText));
        }

        readonly struct DateTimeOffsetValue
        {
            public DateTimeOffsetValue(DateTimeOffset value)
            {
                Value = value;
            }

            public DateTimeOffset Value { get; }

            public void Render(TextWriter output, string? format = null, IFormatProvider? formatProvider = null)
            {
                var custom = (ICustomFormatter?)formatProvider?.GetFormat(typeof(ICustomFormatter));
                if (custom != null)
                {
                    output.Write(custom.Format(format, Value, formatProvider));
                    return;
                }

#if NET
                Span<char> buffer = stackalloc char[32];
                if (Value.TryFormat(buffer, out int written, format, formatProvider ?? CultureInfo.InvariantCulture))
                    output.Write(buffer.Slice(0, written));
                else
                    output.Write(Value.ToString(format, formatProvider ?? CultureInfo.InvariantCulture));
#else
                output.Write(Value.ToString(format, formatProvider ?? CultureInfo.InvariantCulture));
#endif
            }
        }
    }
}
