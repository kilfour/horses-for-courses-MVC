using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Warehouse.Paging;
using HorsesForCourses.Tests.Tools.Coaches;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Coaches.C_GetCoaches;

public class A_GetCoachesApi : CoachesApiControllerTests
{
    private async Task<OkObjectResult?> Act()
    {
        return await controller.GetCoaches() as OkObjectResult;
    }

    [Fact]
    public async Task GetCoaches_uses_the_query_object()
    {
        var response = await controller.GetCoaches();
        getCoachSummaries.Verify(a => a.All(It.IsAny<PageRequest>()));
    }

    [Fact]
    public async Task GetCoachesReturnsOk_With_List()
    {
        var result = await Act();
        Assert.NotNull(result);
        Assert.IsType<PagedResult<CoachSummary>>(result!.Value);
    }
}