using HorsesForCourses.MVC.Controllers;

namespace HorsesForCourses.Tests.Tools.Courses;

public abstract class CoursesMVCControllerTests : CoursesControllerTests
{
    protected readonly CoursesController controller;

    public CoursesMVCControllerTests()
    {
        controller = new CoursesController(service.Object);
    }
}
