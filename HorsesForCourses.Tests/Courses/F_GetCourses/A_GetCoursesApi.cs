using HorsesForCourses.Service.Courses.GetCourses;
using HorsesForCourses.Service.Warehouse.Paging;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Courses.F_GetCourses;

public class A_GetCoursesApi : CoursesApiControllerTests
{
    private async Task<OkObjectResult?> Act()
    {
        return await controller.GetCourses() as OkObjectResult;
    }

    [Fact]
    public async Task GetCourses_uses_the_query_object()
    {
        var response = await controller.GetCourses();
        service.Verify(a => a.GetCourses(1, 25));
    }

    [Fact]
    public async Task GetCourses_Returns_Ok_With_List()
    {
        service.Setup(a => a.GetCourses(1, 25)).ReturnsAsync(TheCanonical.CourseSummaryList());
        var result = await Act();
        Assert.NotNull(result);
        Assert.IsType<PagedResult<CourseSummary>>(result!.Value);
    }
}