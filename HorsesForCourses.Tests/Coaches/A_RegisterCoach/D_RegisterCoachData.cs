using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Service.Warehouse;
using HorsesForCourses.Tests.Tools;

namespace HorsesForCourses.Tests.Coaches.A_RegisterCoach;

public class D_RegisterCoachData : DatabaseTests
{
    private readonly DataSupervisor supervisor;
    private readonly Coach coach;

    public D_RegisterCoachData()
    {
        supervisor = new DataSupervisor(GetDbContext());
        coach = TheCanonical.Coach();
    }

    private async Task Act()
    {
        await supervisor.Enlist(coach);
        await supervisor.Ship();
    }

    private Coach Reload()
        => GetDbContext().Coaches.Find(Id<Coach>.From(coach.Id.Value))!;

    [Fact]
    public async Task Supervisor_Assigns_id()
    {
        await Act();
        Assert.NotEqual(default, coach.Id.Value);
    }

    [Fact]
    public async Task Supervisor_Stores()
    {
        await Act();
        var reloaded = Reload();
        Assert.NotNull(reloaded);
        Assert.NotEqual(default, reloaded.Id.Value);
    }

    [Fact]
    public async Task Supervisor_name_and_email()
    {
        await Act();
        var reloaded = Reload();
        Assert.Equal(TheCanonical.CoachName, reloaded!.Name.Value);
        Assert.Equal(TheCanonical.CoachEmail, reloaded!.Email.Value);
    }
}
