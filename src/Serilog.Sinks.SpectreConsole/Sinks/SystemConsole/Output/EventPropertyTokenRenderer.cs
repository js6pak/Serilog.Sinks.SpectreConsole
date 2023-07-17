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
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.SystemConsole.Rendering;
using Serilog.Sinks.SystemConsole.Themes;
using Spectre.Console;
using Spectre.Console.Rendering;
using Padding = Serilog.Sinks.SystemConsole.Rendering.Padding;

namespace Serilog.Sinks.SystemConsole.Output
{
    class EventPropertyTokenRenderer : OutputTemplateTokenRenderer
    {
        readonly ConsoleTheme _theme;
        readonly PropertyToken _token;
        readonly IFormatProvider? _formatProvider;

        public EventPropertyTokenRenderer(ConsoleTheme theme, PropertyToken token, IFormatProvider? formatProvider)
        {
            _theme = theme;
            _token = token;
            _formatProvider = formatProvider;
        }

        public override IEnumerable<IRenderable> Render(LogEvent logEvent)
        {
            // If a property is missing, don't render anything (message templates render the raw token here).
            if (!logEvent.Properties.TryGetValue(_token.PropertyName, out var propertyValue))
            {
                yield return new Text(Padding.Apply(string.Empty, _token.Alignment));
                yield break;
            }

            var writer = new StringWriter();

            // If the value is a scalar string, support some additional formats: 'u' for uppercase
            // and 'w' for lowercase.
            if (propertyValue is ScalarValue sv && sv.Value is string literalString)
            {
                var cased = Casing.Format(literalString, _token.Format);
                writer.Write(cased);
            }
            else
            {
                propertyValue.Render(writer, _token.Format, _formatProvider);
            }

            yield return new Text(Padding.Apply(writer.ToString(), _token.Alignment), _theme.ToSpectre(ConsoleThemeStyle.SecondaryText));
        }
    }
}