using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace AspNet.Module.Test.Unit;

public abstract class BaseDbUnitTests<TDbTestFixture, TDbContext> : BaseIocUnitTests<TDbTestFixture>
    where TDbContext : DbContext
    where TDbTestFixture : DbUnitTestFixture<TDbContext>
{
    protected BaseDbUnitTests(TDbTestFixture fixture, ITestOutputHelper testOutputHelper)
        : base(fixture, testOutputHelper)
    {
    }

    public override Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    protected override void PreInitialize(IServiceCollection services)
    {
        base.PreInitialize(services);
        RegisterDbContext(services);
        services.AddSingleton<IMemoryCache>(
            _ => new MemoryCache(new MemoryDistributedCacheOptions { SizeLimit = null }));
    }

    protected virtual void RegisterDbContext(IServiceCollection services)
    {
        services.AddSingleton(Fixture.DbContext);
        services.AddSingleton<DbContext>(s => s.GetRequiredService<TDbContext>());
    }
}