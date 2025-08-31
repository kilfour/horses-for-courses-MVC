using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Service.Coaches.GetCoachById;
using HorsesForCourses.Tests.Tools;


namespace HorsesForCourses.Tests.Coaches;


public class GetCoachByIdTests : DatabaseTests
{
    private IdPrimitive IdAssignedByDb;

    private async Task<Coach?> Act()
        => await new GetCoachById(GetDbContext()).Load(IdAssignedByDb);

    [Fact]
    public async Task LoadIt()
    {
        IdAssignedByDb = AddToDb(TheCanonical.Coach());
        var result = await Act();
        Assert.NotNull(result);
        Assert.Equal(IdAssignedByDb, result.Id.Value);
        Assert.Equal(TheCanonical.CoachName, result.Name.Value);
    }

    [Fact]
    public async Task NotThere_Returns_Null()
        => Assert.Null(await Act());
}