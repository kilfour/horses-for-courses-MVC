using HorsesForCourses.Api.Courses.GetCourseDetail;
using HorsesForCourses.Tests.Tools;


namespace HorsesForCourses.Tests.Courses.G_GetCourseDetail;


public class B_GetCourseDetailData : TheDatabaseTest
{
    private async Task<CourseDetail?> Act()
        => await new GetCourseDetail(GetDbContext()).One(TheCanonical.CourseId);

    [Fact]
    public async Task With_Course()
    {
        AddToDb(TheCanonical.Course());
        var detail = await Act();
        Assert.NotNull(detail);
        Assert.Equal(1, detail.Id);
        Assert.Equal(TheCanonical.CourseName, detail.Name);
        Assert.Equal(TheCanonical.CourseStart, detail.Start);
        Assert.Equal(TheCanonical.CourseEnd, detail.End);
        Assert.Equal([], detail.Skills);
        Assert.Null(detail.Coach);
    }

    [Fact]
    public async Task NotThere_Returns_Null()
        => Assert.Null(await Act());
}