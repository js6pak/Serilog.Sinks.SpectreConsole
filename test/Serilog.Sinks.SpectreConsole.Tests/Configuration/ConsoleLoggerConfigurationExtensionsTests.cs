using System.IO;
using System.Linq;
using Spectre.Console;
using Xunit;

namespace Serilog.Sinks.SpectreConsole.Tests.Configuration
{
    public class ConsoleLoggerConfigurationExtensionsTests
    {
        [Fact]
        public void OutputFormattingIsIgnored()
        {
            using (var stream = new MemoryStream())
            {
                var sw = new StreamWriter(stream);

                System.Console.SetOut(sw);
                var config = new LoggerConfiguration()
                    .WriteTo.SpectreConsole();

                var logger = config.CreateLogger();

                logger.Error("test");
                stream.Position = 0;

                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    var controlCharacterCount = result.Count(c => char.IsControl(c) && !char.IsWhiteSpace(c));
                    Assert.Equal(0, controlCharacterCount);
                }
            }
        }
        
        [Fact]
        public void OutputFormattingIsPresent()
        {
            using (var stream = new MemoryStream())
            {
                var sw = new StreamWriter(stream);

                System.Console.SetOut(sw);
                var config = new LoggerConfiguration()
                    .WriteTo.SpectreConsole(console: AnsiConsole.Create(new AnsiConsoleSettings
                    {
                        Ansi = AnsiSupport.Yes,
                    }));

                var logger = config.CreateLogger();

                logger.Error("test");
                stream.Position = 0;

                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    var controlCharacterCount = result.Count(c => char.IsControl(c) && !char.IsWhiteSpace(c));
                    Assert.NotEqual(0, controlCharacterCount);
                }
            }
        }
    }
}