using AspNet.Module.Test.Loggers;
using AspNet.Module.Test.Unit.Mock;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace AspNet.Module.Test.Unit;

public abstract class BaseIocUnitTests<TDbTestFixture> : BaseUnitTests<TDbTestFixture>
    where TDbTestFixture : ConfigUnitTestFixture
{
    protected BaseIocUnitTests(TDbTestFixture fixture, ITestOutputHelper testOutputHelper)
        : base(fixture, testOutputHelper)
    {
    }

    /// <summary>
    ///     Сервисы
    /// </summary>
    protected IServiceProvider Services { get; set; } = null!;

    public override Task InitializeAsync()
    {
        Services = CreateServiceProvider();
        return Task.CompletedTask;
    }

    public override Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    protected TService GetService<TService>() where TService : notnull
    {
        return Services.GetRequiredService<TService>();
    }

    protected virtual void PreInitialize(IServiceCollection services)
    {
        services.AddSingleton(Fixture.Configuration);
        services.AddSingleton(TestOutputHelper);
        services.AddSingleton(typeof(ILogger<>), typeof(XunitLogger<>));
        services.AddSingleton(typeof(ILoggerFactory), typeof(XUnitLoggerFactory));
        services.AddSingleton<MockMediator>();
        services.AddSingleton(s => s.GetRequiredService<MockMediator>().Value);
    }

    protected virtual void PostInitialize(IServiceProvider sp)
    {
    }

    private IServiceProvider CreateServiceProvider()
    {
        var sc = new ServiceCollection();
        PreInitialize(sc);
        var sp = sc.BuildServiceProvider();
        PostInitialize(sp);
        return sp;
    }
}