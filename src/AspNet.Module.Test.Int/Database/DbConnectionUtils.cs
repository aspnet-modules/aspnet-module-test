using System.Text;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AspNet.Module.Test.Int.Database;

public static class DbConnectionUtils
{
    public static string ReplaceDatabase(string originalConnStr, string database)
    {
        var dbsb = new NpgsqlConnectionStringBuilder(originalConnStr) { Database = database };
        return dbsb.ToString();
    }

    public static string NormalizeDatabaseName(string databaseName) =>
        databaseName
            .Replace('-', '_')
            .Replace('.', '_')
            .ToLowerInvariant()
            .Normalize();

    public static string DatabaseNameByDbContext<TDbContext>(string prefix = "test") where TDbContext : DbContext
    {
        var strBuilder = new StringBuilder(prefix);
        if (!string.IsNullOrEmpty(prefix) && !prefix.EndsWith("_"))
        {
            strBuilder.Append('_');
        }

        strBuilder.Append(typeof(TDbContext).Name.ToLowerInvariant().Normalize());
        return strBuilder.ToString();
    }
}