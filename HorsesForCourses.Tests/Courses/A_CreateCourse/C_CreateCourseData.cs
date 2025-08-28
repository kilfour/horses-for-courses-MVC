using HorsesForCourses.Service.Warehouse;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;


namespace HorsesForCourses.Tests.Courses.A_CreateCourse;

public class C_CreateCourseData : CourseDatabaseTests
{
    private readonly DataSupervisor supervisor;

    public C_CreateCourseData()
    {
        supervisor = new DataSupervisor(GetDbContext());
    }

    private async Task Act()
    {
        await supervisor.Enlist(Entity);
        await supervisor.Ship();
    }

    [Fact]
    public async Task Supervisor_Assigns_id()
    {
        await Act();
        Assert.NotEqual(default, Entity.Id.Value);
    }

    [Fact]
    public async Task Supervisor_Stores()
    {
        await Act();
        var reloaded = Reload();
        Assert.NotNull(reloaded);
        Assert.NotEqual(default, reloaded.Id.Value);
    }

    [Fact]
    public async Task Supervisor_persists_name_and_dates()
    {
        await Act();
        var reloaded = Reload();
        Assert.Equal(TheCanonical.CourseName, reloaded!.Name);
        Assert.Equal(TheCanonical.CourseStart, reloaded!.StartDate);
        Assert.Equal(TheCanonical.CourseEnd, reloaded!.EndDate);
    }
}
