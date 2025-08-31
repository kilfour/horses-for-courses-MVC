using HorsesForCourses.Api.Coaches;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.Tests.Coaches.A_RegisterCoach;

public class A_RegisterCoachApi : CoachesApiControllerTests
{
    private readonly RegisterCoachRequest request;

    public A_RegisterCoachApi()
    {
        request = new RegisterCoachRequest(TheCanonical.CoachName, TheCanonical.CoachEmail);
    }

    private async Task<OkObjectResult?> Act()
    {
        return await controller.RegisterCoach(request)! as OkObjectResult;
    }

    [Fact]
    public async Task RegisterCoach_delivers_the_coach_request_as_coach_to_the_service()
    {
        await Act();
        service.Verify(a => a.RegisterCoach(TheCanonical.CoachName, TheCanonical.CoachEmail));
    }

    [Fact]
    public async Task RegisterCoach_ReturnsOk_WithValidId()
    {
        var result = await Act();
        Assert.NotNull(result);
        Assert.IsType<IdPrimitive>(result!.Value);
    }
}
