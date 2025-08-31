using HorsesForCourses.Api.Courses;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.Tests.Courses.A_CreateCourse;

public class A_CreateCourseApi : CoursesApiControllerTests
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
    public async Task CreateCourse_calls_the_service()
    {
        await Act();
        service.Verify(a => a.CreateCourse(TheCanonical.CourseName, TheCanonical.CourseStart, TheCanonical.CourseEnd));
        service.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateCourse_ReturnsOk_WithValidId()
    {
        var result = await Act();
        Assert.NotNull(result);
        Assert.IsType<IdPrimitive>(result!.Value);
    }
}
