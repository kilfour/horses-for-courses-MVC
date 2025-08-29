using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;


namespace HorsesForCourses.Tests.Courses.B_UpdateRequiredSkills;

public class D_UpdateRequiredSkillsData : CourseDatabaseTests
{
    private void Act()
    {
        var context = GetDbContext();
        Reload(context).UpdateRequiredSkills(TheCanonical.Skills);
        context.SaveChanges();
    }

    [Fact]
    public void Skills_can_be_updated()
    {
        Act();
        Assert.Equal(TheCanonical.HardSkillsList, Reload().RequiredSkills);
    }
}