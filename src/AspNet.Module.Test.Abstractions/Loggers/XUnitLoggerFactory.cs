using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace AspNet.Module.Test.Loggers;

public class XUnitLoggerFactory : ILoggerFactory
{
    private readonly ITestOutputHelper _testOutputHelper;

    private ILoggerProvider? _provider;

    public XUnitLoggerFactory(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _provider = new XUnitLoggerProvider(_testOutputHelper);
    }

    public void Dispose() => _provider?.Dispose();

    public void AddProvider(ILoggerProvider provider) => _provider = provider;

    public ILogger CreateLogger(string categoryName) =>
        _provider?.CreateLogger(categoryName) ??
        new XunitLogger<XUnitLoggerFactory>(_testOutputHelper, categoryName);


    private class XUnitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public XUnitLoggerProvider(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName) =>
            new XunitLogger<XUnitLoggerProvider>(_testOutputHelper, categoryName);
    }
}