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
using Spectre.Console;
using Spectre.Console.Rendering;
using Padding = Serilog.Sinks.SystemConsole.Rendering.Padding;

namespace Serilog.Sinks.SystemConsole.Output
{
    class NewLineTokenRenderer : OutputTemplateTokenRenderer
    {
        readonly Alignment? _alignment;

        public NewLineTokenRenderer(Alignment? alignment)
        {
            _alignment = alignment;
        }

        public override IEnumerable<IRenderable> Render(LogEvent logEvent)
        {
            if (_alignment.HasValue)
                yield return new Text(Padding.Apply(Environment.NewLine, _alignment.Value));
            else
                yield return Text.NewLine;
        }
    }
}
