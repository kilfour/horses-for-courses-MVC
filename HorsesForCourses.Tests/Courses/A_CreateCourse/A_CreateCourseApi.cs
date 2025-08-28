using HorsesForCourses.Api.Courses.CreateCourse;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Courses.A_CreateCourse;

public class A_CreateCourseApi : CoursesControllerTests
{
    private readonly CreateCourseRequest request;

    public A_CreateCourseApi()
    {
        request = new CreateCourseRequest(TheCanonical.CourseName, TheCanonical.CourseStart, TheCanonical.CourseEnd);
    }

    private async Task<OkObjectResult?> Act()
    {
        return await controller.CreateCourse(request)! as OkObjectResult;
    }

    [Fact]
    public async Task CreateCourse_delivers_to_the_supervisor()
    {
        await Act();
        supervisor.Verify(a => a.Enlist(It.Is<Course>(a => a.Name == "A")));
        supervisor.Verify(a => a.Ship());
    }

    [Fact]
    public async Task CreateCourse_ReturnsOk_WithValidId()
    {
        var result = await Act();
        Assert.NotNull(result);
        Assert.IsType<int>(result!.Value);
    }
}
