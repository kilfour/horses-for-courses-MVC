using HorsesForCourses.Service.Courses.GetCourses;
using HorsesForCourses.Service.Warehouse.Paging;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Courses.F_GetCourses;

public class A_GetCoursesApi : CoursesControllerTests
{
    private async Task<OkObjectResult?> Act()
    {
        return await controller.GetCourses() as OkObjectResult;
    }

    [Fact]
    public async Task UpdateSkills_uses_the_query_object()
    {
        var response = await controller.GetCourses();
        getCourseSummaries.Verify(a => a.All(It.IsAny<PageRequest>()));
    }

    [Fact]
    public async Task GetCoursesReturnsOk_With_List()
    {
        var result = await Act();
        Assert.NotNull(result);
        Assert.IsType<PagedResult<CourseSummary>>(result!.Value);
    }
}