using HorsesForCourses.Service.Courses.GetCourses;
using HorsesForCourses.Service.Warehouse.Paging;
using HorsesForCourses.Tests.Tools;


namespace HorsesForCourses.Tests.Courses.F_GetCourses;


public class D_GetCoursesData : DatabaseTests
{
    private async Task<PagedResult<CourseSummary>> Act(PageRequest request)
        => await new GetCourseSummaries(GetDbContext()).Paged(request);

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
    public async Task With_Course()
    {
        var id = AddToDb(TheCanonical.Course());
        var result = await Act(new PageRequest());
        Assert.Single(result.Items);

        var summary = result.Items.Single();
        Assert.Equal(id, summary.Id);
        Assert.Equal(TheCanonical.CourseName, summary.Name);
        Assert.Equal(TheCanonical.CourseStart, summary.StartDate);
        Assert.Equal(TheCanonical.CourseEnd, summary.EndDate);
        Assert.False(summary.HasSchedule);
        Assert.False(summary.HasCoach);

        Assert.Equal(1, result.TotalCount);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(25, result.PageSize);
        Assert.Equal(1, result.TotalPages);
        Assert.False(result.HasPrevious);
        Assert.False(result.HasNext);
    }
}