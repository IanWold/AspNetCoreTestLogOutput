using System.Collections.Concurrent;
using AspNetCoreTestLogOutput.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace AspNetCoreTestLogOutput.Test;

file record LogMessage(LogLevel LogLevel, string Message);

file class TestLogger(ConcurrentBag<LogMessage> logs) : ILogger
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
        logs.Add(new(logLevel, formatter(state, exception)));

    public bool IsEnabled(LogLevel logLevel) =>
        true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull =>
        null;
}

file class TestLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentBag<LogMessage> _logs = [];

    public IReadOnlyCollection<LogMessage> Logs =>
        _logs;

    public ILogger CreateLogger(string categoryName) =>
        new TestLogger(_logs);

    public void Dispose() =>
        GC.SuppressFinalize(this);
}

public class Test
{
    [Fact]
    public async Task TestErrorLogged()
    {
        var loggerProvider = new TestLoggerProvider();
        var client = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
                builder.ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddProvider(loggerProvider);
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
            )
            .CreateClient();

        var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Put, "/test"));

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains(loggerProvider.Logs, l => l.LogLevel == LogLevel.Error && l.Message.Contains("Test error"));
    }
}
