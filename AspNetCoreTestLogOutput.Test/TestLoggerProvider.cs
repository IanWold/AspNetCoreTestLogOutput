using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace AspNetCoreTestLogOutput.Test;

public class TestLoggerProvider : ILoggerProvider
{
    public record LogMessage(LogLevel LogLevel, Exception? Exception, string Message);

    public class TestLogger(ConcurrentBag<LogMessage> logs) : ILogger
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
            logs.Add(new LogMessage(logLevel, exception, formatter(state, exception)));

        public bool IsEnabled(LogLevel logLevel) =>
            true;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull =>
            null;
    }

    private readonly ConcurrentBag<LogMessage> _logs = [];

    public IReadOnlyCollection<LogMessage> Logs =>
        _logs;

    public ILogger CreateLogger(string categoryName) =>
        new TestLogger(_logs);

    public void Dispose() =>
        GC.SuppressFinalize(this);
}
