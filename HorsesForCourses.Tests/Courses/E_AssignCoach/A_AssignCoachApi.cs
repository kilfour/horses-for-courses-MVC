using HorsesForCourses.Api.Courses;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Courses.E_AssignCoach;


public class A_AssignCoachApi : CoursesApiControllerTests
{
    [Fact]
    public async Task AssignCoach_uses_the_service()
    {
        await controller.AssignCoach(TheCanonical.CourseId, new AssignCoachRequest(TheCanonical.CoachId));
        service.Verify(a => a.AssignCoach(TheCanonical.CourseId, TheCanonical.CoachId));
    }

    [Fact]
    public async Task AssignCoach_NoContent()
    {
        service.Setup(a => a.AssignCoach(TheCanonical.CourseId, TheCanonical.CoachId)).ReturnsAsync(true);
        var response = await controller.AssignCoach(TheCanonical.CourseId, new AssignCoachRequest(TheCanonical.CoachId));
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateTimeSlots_Returns_Not_Found_If_No_Course()
    {
        var response = await controller.AssignCoach(TheCanonical.BadId, new AssignCoachRequest(TheCanonical.CoachId));
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task UpdateTimeSlots_Returns_Not_Found_If_No_Coach()
    {
        var response = await controller.AssignCoach(TheCanonical.CourseId, new AssignCoachRequest(TheCanonical.BadId));
        Assert.IsType<NotFoundResult>(response);
    }
}