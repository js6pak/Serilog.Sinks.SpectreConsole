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
using Serilog.Sinks.SystemConsole.Rendering;
using Serilog.Sinks.SystemConsole.Themes;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Serilog.Sinks.SystemConsole.Formatting
{
    class ThemedDisplayValueFormatter : ThemedValueFormatter
    {
        readonly IFormatProvider? _formatProvider;

        public ThemedDisplayValueFormatter(ConsoleTheme theme, IFormatProvider? formatProvider)
            : base(theme)
        {
            _formatProvider = formatProvider;
        }

        public override ThemedValueFormatter SwitchTheme(ConsoleTheme theme)
        {
            return new ThemedDisplayValueFormatter(theme, _formatProvider);
        }

        protected override IEnumerable<IRenderable> VisitScalarValue(ThemedValueFormatterState state, ScalarValue scalar)
        {
            if (scalar is null)
                throw new ArgumentNullException(nameof(scalar));
            yield return FormatLiteralValue(scalar, state.Format);
        }

        protected override IEnumerable<IRenderable> VisitSequenceValue(ThemedValueFormatterState state, SequenceValue sequence)
        {
            if (sequence is null)
                throw new ArgumentNullException(nameof(sequence));

            yield return new Text("[", Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));

            var delim = string.Empty;
            for (var index = 0; index < sequence.Elements.Count; ++index)
            {
                if (delim.Length != 0)
                {
                    yield return new Text(delim, Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));
                }

                delim = ", ";
                foreach (var renderable in Visit(state, sequence.Elements[index]))
                {
                    yield return renderable;
                }
            }

            yield return new Text("]", Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));
        }

        protected override IEnumerable<IRenderable> VisitStructureValue(ThemedValueFormatterState state, StructureValue structure)
        {
            if (structure.TypeTag != null)
            {
                yield return new Text(structure.TypeTag, Theme.ToSpectre(ConsoleThemeStyle.Name));
                yield return new Text(" ");
            }

            yield return new Text("{", Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));

            var delim = string.Empty;
            for (var index = 0; index < structure.Properties.Count; ++index)
            {
                if (delim.Length != 0)
                {
                    yield return new Text(delim, Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));
                }

                delim = ", ";

                var property = structure.Properties[index];

                yield return new Text(property.Name, Theme.ToSpectre(ConsoleThemeStyle.Name));

                yield return new Text("=", Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));

                foreach (var renderable in Visit(state.Nest(), property.Value))
                {
                    yield return renderable;
                }
            }

            yield return new Text("}", Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));
        }

        protected override IEnumerable<IRenderable> VisitDictionaryValue(ThemedValueFormatterState state, DictionaryValue dictionary)
        {
            yield return new Text("{", Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));

            var delim = string.Empty;
            foreach (var element in dictionary.Elements)
            {
                if (delim.Length != 0)
                {
                    yield return new Text(delim, Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));
                }

                delim = ", ";

                yield return new Text("[", Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));

                foreach (var renderable in Visit(state.Nest(), element.Key))
                {
                    yield return renderable;
                }

                yield return new Text("]=", Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));

                foreach (var renderable in Visit(state.Nest(), element.Value))
                {
                    yield return renderable;
                }
            }

            yield return new Text("}", Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));
        }

        public IRenderable FormatLiteralValue(ScalarValue scalar, string? format)
        {
            var value = scalar.Value;

            if (value is null)
            {
                return new Text("null", Theme.ToSpectre(ConsoleThemeStyle.Null));
            }

            if (value is string str)
            {
                return new Text(format == "l" ? str : ThemedJsonValueFormatter.WriteQuotedJsonString(str), Theme.ToSpectre(ConsoleThemeStyle.String));
            }

            if (value is ValueType)
            {
                if (value is int or uint or long or ulong or decimal or byte or sbyte or short or ushort or float or double)
                {
                    return new Text(scalar.Render(format, _formatProvider), Theme.ToSpectre(ConsoleThemeStyle.Number));
                }

                if (value is bool b)
                {
                    return new Text(b.ToString(), Theme.ToSpectre(ConsoleThemeStyle.Boolean));
                }

                if (value is char ch)
                {
                    return new Text($"\'{ch.ToString()}\'", Theme.ToSpectre(ConsoleThemeStyle.Scalar));
                }
            }

            return new Text(scalar.Render(format, _formatProvider), Theme.ToSpectre(ConsoleThemeStyle.Scalar));
        }
    }
}
