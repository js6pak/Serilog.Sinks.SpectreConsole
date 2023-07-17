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
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Serilog.Sinks.SystemConsole.Formatting
{
    class ThemedJsonValueFormatter : ThemedValueFormatter
    {
        readonly ThemedDisplayValueFormatter _displayFormatter;
        readonly IFormatProvider? _formatProvider;

        public ThemedJsonValueFormatter(ConsoleTheme theme, IFormatProvider? formatProvider)
            : base(theme)
        {
            _displayFormatter = new ThemedDisplayValueFormatter(theme, formatProvider);
            _formatProvider = formatProvider;
        }

        public override ThemedValueFormatter SwitchTheme(ConsoleTheme theme)
        {
            return new ThemedJsonValueFormatter(theme, _formatProvider);
        }

        protected override IEnumerable<IRenderable> VisitScalarValue(ThemedValueFormatterState state, ScalarValue scalar)
        {
            if (scalar is null)
                throw new ArgumentNullException(nameof(scalar));

            // At the top level, for scalar values, use "display" rendering.
            if (state.IsTopLevel)
                yield return _displayFormatter.FormatLiteralValue(scalar, state.Format);
            else
                yield return FormatLiteralValue(scalar);
        }

        protected override IEnumerable<IRenderable> VisitSequenceValue(ThemedValueFormatterState state, SequenceValue sequence)
        {
            if (sequence == null)
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
                foreach (var renderable in Visit(state.Nest(), sequence.Elements[index]))
                {
                    yield return renderable;
                }
            }

            yield return new Text("]", Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));
        }

        protected override IEnumerable<IRenderable> VisitStructureValue(ThemedValueFormatterState state, StructureValue structure)
        {
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

                yield return new Text(WriteQuotedJsonString(property.Name), Theme.ToSpectre(ConsoleThemeStyle.Name));

                yield return new Text(": ", Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));

                foreach (var renderable in Visit(state.Nest(), property.Value))
                {
                    yield return renderable;
                }
            }

            if (structure.TypeTag != null)
            {
                yield return new Text(delim, Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));

                yield return new Text(WriteQuotedJsonString("$type"), Theme.ToSpectre(ConsoleThemeStyle.Name));

                yield return new Text(": ", Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));

                yield return new Text(WriteQuotedJsonString(structure.TypeTag), Theme.ToSpectre(ConsoleThemeStyle.String));
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

                var style = element.Key.Value == null
                    ? ConsoleThemeStyle.Null
                    : element.Key.Value is string
                        ? ConsoleThemeStyle.String
                        : ConsoleThemeStyle.Scalar;

                yield return new Text(WriteQuotedJsonString((element.Key.Value ?? "null").ToString() ?? ""), Theme.ToSpectre(style));

                yield return new Text(": ", Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));

                foreach (var renderable in Visit(state.Nest(), element.Value))
                {
                    yield return renderable;
                }
            }

            yield return new Text("}", Theme.ToSpectre(ConsoleThemeStyle.TertiaryText));
        }

        IRenderable FormatLiteralValue(ScalarValue scalar)
        {
            var value = scalar.Value;

            if (value == null)
            {
                return new Text("null", Theme.ToSpectre(ConsoleThemeStyle.Null));
            }

            if (value is string str)
            {
                return new Text(WriteQuotedJsonString(str), Theme.ToSpectre(ConsoleThemeStyle.String));
            }

            if (value is ValueType)
            {
                if (value is int or uint or long or ulong or decimal or byte or sbyte or short or ushort)
                {
                    return new Text(((IFormattable)value).ToString(null, CultureInfo.InvariantCulture), Theme.ToSpectre(ConsoleThemeStyle.Number));
                }

                if (value is double d)
                {
                    return new Text(double.IsNaN(d) || double.IsInfinity(d)
                        ? WriteQuotedJsonString(d.ToString(CultureInfo.InvariantCulture))
                        : d.ToString("R", CultureInfo.InvariantCulture), Theme.ToSpectre(ConsoleThemeStyle.Number));
                }

                if (value is float f)
                {
                    return new Text(float.IsNaN(f) || float.IsInfinity(f)
                        ? WriteQuotedJsonString(f.ToString(CultureInfo.InvariantCulture))
                        : f.ToString("R", CultureInfo.InvariantCulture), Theme.ToSpectre(ConsoleThemeStyle.Number));
                }

                if (value is bool b)
                {
                    return new Text(b ? "true" : "false", Theme.ToSpectre(ConsoleThemeStyle.Boolean));
                }

                if (value is char ch)
                {
                    return new Text(WriteQuotedJsonString(ch.ToString()), Theme.ToSpectre(ConsoleThemeStyle.Scalar));
                }

                if (value is DateTime or DateTimeOffset)
                {
                    return new Text($"\"{((IFormattable)value).ToString("O", CultureInfo.InvariantCulture)}\"", Theme.ToSpectre(ConsoleThemeStyle.Scalar));
                }
            }

            return new Text(WriteQuotedJsonString(value.ToString() ?? ""), Theme.ToSpectre(ConsoleThemeStyle.Scalar));
        }

        public static string WriteQuotedJsonString(string str)
        {
            var stringWriter = new StringWriter();
            JsonValueFormatter.WriteQuotedJsonString(str, stringWriter);
            return stringWriter.ToString();
        }
    }
}
