using HorsesForCourses.Core.Domain.Coaches.InvalidationReasons;
using HorsesForCourses.Core.Domain.Skills;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;


namespace HorsesForCourses.Tests.Coaches.B_UpdateSkills;

public class E_UpdateSkillsDomain : CoachDomainTests
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
        Entity.UpdateSkills(TheCanonical.Skills);
        Assert.Equal(TheCanonical.HardSkills.OrderBy(a => a.Value), Entity.Skills.OrderBy(a => a.Value));
    }

    [Fact]
    public void UpdateSkills_WithInValidSkill_Throws()
    {
        var skills = new List<string> { "" };
        Assert.Throws<SkillValueCanNotBeEmpty>(() => Entity.UpdateSkills(skills));
    }

    [Fact]
    public void UpdateSkills_With_Duplicates_Throws()
    {
        var skills = new List<string> { "a", "a" };
        Assert.Throws<CoachAlreadyHasSkill>(() => Entity.UpdateSkills(skills));
    }
}
