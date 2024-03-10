using AspNet.Module.Test.Helpers;
using AspNet.Module.Test.Int.Auth;
using AspNet.Module.Test.Int.Database;
using AspNet.Module.Test.Int.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using RichardSzalay.MockHttp;

namespace AspNet.Module.Test.Int;

/// <summary>
///     Базовый класс Fixture
/// </summary>
public class BaseIntTestFixture<TProgram, TAuthHandler, TDbContext> : WebApplicationFactory<TProgram>
    where TProgram : class
    where TAuthHandler : BaseTestAuthHandler
    where TDbContext : DbContext
{
    public const string AuthSchema = "Test";
    public const string DatabaseKey = "Default";
    public const string EnvironmentName = "Testing";
    public const string RolesPolicy = "Roles";

    private readonly RunOnce _databaseRunOnce = new();

    protected virtual string Project => string.Empty;

    protected virtual bool IncludeFluentValidation { get; } = false;

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder
            .UseEnvironment("Testing")
            .ConfigureAppConfiguration((wbc, cb) => { ConfigureConfiguration(cb); })
            .ConfigureTestServices(sc =>
            {
                sc.WithoutHealthChecks();
                if (!IncludeFluentValidation)
                {
                    sc.WithoutFluentValidators();
                }

                AddAuth(sc);
                AddRefit(sc);
                // https://docs.microsoft.com/ru-ru/aspnet/core/test/integration-tests?view=aspnetcore-6.0#customize-webapplicationfactory
                _databaseRunOnce.Execute(() => InitializeDatabase(sc));
            });

    protected override IWebHostBuilder? CreateWebHostBuilder() =>
        base.CreateWebHostBuilder()?.UseEnvironment(EnvironmentName);

    protected virtual void AddAuth(IServiceCollection sc)
    {
        sc.AddAuthentication(AuthSchema)
            .AddScheme<AuthenticationSchemeOptions, TAuthHandler>(AuthSchema, _ => { });
        sc.AddAuthorization(options =>
        {
            options.AddPolicy(RolesPolicy, b =>
            {
                b.AuthenticationSchemes.Add("Test");
                b.RequireAuthenticatedUser();
            });
            options.DefaultPolicy = options.GetPolicy(RolesPolicy)!;
        });
    }

    protected virtual void AddRefit(IServiceCollection sc)
    {
        sc.AddSingleton(new MockHttpMessageHandler());
        sc.AddSingleton(sp => new RefitSettings
        {
            HttpMessageHandlerFactory = sp.GetRequiredService<MockHttpMessageHandler>
        });
    }

    protected virtual void ConfigureConfiguration(IConfigurationBuilder cb)
    {
        cb.AddUserSecrets<TProgram>(true);
        cb.AddEnvironmentVariables();
        cb.AddInMemoryCollection(new Dictionary<string, string>
        {
            { $"ConnectionStrings:{DatabaseKey}", ExtractConnStr(cb) }
        });
    }

    protected virtual string ExtractConnStr(IConfigurationBuilder configuration)
    {
        var config = configuration.Build();
        var connStr = GetConnStrByConfiguration(config);
        return DbConnectionUtils.ReplaceDatabase(connStr, DatabaseName(Project));
    }

    protected virtual void InitializeDatabase(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        dbContext.Database.EnsureCreated();
    }

    private static string DatabaseName(string project) =>
        DbConnectionUtils.NormalizeDatabaseName(
            DbConnectionUtils.DatabaseNameByDbContext<TDbContext>("test_int_" + project));

    private static string GetConnStrByConfiguration(IConfiguration configuration, string key = DatabaseKey) =>
        configuration.GetConnectionString(key) ??
        throw new ArgumentNullException(nameof(configuration), "Не указана строка подключения к БД");
}