using HorsesForCourses.Service.Courses;
using Moq;

namespace HorsesForCourses.Tests.Tools.Courses;

public abstract class CoursesControllerTests
{
    protected readonly Mock<ICoursesService> service;

    public CoursesControllerTests()
    {
        service = new Mock<ICoursesService>();
    }
}
