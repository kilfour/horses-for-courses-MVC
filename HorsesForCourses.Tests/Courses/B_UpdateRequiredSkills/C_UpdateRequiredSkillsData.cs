using HorsesForCourses.Core.Domain.Skills;
using HorsesForCourses.Tests.Tools.Courses;


namespace HorsesForCourses.Tests.Courses.B_UpdateRequiredSkills;

public class C_UpdateRequiredSkillsData : CourseDatabaseTests
{
    private void Act()
    {
        var context = GetDbContext();
        Reload(context).UpdateRequiredSkills(["one", "two"]);
        context.SaveChanges();
    }

    [Fact]
    public void Skills_can_be_updated()
    {
        Act();
        Assert.Equal([Skill.From("one"), Skill.From("two")], Reload().RequiredSkills);
    }
}