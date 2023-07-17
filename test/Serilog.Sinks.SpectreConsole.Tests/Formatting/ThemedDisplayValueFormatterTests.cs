using Serilog.Events;
using Serilog.Sinks.SystemConsole.Formatting;
using Serilog.Sinks.SystemConsole.Themes;
using Xunit;

namespace Serilog.Sinks.SpectreConsole.Tests.Formatting
{
    public class ThemedDisplayValueFormatterTests
    {
        [Theory]
        [InlineData("Hello", null, "\"Hello\"")]
        [InlineData("Hello", "l", "Hello")]
        public void StringFormattingIsApplied(string value, string format, string expected)
        {
            var formatter = new ThemedDisplayValueFormatter(ConsoleTheme.None, null);
            var actual = formatter.FormatLiteralValue(new ScalarValue(value), format).Render();
            Assert.Equal(expected, actual);
        }
    }
}
