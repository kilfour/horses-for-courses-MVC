
using HorsesForCourses.MVC.Controllers;
using HorsesForCourses.Tests.Tools.Courses;

namespace HorsesForCourses.Tests.Tools.Coaches;

public abstract class CoursesMVCControllerTests : CoursesControllerTests
{
    protected readonly CoursesController controller;

    public CoursesMVCControllerTests()
    {
        controller = new CoursesController(service.Object);
    }
}
