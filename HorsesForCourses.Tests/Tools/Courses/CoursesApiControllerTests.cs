using HorsesForCourses.Api.Courses;

namespace HorsesForCourses.Tests.Tools.Courses;

public abstract class CoursesApiControllerTests : CoursesControllerTests
{
    protected readonly CoursesController controller;

    public CoursesApiControllerTests()
    {
        controller = new CoursesController(service.Object);
    }
}