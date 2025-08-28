using HorsesForCourses.Api.Warehouse;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Core.Domain.Skills;
using HorsesForCourses.Tests.Tools;


namespace HorsesForCourses.Tests.Coaches.B_UpdateSkills;


public class C_UpdateSkillsData : TheDatabaseTest
{
    private readonly Coach coach;

    public C_UpdateSkillsData()
    {
        coach = TheCanonical.Coach();
        AddToDb(coach);
    }

    private void Act()
    {
        var context = GetDbContext();
        Reload(context).UpdateSkills(["one", "two"]);
        context.SaveChanges();
    }

    private Coach Reload() => Reload(GetDbContext());
    private Coach Reload(AppDbContext dbContext) => dbContext.Coaches.Single(a => a.Id == coach.Id);

    [Fact]
    public void Skills_can_be_updated()
    {
        Act();
        Assert.Equal([Skill.From("one"), Skill.From("two")], Reload().Skills);
    }
}