using HorsesForCourses.Core.Domain;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Moq;

namespace HorsesForCourses.Tests.Courses.B_UpdateRequiredSkills;

public class C_UpdateRequiredSkillsService : CoursesServiceTests
{
    private readonly IEnumerable<string> skills = ["one", "two"];

    [Fact]
    public async Task UpdateRequiredSkills_uses_the_query_object()
    {
        await service.UpdateRequiredSkills(TheCanonical.CourseId, skills);
        getCourseById.Verify(a => a.Load(TheCanonical.CourseId));
    }

    [Fact]
    public async Task UpdateRequiredSkills_calls_domain()
    {
        getCourseById.Setup(a => a.Load(TheCanonical.CourseId)).ReturnsAsync(courseSpy);
        await service.UpdateRequiredSkills(TheCanonical.CourseId, skills);
        Assert.True(courseSpy.RequiredSkillsCalled);
        Assert.Equal(skills, courseSpy.RequiredSkillsSeen);
    }

    [Fact]
    public async Task UpdateRequiredSkills_calls_supervisor_ship()
    {
        getCourseById.Setup(a => a.Load(TheCanonical.CourseId)).ReturnsAsync(courseSpy);
        await service.UpdateRequiredSkills(TheCanonical.CourseId, skills);
        supervisor.Verify(a => a.Ship());
    }

    [Fact]
    public async Task UpdateRequiredSkills_Does_Not_Ship_On_Exception()
    {
        getCourseById.Setup(a => a.Load(TheCanonical.CourseId)).ReturnsAsync(courseSpy);
        await Assert.ThrowsAnyAsync<DomainException>(
            async () => await service.UpdateRequiredSkills(TheCanonical.CourseId, ["one", "one"]));
        supervisor.Verify(a => a.Ship(), Times.Never);
    }

    [Fact]
    public async Task UpdateRequiredSkills_success_returns_true()
    {
        getCourseById.Setup(a => a.Load(TheCanonical.CourseId)).ReturnsAsync(courseSpy);
        var success = await service.UpdateRequiredSkills(TheCanonical.CourseId, skills);
        Assert.True(success);
    }

    [Fact]
    public async Task UpdateRequiredSkills_failure_returns_false()
    {
        var success = await service.UpdateRequiredSkills(-1, skills);
        Assert.False(success);
    }
}