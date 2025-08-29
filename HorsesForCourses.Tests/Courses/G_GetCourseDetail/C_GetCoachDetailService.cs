using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Moq;

namespace HorsesForCourses.Tests.Courses.G_GetCourseDetail;

public class C_GetCourseDetailService : CoursesServiceTests
{
    [Fact]
    public async Task GetCourseDetail_uses_the_query_object()
    {
        await service.GetCourseDetail(TheCanonical.CourseId);
        getCourseDetail.Verify(a => a.One(TheCanonical.CourseId));
    }

    [Fact]
    public async Task GetCourseDetail_success_returns_detail()
    {
        var expected = TheCanonical.CourseDetail();
        getCourseDetail.Setup(a => a.One(TheCanonical.CourseId)).ReturnsAsync(expected);
        var result = await service.GetCourseDetail(TheCanonical.CourseId);
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetCourseDetail_failure_returns_null()
        => Assert.Null(await service.GetCourseDetail(TheCanonical.CourseId));
}