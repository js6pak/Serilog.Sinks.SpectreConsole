﻿// Copyright 2017 Serilog Contributors
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

using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole;
using Serilog.Sinks.SystemConsole.Output;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using Spectre.Console;

namespace Serilog
{
    /// <summary>
    /// Adds the WriteTo.SpectreConsole() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class SpectreConsoleLoggerConfigurationExtensions
    {
        const string DefaultConsoleOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// Writes log events to <see cref="AnsiConsole"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="outputTemplate">A message template describing the format used to write to the sink.
        /// The default is <code>"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"</code>.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        /// to be changed at runtime.</param>
        /// <param name="theme">The theme to apply to the styled output. If not specified,
        /// uses <see cref="SystemConsoleTheme.Literate"/>.</param>
        /// <param name="console">The spectre console instance.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="sinkConfiguration"/> is <code>null</code></exception>
        /// <exception cref="ArgumentNullException">When <paramref name="outputTemplate"/> is <code>null</code></exception>
        public static LoggerConfiguration SpectreConsole(
            this LoggerSinkConfiguration sinkConfiguration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultConsoleOutputTemplate,
            IFormatProvider? formatProvider = null,
            LoggingLevelSwitch? levelSwitch = null,
            ConsoleTheme? theme = null, 
            IAnsiConsole? console = null)
        {
            if (sinkConfiguration is null) throw new ArgumentNullException(nameof(sinkConfiguration));
            if (outputTemplate is null) throw new ArgumentNullException(nameof(outputTemplate));

            var formatter = new OutputTemplateRenderer(theme ?? SystemConsoleThemes.Literate, outputTemplate, formatProvider);
            return sinkConfiguration.Sink(new SpectreConsoleSink(console ?? AnsiConsole.Console, formatter), restrictedToMinimumLevel, levelSwitch);
        }

        /// <summary>
        /// Writes log events to <see cref="AnsiConsole"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="formatter">Controls the rendering of log events into text, for example to log JSON. To
        /// control plain text formatting, use the overload that accepts an output template.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        /// to be changed at runtime.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="sinkConfiguration"/> is <code>null</code></exception>
        /// <exception cref="ArgumentNullException">When <paramref name="formatter"/> is <code>null</code></exception>
        public static LoggerConfiguration SpectreConsole(
            this LoggerSinkConfiguration sinkConfiguration,
            ISpectreFormatter formatter,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch? levelSwitch = null)
        {
            if (sinkConfiguration is null) throw new ArgumentNullException(nameof(sinkConfiguration));
            if (formatter is null) throw new ArgumentNullException(nameof(formatter));

            return sinkConfiguration.Sink(new SpectreConsoleSink(AnsiConsole.Console, formatter), restrictedToMinimumLevel, levelSwitch);
        }
    }
}
