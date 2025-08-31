using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Courses.B_UpdateRequiredSkills;


public class A_UpdateRequiredSkillsApi : CoursesApiControllerTests
{
    [Fact]
    public async Task UpdateRequiredSkills_uses_the_service()
    {
        var response = await controller.UpdateRequiredSkills(TheCanonical.CourseId, TheCanonical.Skills);
        service.Verify(a => a.UpdateRequiredSkills(TheCanonical.CourseId, TheCanonical.Skills));
    }

    [Fact]
    public async Task UpdateRequiredSkills_Returns_NoContent()
    {
        service.Setup(a => a.UpdateRequiredSkills(TheCanonical.CourseId, TheCanonical.Skills)).ReturnsAsync(true);
        var response = await controller.UpdateRequiredSkills(TheCanonical.CourseId, TheCanonical.Skills);
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateRequiredSkills_Returns_Not_Found_If_No_Course()
    {
        var response = await controller.UpdateRequiredSkills(TheCanonical.BadId, TheCanonical.Skills);
        Assert.IsType<NotFoundResult>(response);
    }
}