using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Service.Warehouse;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HorsesForCourses.Tests.Tools;

public abstract class DatabaseTests : IDisposable
{
    private readonly SqliteConnection connection;
    protected readonly DbContextOptions<AppDbContext> Options;
    protected AppDbContext GetDbContext() => new(Options);

    public DatabaseTests()
    {
        connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection);
        Options = builder.Options;
        var dbContext = GetDbContext();
        dbContext.Database.EnsureCreated();
    }

    public void Dispose() => connection.Dispose();

    protected IdPrimitive AddToDb<T>(DomainEntity<T> entity)
    {
        using var context = GetDbContext();
        {
            context.Add(entity!);
        }
        context.SaveChanges();
        return entity.Id.Value;
    }
}
