using HorsesForCourses.Core.Domain.Coaches.InvalidationReasons;
using HorsesForCourses.Core.Domain.Skills;
using HorsesForCourses.Tests.Tools.Coaches;


namespace HorsesForCourses.Tests.Coaches.B_UpdateSkills;

public class B_UpdateSkillsDomain : CoachDomainTests
{
    [Fact]
    public void CreateSkill_Valid_ShouldSucceed()
        => Assert.Equal("DotNet", Skill.From("DotNet").Value);

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_Skill_Empty_ShouldThrow(string? value)
        => Assert.Throws<SkillValueCanNotBeEmpty>(() => Skill.From(value!));

    [Fact]
    public void UpdateSkills_WithValidData_ShouldSucceed()
    {
        var skills = new List<string> { "one", "two" };
        Entity.UpdateSkills(skills);
        Assert.Equal([Skill.From("one"), Skill.From("two")], Entity.Skills);
    }

    [Fact]
    public void UpdateSkills_WithInValidSkill_Throws()
    {
        var skills = new List<string> { "", "two" };
        Assert.Throws<SkillValueCanNotBeEmpty>(() => Entity.UpdateSkills(skills));
    }

    [Fact]
    public void UpdateSkills_With_Duplicates_Throws()
    {
        var skills = new List<string> { "two", "two" };
        Assert.Throws<CoachAlreadyHasSkill>(() => Entity.UpdateSkills(skills));
    }
}
