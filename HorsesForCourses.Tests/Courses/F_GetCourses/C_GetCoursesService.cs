using HorsesForCourses.Service.Warehouse.Paging;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Moq;

namespace HorsesForCourses.Tests.Courses.F_GetCourses;

public class C_GetCoursesService : CoursesServiceTests
{
    [Fact]
    public async Task GetCourses_uses_the_query_object()
    {
        await service.GetCourses(1, 25);
        getCourseSummaries.Verify(a => a.Paged(It.IsAny<PageRequest>()));
    }

    [Fact]
    public async Task GetCourses_success_returns_list_of_summaries()
    {
        var expected = TheCanonical.CourseSummaryList();
        getCourseSummaries.Setup(a => a.Paged(It.IsAny<PageRequest>())).ReturnsAsync(expected);
        var result = await service.GetCourses(1, 25);
        Assert.Equal(expected, result);
    }
}