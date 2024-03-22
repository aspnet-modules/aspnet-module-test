using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace AspNet.Module.Test.Loggers;

public class XunitLogger<T> : ILogger<T>, IDisposable
{
    private readonly string? _categoryName;
    private readonly ITestOutputHelper _output;

    public XunitLogger(ITestOutputHelper output)
    {
        _output = output;
    }

    public XunitLogger(ITestOutputHelper output, string categoryName)
    {
        _output = output;
        _categoryName = categoryName;
    }

    public void Dispose()
    {
        // ignore
    }

    public IDisposable BeginScope<TState>(TState state) => this;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);
        _output.WriteLine(_categoryName != null ? $"{_categoryName}: {message}" : message);
    }
}