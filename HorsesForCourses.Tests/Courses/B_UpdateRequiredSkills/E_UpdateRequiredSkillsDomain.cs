using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
using HorsesForCourses.Core.Domain.Skills;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;


namespace HorsesForCourses.Tests.Courses.B_UpdateRequiredSkills;

public class E_UpdateRequiredSkillsDomain : CourseDomainTests
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
    public void UpdateRequiredSkills_WithValidData_ShouldSucceed()
    {
        Entity.UpdateRequiredSkills(TheCanonical.Skills);
        Assert.Equal(TheCanonical.HardSkillsList, Entity.RequiredSkills);
    }

    [Fact]
    public void UpdateRequiredSkills_WithInValidSkill_Throws()
    {
        var skills = new List<string> { "" };
        Assert.Throws<SkillValueCanNotBeEmpty>(() => Entity.UpdateRequiredSkills(skills));
    }

    [Fact]
    public void UpdateRequiredSkills_With_Duplicates_Throws()
    {
        var skills = new List<string> { "a", "a" };
        Assert.Throws<CourseAlreadyHasSkill>(() => Entity.UpdateRequiredSkills(skills));
    }

    [Fact]
    public void UpdateRequiredSkills_When_Confirmed_Throws()
        => Assert.Throws<CourseAlreadyConfirmed>(() =>
            Entity.UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a)
                .Confirm()
                .UpdateRequiredSkills(["one"]));
}
