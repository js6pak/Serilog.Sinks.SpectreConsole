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
using Serilog.Data;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Spectre.Console.Rendering;

namespace Serilog.Sinks.SystemConsole.Formatting
{
    abstract class ThemedValueFormatter : LogEventPropertyValueVisitor<ThemedValueFormatterState, IEnumerable<IRenderable>>
    {
        protected ConsoleTheme Theme { get; }

        protected ThemedValueFormatter(ConsoleTheme theme)
        {
            Theme = theme ?? throw new ArgumentNullException(nameof(theme));
        }

        public IEnumerable<IRenderable> Format(LogEventPropertyValue value, string? format, bool literalTopLevel = false)
        {
            return Visit(new ThemedValueFormatterState {  Format = format, IsTopLevel = literalTopLevel }, value);
        }

        public abstract ThemedValueFormatter SwitchTheme(ConsoleTheme theme);
    }
}