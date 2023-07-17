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
using Spectre.Console;

namespace Serilog.Sinks.SystemConsole.Themes
{
    static class SystemConsoleThemes
    {
        public static SystemConsoleTheme Literate { get; } = new(
            new Dictionary<ConsoleThemeStyle, Style>
            {
                [ConsoleThemeStyle.Text] = new(ConsoleColor.White),
                [ConsoleThemeStyle.SecondaryText] = new(ConsoleColor.Gray),
                [ConsoleThemeStyle.TertiaryText] = new(ConsoleColor.DarkGray),
                [ConsoleThemeStyle.Invalid] = new(ConsoleColor.Yellow),
                [ConsoleThemeStyle.Null] = new(ConsoleColor.Blue),
                [ConsoleThemeStyle.Name] = new(ConsoleColor.Gray),
                [ConsoleThemeStyle.String] = new(ConsoleColor.Cyan),
                [ConsoleThemeStyle.Number] = new(ConsoleColor.Magenta),
                [ConsoleThemeStyle.Boolean] = new(ConsoleColor.Blue),
                [ConsoleThemeStyle.Scalar] = new(ConsoleColor.Green),
                [ConsoleThemeStyle.LevelVerbose] = new(ConsoleColor.Gray),
                [ConsoleThemeStyle.LevelDebug] = new(ConsoleColor.Gray),
                [ConsoleThemeStyle.LevelInformation] = new(ConsoleColor.White),
                [ConsoleThemeStyle.LevelWarning] = new(ConsoleColor.Yellow),
                [ConsoleThemeStyle.LevelError] = new(ConsoleColor.White, ConsoleColor.Red),
                [ConsoleThemeStyle.LevelFatal] = new(ConsoleColor.White, ConsoleColor.Red),
            });

        public static SystemConsoleTheme Grayscale { get; } = new(
            new Dictionary<ConsoleThemeStyle, Style>
            {
                [ConsoleThemeStyle.Text] = new(ConsoleColor.White),
                [ConsoleThemeStyle.SecondaryText] = new(ConsoleColor.Gray),
                [ConsoleThemeStyle.TertiaryText] = new(ConsoleColor.DarkGray),
                [ConsoleThemeStyle.Invalid] = new(ConsoleColor.White, ConsoleColor.DarkGray),
                [ConsoleThemeStyle.Null] = new(ConsoleColor.White),
                [ConsoleThemeStyle.Name] = new(ConsoleColor.Gray),
                [ConsoleThemeStyle.String] = new(ConsoleColor.White),
                [ConsoleThemeStyle.Number] = new(ConsoleColor.White),
                [ConsoleThemeStyle.Boolean] = new(ConsoleColor.White),
                [ConsoleThemeStyle.Scalar] = new(ConsoleColor.White),
                [ConsoleThemeStyle.LevelVerbose] = new(ConsoleColor.DarkGray),
                [ConsoleThemeStyle.LevelDebug] = new(ConsoleColor.DarkGray),
                [ConsoleThemeStyle.LevelInformation] = new(ConsoleColor.White),
                [ConsoleThemeStyle.LevelWarning] = new(ConsoleColor.White, ConsoleColor.DarkGray),
                [ConsoleThemeStyle.LevelError] = new(ConsoleColor.Black, ConsoleColor.White),
                [ConsoleThemeStyle.LevelFatal] = new(ConsoleColor.Black, ConsoleColor.White),
            });

        public static SystemConsoleTheme Colored { get; } = new(
            new Dictionary<ConsoleThemeStyle, Style>
            {
                [ConsoleThemeStyle.Text] = new(ConsoleColor.Gray),
                [ConsoleThemeStyle.SecondaryText] = new(ConsoleColor.DarkGray),
                [ConsoleThemeStyle.TertiaryText] = new(ConsoleColor.DarkGray),
                [ConsoleThemeStyle.Invalid] = new(ConsoleColor.Yellow),
                [ConsoleThemeStyle.Null] = new(ConsoleColor.White),
                [ConsoleThemeStyle.Name] = new(ConsoleColor.White),
                [ConsoleThemeStyle.String] = new(ConsoleColor.White),
                [ConsoleThemeStyle.Number] = new(ConsoleColor.White),
                [ConsoleThemeStyle.Boolean] = new(ConsoleColor.White),
                [ConsoleThemeStyle.Scalar] = new(ConsoleColor.White),
                [ConsoleThemeStyle.LevelVerbose] = new(ConsoleColor.Gray, ConsoleColor.DarkGray),
                [ConsoleThemeStyle.LevelDebug] = new(ConsoleColor.White, ConsoleColor.DarkGray),
                [ConsoleThemeStyle.LevelInformation] = new(ConsoleColor.White, ConsoleColor.Blue),
                [ConsoleThemeStyle.LevelWarning] = new(ConsoleColor.DarkGray, ConsoleColor.Yellow),
                [ConsoleThemeStyle.LevelError] = new(ConsoleColor.White, ConsoleColor.Red),
                [ConsoleThemeStyle.LevelFatal] = new(ConsoleColor.White, ConsoleColor.Red),
            });
    }
}
