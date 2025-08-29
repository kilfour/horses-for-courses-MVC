using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Service.Warehouse;

namespace HorsesForCourses.Tests.Tools.Courses;

public class CourseDatabaseTests : DatabaseTests
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