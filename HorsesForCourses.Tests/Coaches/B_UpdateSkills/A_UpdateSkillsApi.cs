using HorsesForCourses.Api.Coaches.UpdateSkills;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;
using Microsoft.AspNetCore.Mvc;


namespace HorsesForCourses.Tests.Coaches.B_UpdateSkills;

public class A_UpdateSkillsApi : CoachesControllerTests
{
    private readonly UpdateSkillsRequest request;

    public A_UpdateSkillsApi()
    {
        request = new UpdateSkillsRequest(["one", "two"]);
    }

    [Fact]
    public async Task UpdateSkills_uses_the_query_object()
    {
        var response = await controller.UpdateSkills(TheCanonical.CoachId, request);
        coachQuery.Verify(a => a.Load(TheCanonical.CoachId));
    }

    [Fact]
    public async Task UpdateSkills_calls_update_skills()
    {
        await controller.UpdateSkills(TheCanonical.CoachId, request);
        Assert.True(spy.Called);
        Assert.Equal(request.Skills, spy.Seen);
    }

    [Fact]
    public async Task UpdateSkills_calls_supervisor_ship()
    {
        await controller.UpdateSkills(TheCanonical.CoachId, request);
        supervisor.Verify(a => a.Ship());
    }

    [Fact]
    public async Task UpdateSkills_ReturnsOk_WithValidId()
    {
        var response = await controller.UpdateSkills(TheCanonical.CoachId, request);
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateSkills_Returns_Not_Found_If_No_Coach()
    {
        var response = await controller.UpdateSkills(-1, request);
        Assert.IsType<NotFoundResult>(response);
    }
}