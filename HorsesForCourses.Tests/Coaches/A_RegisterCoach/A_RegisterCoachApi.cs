using HorsesForCourses.Api.Coaches.RegisterCoach;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Coaches.A_RegisterCoach;

public class A_RegisterCoachApi : CoachesControllerTests
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
    public async Task RegisterCoach_delivers_the_coach_request_as_coach_to_the_supervisor()
    {
        await Act();
        supervisor.Verify(a => a.Enlist(
            It.Is<Coach>(a =>
                a.Name == TheCanonical.CoachName &&
                a.Email == TheCanonical.CoachEmail)));
        supervisor.Verify(a => a.Ship());
    }

    [Fact]
    public async Task RegisterCoach_ReturnsOk_WithValidId()
    {
        var result = await Act();
        Assert.NotNull(result);
        Assert.IsType<int>(result!.Value);
    }
}