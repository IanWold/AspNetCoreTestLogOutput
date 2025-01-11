using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace AspNetCoreTestLogOutput.Test;

public class Test
{
    private TestLoggerProvider LoggerProvider { get ;set; } = new();

    [Fact]
    public async Task TestErrorLogged()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddProvider(LoggerProvider);
                logging.SetMinimumLevel(LogLevel.Trace);
            })
        );

        var client = factory.CreateClient();

        await client.SendAsync(new HttpRequestMessage(HttpMethod.Put, "/test"));

        Assert.Contains(LoggerProvider.Logs, l => l.LogLevel == LogLevel.Error && l.Message.Contains("Test error"));
    }
}
