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
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.SystemConsole.Formatting;
using Serilog.Sinks.SystemConsole.Themes;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Serilog.Sinks.SystemConsole.Rendering
{
    class ThemedMessageTemplateRenderer
    {
        readonly ConsoleTheme _theme;
        readonly ThemedValueFormatter _valueFormatter;
        readonly bool _isLiteral;
        static readonly ConsoleTheme NoTheme = new EmptyConsoleTheme();
        readonly ThemedValueFormatter _unthemedValueFormatter;

        public ThemedMessageTemplateRenderer(ConsoleTheme theme, ThemedValueFormatter valueFormatter, bool isLiteral)
        {
            _theme = theme ?? throw new ArgumentNullException(nameof(theme));
            _valueFormatter = valueFormatter;
            _isLiteral = isLiteral;
            _unthemedValueFormatter = valueFormatter.SwitchTheme(NoTheme);
        }

        public IEnumerable<IRenderable> Render(MessageTemplate template, IReadOnlyDictionary<string, LogEventPropertyValue> properties)
        {
            foreach (var token in template.Tokens)
            {
                if (token is TextToken tt)
                {
                    yield return RenderTextToken(tt);
                }
                else
                {
                    var pt = (PropertyToken)token;
                    foreach (var renderable in RenderPropertyToken(pt, properties))
                    {
                        yield return renderable;
                    }
                }
            }
        }

        IRenderable RenderTextToken(TextToken tt)
        {
            return new Text(tt.Text, _theme.ToSpectre(ConsoleThemeStyle.Text));
        }

        IEnumerable<IRenderable> RenderPropertyToken(PropertyToken pt, IReadOnlyDictionary<string, LogEventPropertyValue> properties)
        {
            if (!properties.TryGetValue(pt.PropertyName, out var propertyValue))
            {
                return new[] { new Text(pt.ToString(), _theme.ToSpectre(ConsoleThemeStyle.Invalid)) };
            }

            if (!pt.Alignment.HasValue)
            {
                return RenderValue(_theme, _valueFormatter, propertyValue, pt.Format);
            }

            return new[] { new RenderableCollection(RenderValue(_theme, _valueFormatter, propertyValue, pt.Format), pt.Alignment) };
        }

        IEnumerable<IRenderable> RenderValue(ConsoleTheme theme, ThemedValueFormatter valueFormatter, LogEventPropertyValue propertyValue, string? format)
        {
            if (_isLiteral && propertyValue is ScalarValue { Value: string value })
            {
                return new[] { new Text(value, theme.ToSpectre(ConsoleThemeStyle.String)) };
            }

            return valueFormatter.Format(propertyValue, format, _isLiteral);
        }
    }
}
