using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Module.Test.Unit.Database;

[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public static class DbContextFactory
{
    public static TDbContext CreateByOptions<TDbContext>(DbContextOptions<TDbContext> options)
        where TDbContext : DbContext
    {
        const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        return (TDbContext)Activator.CreateInstance(typeof(TDbContext), flags, null,
            new object[] { options }, null)!;
    }

    public static TDbContext CreateByOptions<TDbContext>(DbContextOptions options)
        where TDbContext : DbContext
    {
        const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        return (TDbContext)Activator.CreateInstance(typeof(TDbContext), flags, null,
            new object[] { options }, null)!;
    }
}