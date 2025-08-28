using HorsesForCourses.Api.Coaches.GetCoaches;
using HorsesForCourses.Service.Warehouse.Paging;
using HorsesForCourses.Tests.Tools;


namespace HorsesForCourses.Tests.Coaches.C_GetCoaches;


public class B_GetCoachesData : TheDatabaseTest
{
    private async Task<PagedResult<CoachSummary>> Act(PageRequest request)
        => await new GetCoachSummaries(GetDbContext()).All(request);

    [Fact]
    public async Task EmptyList()
    {
        var result = await Act(new PageRequest());
        Assert.Equal([], result.Items);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(25, result.PageSize);
        Assert.Equal(0, result.TotalPages);
        Assert.False(result.HasPrevious);
        Assert.False(result.HasNext);
    }

    [Fact]
    public async Task With_Coach()
    {
        AddToDb(TheCanonical.Coach());
        var result = await Act(new PageRequest());
        Assert.Single(result.Items);

        var summary = result.Items.Single();
        Assert.Equal(1, summary.Id);
        Assert.Equal(TheCanonical.CoachName, summary.Name);
        Assert.Equal(TheCanonical.CoachEmail, summary.Email);
        Assert.Equal(0, summary.NumberOfCoursesAssignedTo);

        Assert.Equal(1, result.TotalCount);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(25, result.PageSize);
        Assert.Equal(1, result.TotalPages);
        Assert.False(result.HasPrevious);
        Assert.False(result.HasNext);
    }
}