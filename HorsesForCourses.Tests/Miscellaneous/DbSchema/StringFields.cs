using HorsesForCourses.Service.Warehouse;
using Microsoft.EntityFrameworkCore;

namespace HorsesForCourses.Tests.Miscellaneous.DbSchema;

public class StringFields
{
    [Fact]
    public void AllStringsHaveLengthDefined()
    {
        var options =
            new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("DoesNotMatter")
                .Options;
        using var context = new AppDbContext(options);
        var sql =
            context.Database.GenerateCreateScript()
                .Split(Environment.NewLine)
                .Where(a => a.Contains("nvarchar"));
        foreach (var stringSql in sql)
        {
            Assert.Contains("nvarchar(100)", stringSql);
        }

    }
}