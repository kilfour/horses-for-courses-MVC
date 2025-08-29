using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Tests.Tools;


namespace HorsesForCourses.Tests.Coaches;


public class GetCoachByIdTests : DatabaseTests
{
    private const int ExpectedIdAssignedByDb = 1;
    private async Task<Coach?> Act()
        => await new GetCoachById(GetDbContext()).Load(ExpectedIdAssignedByDb);

    [Fact]
    public async Task LoadIt()
    {
        AddToDb(TheCanonical.Coach());
        var result = await Act();
        Assert.NotNull(result);
        Assert.Equal(ExpectedIdAssignedByDb, result.Id.Value);
        Assert.Equal(TheCanonical.CoachName, result.Name);
    }

    [Fact]
    public async Task NotThere_Returns_Null()
        => Assert.Null(await Act());
}