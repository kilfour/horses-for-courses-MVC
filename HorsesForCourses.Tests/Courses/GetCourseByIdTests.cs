using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Service.Courses.GetCourseById;
using HorsesForCourses.Tests.Tools;


namespace HorsesForCourses.Tests.Courses;


public class GetCourseByIdTests : DatabaseTests
{
    private IdPrimitive IdAssignedByDb;
    private async Task<Course?> Act()
        => await new GetCourseById(GetDbContext()).Load(IdAssignedByDb);

    [Fact]
    public async Task LoadIt()
    {
        IdAssignedByDb = AddToDb(TheCanonical.Course());
        var result = await Act();
        Assert.NotNull(result);
        Assert.Equal(IdAssignedByDb, result.Id.Value);
        Assert.Equal(TheCanonical.CourseName, result.Name.Value);
    }

    [Fact]
    public async Task NotThere_Returns_Null()
        => Assert.Null(await Act());
}