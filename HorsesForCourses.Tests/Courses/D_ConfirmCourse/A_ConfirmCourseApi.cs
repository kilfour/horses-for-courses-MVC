using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Courses.D_ConfirmCourse;


public class A_ConfirmCourseApi : CoursesApiControllerTests
{
    [Fact]
    public async Task ConfirmCourse_uses_the_service()
    {
        await controller.ConfirmCourse(TheCanonical.CourseId);
        service.Verify(a => a.ConfirmCourse(TheCanonical.CourseId));
    }

    [Fact]
    public async Task ConfirmCourse_NoContent()
    {
        service.Setup(a => a.ConfirmCourse(TheCanonical.CourseId)).ReturnsAsync(true);
        var response = await controller.ConfirmCourse(TheCanonical.CourseId);
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateTimeSlots_Returns_Not_Found_If_No_Course()
    {
        var response = await controller.ConfirmCourse(TheCanonical.BadId);
        Assert.IsType<NotFoundResult>(response);
    }
}