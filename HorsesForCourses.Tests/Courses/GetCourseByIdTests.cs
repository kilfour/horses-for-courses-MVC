using HorsesForCourses.Api.Courses;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Tests.Tools;


namespace HorsesForCourses.Tests.Courses;


public class GetCourseByIdTests : TheDatabaseTest
{
    private async Task<Course?> Act()
        => await new GetCourseById(GetDbContext()).Load(TheCanonical.CourseId);

    [Fact]
    public async Task LoadIt()
    {
        AddToDb(TheCanonical.Course());
        var result = await Act();
        Assert.NotNull(result);
        Assert.Equal(1, result.Id.Value);
        Assert.Equal(TheCanonical.CourseName, result.Name);
    }

    [Fact]
    public async Task NotThere_Returns_Null()
        => Assert.Null(await Act());
}