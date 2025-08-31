using HorsesForCourses.Core.Domain;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;
using Moq;

namespace HorsesForCourses.Tests.Coaches.B_UpdateSkills;

public class C_UpdateSkillsService : CoachesServiceTests
{
    [Fact]
    public async Task UpdateSkills_uses_the_query_object()
    {
        var response = await service.UpdateSkills(TheCanonical.CoachId, TheCanonical.Skills);
        getCoachById.Verify(a => a.Load(TheCanonical.CoachId));
    }

    [Fact]
    public async Task UpdateSkills_calls_update_skills()
    {
        getCoachById.Setup(a => a.Load(TheCanonical.CoachId)).ReturnsAsync(spy);
        await service.UpdateSkills(TheCanonical.CoachId, TheCanonical.Skills);
        Assert.True(spy.UpdateSkillsCalled);
        Assert.Equal(TheCanonical.Skills, spy.UpdateSkillsSeen);
    }

    [Fact]
    public async Task UpdateSkills_calls_supervisor_ship()
    {
        getCoachById.Setup(a => a.Load(TheCanonical.CoachId)).ReturnsAsync(spy);
        await service.UpdateSkills(TheCanonical.CoachId, TheCanonical.Skills);
        supervisor.Verify(a => a.Ship());
    }

    [Fact]
    public async Task UpdateSkills_Does_Not_Ship_On_Exception()
    {
        getCoachById.Setup(a => a.Load(TheCanonical.CoachId)).ReturnsAsync(spy);
        await Assert.ThrowsAnyAsync<DomainException>(
            async () => await service.UpdateSkills(TheCanonical.CoachId, ["a", "a"]));
        supervisor.Verify(a => a.Ship(), Times.Never);
    }

    [Fact]
    public async Task UpdateSkills_success_returns_true()
    {
        getCoachById.Setup(a => a.Load(TheCanonical.CoachId)).ReturnsAsync(spy);
        var success = await service.UpdateSkills(TheCanonical.CoachId, TheCanonical.Skills);
        Assert.True(success);
    }

    [Fact]
    public async Task UpdateSkills_failure_returns_false()
    {
        var success = await service.UpdateSkills(TheCanonical.BadId, TheCanonical.Skills);
        Assert.False(success);
    }
}