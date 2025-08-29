using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Service.Courses.GetCourseById;
using HorsesForCourses.Tests.Tools;


namespace HorsesForCourses.Tests.Courses;


public class GetCourseByIdTests : DatabaseTests
{
    private const int ExpectedIdAssignedByDb = 1;
    private async Task<Course?> Act()
        => await new GetCourseById(GetDbContext()).Load(ExpectedIdAssignedByDb);

    [Fact]
    public async Task LoadIt()
    {
        AddToDb(TheCanonical.Course());
        var result = await Act();
        Assert.NotNull(result);
        Assert.Equal(ExpectedIdAssignedByDb, result.Id.Value);
        Assert.Equal(TheCanonical.CourseName, result.Name);
    }

    [Fact]
    public async Task NotThere_Returns_Null()
        => Assert.Null(await Act());
}