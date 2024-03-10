using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AspNet.Module.Test.Unit.Database;

public static class DbContextOptionsFactory
{
    public const string DefaultDatabaseName = "UnitTests";

    public static DbContextOptionsBuilder<TDbContext> CreateInMemoryOptions<TDbContext>(
        string databaseName = DefaultDatabaseName)
        where TDbContext : DbContext
    {
        var dbContextBuilder = new DbContextOptionsBuilder<TDbContext>();
        dbContextBuilder.UseInMemoryDatabase(databaseName);
        dbContextBuilder.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        return dbContextBuilder;
    }

    public static DbContextOptionsBuilder<TDbContext> CreateInPostgresOptions<TDbContext>(string connStr)
        where TDbContext : DbContext
    {
        var dbContextBuilder = new DbContextOptionsBuilder<TDbContext>();
        dbContextBuilder.UseNpgsql(connStr);
        return dbContextBuilder;
    }
}