using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Tests.Tools;


namespace HorsesForCourses.Tests.Coaches.D_GetCoachDetail;


public class D_GetCoachDetailData : DatabaseTests
{
    private IdPrimitive IdAssignedByDb;

    private async Task<CoachDetail?> Act()
        => await new GetCoachDetail(GetDbContext()).One(IdAssignedByDb);

    [Fact]
    public async Task With_Coach()
    {
        IdAssignedByDb = AddToDb(TheCanonical.Coach());
        var detail = await Act();
        Assert.NotNull(detail);
        Assert.Equal(IdAssignedByDb, detail.Id);
        Assert.Equal(TheCanonical.CoachName, detail.Name);
        Assert.Equal(TheCanonical.CoachEmail, detail.Email);
        Assert.Equal([], detail.Skills);
        Assert.Equal([], detail.Courses);
    }

    [Fact]
    public async Task NotThere_Returns_Null()
        => Assert.Null(await Act());
}