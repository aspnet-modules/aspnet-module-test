using AspNet.Module.Test.Loggers;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace AspNet.Module.Test.Unit;

public abstract class BaseUnitTests<TTestFixture> : IAsyncLifetime
{
    protected BaseUnitTests(TTestFixture fixture, ITestOutputHelper testOutputHelper)
    {
        Fixture = fixture;
        TestOutputHelper = testOutputHelper;
        Logger = new XunitLogger<BaseUnitTests<TTestFixture>>(testOutputHelper);
    }

    public TTestFixture Fixture { get; }

    /// <summary>
    ///     Базовый логгер
    /// </summary>
    protected ITestOutputHelper TestOutputHelper { get; }
    
    /// <summary>
    ///     Логгер
    /// </summary>
    protected ILogger<BaseUnitTests<TTestFixture>> Logger { get; }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}