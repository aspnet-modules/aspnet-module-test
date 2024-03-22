using AspNet.Module.Test.Helpers;
using AspNet.Module.Test.Unit.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace AspNet.Module.Test.Unit;

public class DbUnitTestFixture<TDbContext> : ConfigUnitTestFixture
    where TDbContext : DbContext
{
    public const string DatabaseKey = "Default";

    private readonly AsyncRunOnce _runOnce;
    private string? _dbConnStr;

    protected DbUnitTestFixture()
    {
        _runOnce = new AsyncRunOnce();
    }

    public string DbConnStr => _dbConnStr ??= ResolveConnStr();

    public TDbContext DbContext { get; private set; } = null!;

    protected virtual string Project => string.Empty;
    protected DbContextOptions<TDbContext> DbContextOptions { get; private set; } = null!;

    public override async Task DisposeAsync()
    {
        await DbContext.Database.CloseConnectionAsync();
        await DbContext.DisposeAsync();
    }

    public override async Task InitializeAsync() =>
        await _runOnce.Execute(async () =>
        {
            DbContextOptions = CreateDbContextOptions();
            DbContext = DbContextFactory.CreateByOptions(DbContextOptions);
            await ConfigureDatabase(DbContext);
        });

    protected virtual async Task ConfigureDatabase(TDbContext dbContext) =>
        await dbContext.Database.EnsureCreatedAsync();

    protected virtual void ConfigureDataSource(NpgsqlDataSourceBuilder builder)
    {
    }

    protected virtual void ConfigureDbContextOptionsBuilder(DbContextOptionsBuilder<TDbContext> builder)
    {
        builder.UseSnakeCaseNamingConvention();
        builder.EnableDetailedErrors();
        builder.UseApplicationServiceProvider(new ServiceCollection().BuildServiceProvider());
    }

    protected virtual DbContextOptions<TDbContext> CreateDbContextOptions()
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(DbConnStr);
        ConfigureDataSource(dataSourceBuilder);
        var optionsBuilder = DbContextOptionsFactory.CreateInPostgresOptions<TDbContext>(dataSourceBuilder.Build());
        ConfigureDbContextOptionsBuilder(optionsBuilder);

        return optionsBuilder.Options;
    }

    private string ResolveConnStr()
    {
        var connStr = GetConnStrByConfiguration(Configuration);
        return DbConnectionUtils.ReplaceDatabase(connStr, DatabaseName(Project));
    }

    private static string DatabaseName(string project) =>
        DbConnectionUtils.NormalizeDatabaseName(
            DbConnectionUtils.DatabaseNameByDbContext<TDbContext>("test_unit_" + project));

    private static string GetConnStrByConfiguration(IConfiguration configuration, string key = DatabaseKey) =>
        configuration.GetConnectionString(key) ??
        throw new ArgumentNullException(nameof(configuration), "Не указана строка подключения к БД");
}