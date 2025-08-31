using System.Reflection;
using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;
using Moq;
using QuickPulse.Show;

namespace HorsesForCourses.Tests.Coaches.A_RegisterCoach;

public class C_RegisterCoachService : CoachesServiceTests
{
    [Fact]
    public async Task RegisterCoach_delivers_the_coach_to_the_supervisor()
    {
        await service.RegisterCoach(TheCanonical.CoachName, TheCanonical.CoachEmail);
        supervisor.Verify(a => a.Enlist(
            It.Is<Coach>(a =>
                a.Name.Value == TheCanonical.CoachName &&
                a.Email.Value == TheCanonical.CoachEmail)));
        supervisor.Verify(a => a.Ship());
    }

    [Fact]
    public async Task RegisterCoach_Does_Not_Ship_On_Exception()
    {
        await Assert.ThrowsAnyAsync<DomainException>(async () => await service.RegisterCoach(string.Empty, string.Empty));
        supervisor.Verify(a => a.Ship(), Times.Never);
    }
}