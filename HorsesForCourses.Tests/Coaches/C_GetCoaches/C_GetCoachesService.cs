using HorsesForCourses.Service.Warehouse.Paging;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;
using Moq;

namespace HorsesForCourses.Tests.Coaches.C_GetCoaches;

public class C_GetCoachesService : CoachesServiceTests
{
    [Fact]
    public async Task GetCoaches_uses_the_query_object()
    {
        await service.GetCoaches(1, 25);
        getCoachSummaries.Verify(a => a.Paged(It.IsAny<PageRequest>()));
    }

    [Fact]
    public async Task GetCoaches_success_returns_list_of_summaries()
    {
        var expected = TheCanonical.CoachSummaryList();
        getCoachSummaries.Setup(a => a.Paged(It.IsAny<PageRequest>())).ReturnsAsync(expected);
        var result = await service.GetCoaches(1, 25);
        Assert.Equal(expected, result);
    }
}