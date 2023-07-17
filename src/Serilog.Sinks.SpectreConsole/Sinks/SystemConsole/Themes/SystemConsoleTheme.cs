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
using System.Linq;
using Spectre.Console;

namespace Serilog.Sinks.SystemConsole.Themes
{
    /// <summary>
    /// A console theme using the <see cref="System.ConsoleColor"/> class.
    /// </summary>
    public class SystemConsoleTheme : ConsoleTheme
    {
        /// <summary>
        /// A theme using only gray, black and white.
        /// </summary>
        public static SystemConsoleTheme Grayscale { get; } = SystemConsoleThemes.Grayscale;

        /// <summary>
        /// A theme in the style of the original <i>Serilog.Sinks.Literate</i>.
        /// </summary>
        public static SystemConsoleTheme Literate { get; } = SystemConsoleThemes.Literate;

        /// <summary>
        /// A theme based on the original Serilog "colored console" sink.
        /// </summary>
        public static SystemConsoleTheme Colored { get; } = SystemConsoleThemes.Colored;

        /// <summary>
        /// Construct a theme given a set of styles.
        /// </summary>
        /// <param name="styles">Styles to apply within the theme.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="styles"/> is <code>null</code></exception>
        public SystemConsoleTheme(IReadOnlyDictionary<ConsoleThemeStyle, Style> styles)
        {
            if (styles is null) throw new ArgumentNullException(nameof(styles));
            Styles = styles.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        /// <inheritdoc/>
        public IReadOnlyDictionary<ConsoleThemeStyle, Style> Styles { get; }

        /// <inheritdoc/>
        public override Style ToSpectre(ConsoleThemeStyle style)
        {
            return Styles.TryGetValue(style, out var value) ? value : Style.Plain;
        }
    }
}