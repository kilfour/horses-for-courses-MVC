using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Courses.G_GetCourseDetail;

public class A_GetCourseDetailApi : CoursesControllerTests
{

    [Fact]
    public async Task GetCourseDetail_uses_the_query_object()
    {
        var result = await controller.GetCourseDetail(TheCanonical.CourseId);
        getCourseDetail.Verify(a => a.One(TheCanonical.CourseId));
    }

    [Fact]
    public async Task GetCourseDetailReturnsOk_With_List()
    {
        getCourseDetail.Setup(a => a.One(TheCanonical.CourseId)).ReturnsAsync(new CourseDetail());
        var result = await controller.GetCourseDetail(1) as OkObjectResult;
        Assert.NotNull(result);
        Assert.IsType<CourseDetail>(result!.Value);
    }

    [Fact]
    public async Task GetCourseDetailReturns_NotFound_If_Course_Not_Present()
    {
        var result = await controller.GetCourseDetail(-1);
        Assert.IsType<NotFoundResult>(result);
    }
}