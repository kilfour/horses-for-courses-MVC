using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Courses.G_GetCourseDetail;

public class A_GetCourseDetailApi : CoursesApiControllerTests
{
    [Fact]
    public async Task GetCourseDetail_uses_the_service()
    {
        var result = await controller.GetCourseDetail(TheCanonical.CourseId);
        service.Verify(a => a.GetCourseDetail(TheCanonical.CourseId));
    }

    [Fact]
    public async Task GetCourseDetailReturnsOk_With_Detail()
    {
        service.Setup(a => a.GetCourseDetail(TheCanonical.CourseId)).ReturnsAsync(new CourseDetail());
        var result = await controller.GetCourseDetail(TheCanonical.CourseId) as OkObjectResult;
        Assert.NotNull(result);
        Assert.IsType<CourseDetail>(result!.Value);
    }

    [Fact]
    public async Task GetCourseDetailReturns_NotFound_If_Course_Not_Present()
        => Assert.IsType<NotFoundResult>(await controller.GetCourseDetail(TheCanonical.BadId));
}