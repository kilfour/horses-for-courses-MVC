using HorsesForCourses.Service.Warehouse;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HorsesForCourses.Tests.Tools;

public abstract class TheDatabaseTest : IDisposable
{
    private readonly SqliteConnection connection;
    protected readonly DbContextOptions<AppDbContext> Options;
    protected AppDbContext GetDbContext() => new(Options);

    public TheDatabaseTest()
    {
        connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection);
        Options = builder.Options;
        var dbContext = GetDbContext();
        dbContext.Database.EnsureCreated();
    }

    public void Dispose() => connection.Dispose();

    protected void AddToDb(params object[] entities)
    {
        using var context = GetDbContext();
        foreach (var entity in entities)
        {
            context.Add(entity!);
        }
        context.SaveChanges();
    }
}
