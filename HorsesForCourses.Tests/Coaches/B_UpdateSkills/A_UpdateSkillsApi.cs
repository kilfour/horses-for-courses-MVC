using HorsesForCourses.Api.Coaches;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace HorsesForCourses.Tests.Coaches.B_UpdateSkills;

public class A_UpdateSkillsApi : CoachesApiControllerTests
{
    private readonly UpdateSkillsRequest request;

    public A_UpdateSkillsApi()
    {
        request = new UpdateSkillsRequest(TheCanonical.Skills);
    }

    [Fact]
    public async Task UpdateSkills_uses_the_service()
    {
        var response = await controller.UpdateSkills(TheCanonical.CoachId, request);
        service.Verify(a => a.UpdateSkills(TheCanonical.CoachId, It.Is<List<string>>(a => a == request.Skills)));
    }

    [Fact]
    public async Task UpdateSkills_Service_Success_Returns_NoContent()
    {
        service.Setup(a => a.UpdateSkills(TheCanonical.CoachId, request.Skills)).ReturnsAsync(true);
        var response = await controller.UpdateSkills(TheCanonical.CoachId, request);
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateSkills_Service_Failure_Returns_Not_Found()
    {
        service.Setup(a => a.UpdateSkills(TheCanonical.CoachId, request.Skills)).ReturnsAsync(false);
        var response = await controller.UpdateSkills(TheCanonical.BadId, request);
        Assert.IsType<NotFoundResult>(response);
    }
}