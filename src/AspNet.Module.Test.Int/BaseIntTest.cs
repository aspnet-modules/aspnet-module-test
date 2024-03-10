using AspNet.Module.Test.Int.Auth;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AspNet.Module.Test.Int;

/// <summary>
///     Базовый класс интеграционного теста
/// </summary>
public abstract class BaseIntTest<TIntFixture, TProgram, TAuthHandler, TDbContext> :
    IClassFixture<BaseIntTestFixture<TProgram, TAuthHandler, TDbContext>>
    where TIntFixture : BaseIntTestFixture<TProgram, TAuthHandler, TDbContext>
    where TProgram : class
    where TAuthHandler : BaseTestAuthHandler
    where TDbContext : DbContext
{
    protected BaseIntTest(TIntFixture fixture)
    {
        Fixture = fixture;
    }

    protected TIntFixture Fixture { get; }
}