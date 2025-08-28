using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Coaches.D_GetCoachDetail;

public class A_GetCoachDetailApi : CoachesControllerTests
{
    [Fact]
    public async Task GetCoachDetail_uses_the_query_object()
    {
        var result = await controller.GetCoachDetail(TheCanonical.CoachId);
        getCoachDetail.Verify(a => a.One(It.IsAny<int>()));
    }

    [Fact]
    public async Task GetCoachDetailReturnsOk_With_List()
    {
        var result = await controller.GetCoachDetail(TheCanonical.CoachId);
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<CoachDetail>(okResult!.Value);
    }

    [Fact]
    public async Task GetCoachDetail_Returns_Not_Found_If_No_Coach()
    {
        var response = await controller.GetCoachDetail(-1);
        Assert.IsType<NotFoundResult>(response);
    }
}