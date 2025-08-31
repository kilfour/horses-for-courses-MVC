using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Coaches.D_GetCoachDetail;

public class A_GetCoachDetailApi : CoachesApiControllerTests
{
    [Fact]
    public async Task GetCoachDetail_uses_the_service()
    {
        var result = await controller.GetCoachDetail(TheCanonical.CoachId);
        service.Verify(a => a.GetCoachDetail(TheCanonical.CoachId));
    }

    [Fact]
    public async Task GetCoachDetail_Returns_Ok()
    {
        service.Setup(a => a.GetCoachDetail(TheCanonical.CoachId)).ReturnsAsync(TheCanonical.CoachDetail());
        var result = await controller.GetCoachDetail(TheCanonical.CoachId);
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<CoachDetail>(okResult!.Value);
    }

    [Fact]
    public async Task GetCoachDetail_Returns_Not_Found_If_No_Coach()
    {
        var response = await controller.GetCoachDetail(TheCanonical.BadId);
        Assert.IsType<NotFoundResult>(response);
    }
}