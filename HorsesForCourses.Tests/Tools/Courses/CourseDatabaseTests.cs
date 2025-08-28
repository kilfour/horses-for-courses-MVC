using HorsesForCourses.Api.Warehouse;
using HorsesForCourses.Core.Domain.Courses;

namespace HorsesForCourses.Tests.Tools.Courses;

public class CourseDatabaseTests : TheDatabaseTest
{
    protected readonly Course Entity;

    public CourseDatabaseTests()
    {
        Entity = TheCanonical.Course();
        AddToDb(Entity);
    }

    protected Course Reload() => Reload(GetDbContext());
    protected Course Reload(AppDbContext dbContext) => dbContext.Courses.Single(a => a.Id == Entity.Id);
}