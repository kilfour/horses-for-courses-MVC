using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Service.Warehouse;
using HorsesForCourses.Tests.Tools;

namespace HorsesForCourses.Tests.Coaches.B_UpdateSkills;

public class D_UpdateSkillsData : DatabaseTests
{
    private readonly Coach coach;

    public D_UpdateSkillsData()
    {
        coach = TheCanonical.Coach();
        AddToDb(coach);
    }

    private Coach Reload() => Reload(GetDbContext());
    private Coach Reload(AppDbContext dbContext) => dbContext.Coaches.Single(a => a.Id == coach.Id);

    private void Act()
    {
        var context = GetDbContext();
        Reload(context).UpdateSkills(TheCanonical.Skills);
        context.SaveChanges();
    }

    [Fact]
    public void Skills_can_be_updated()
    {
        Act();
        Assert.Equal(TheCanonical.HardSkillsList, Reload().Skills);
    }
}