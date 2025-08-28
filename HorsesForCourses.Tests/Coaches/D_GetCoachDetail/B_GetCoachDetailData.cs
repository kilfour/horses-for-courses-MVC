using HorsesForCourses.Api.Coaches.GetCoachDetail;
using HorsesForCourses.Api.Coaches.GetCoaches;
using HorsesForCourses.Tests.Tools;


namespace HorsesForCourses.Tests.Coaches.D_GetCoachDetail;


public class B_GetCoachDetailData : TheDatabaseTest
{
    private async Task<CoachDetail?> Act()
        => await new GetCoachDetail(GetDbContext()).One(TheCanonical.CoachId);

    [Fact]
    public async Task With_Coach()
    {
        AddToDb(TheCanonical.Coach());
        var detail = await Act();
        Assert.NotNull(detail);
        Assert.Equal(1, detail.Id);
        Assert.Equal(TheCanonical.CoachName, detail.Name);
        Assert.Equal(TheCanonical.CoachEmail, detail.Email);
        Assert.Equal([], detail.Skills);
        Assert.Equal([], detail.Courses);
    }

    [Fact]
    public async Task NotThere_Returns_Null()
        => Assert.Null(await Act());
}