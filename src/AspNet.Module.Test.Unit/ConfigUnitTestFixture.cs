using Microsoft.Extensions.Configuration;
using Xunit;

namespace AspNet.Module.Test.Unit;

/// <summary>
///     Базовый Test Fixture
/// </summary>
public class ConfigUnitTestFixture : IAsyncLifetime
{
    private IConfiguration? _configuration;

    public IConfiguration Configuration => _configuration ??= CreateConfiguration();

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    private IConfiguration CreateConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder();
        PreConfiguration(configurationBuilder);
        return configurationBuilder.Build();
    }

    protected virtual void PreConfiguration(ConfigurationBuilder builder)
    {
        builder.AddEnvironmentVariables();
    }
}