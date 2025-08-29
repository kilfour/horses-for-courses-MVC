using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Courses;
using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Service.Courses.GetCourses;
using HorsesForCourses.Service.Warehouse;
using Moq;

namespace HorsesForCourses.Tests.Tools.Courses;

public abstract class CoursesServiceTests
{
    protected readonly ICoursesService service;
    protected readonly CoursesRepository repository;
    protected readonly Mock<IAmASuperVisor> supervisor;
    protected readonly Mock<IGetCourseById> getCourseById;
    protected readonly Mock<IGetCoachById> getCoachById;
    protected readonly Mock<IGetTheCourseSummaries> getCourseSummaries;
    protected readonly Mock<IGetTheCourseDetail> getCourseDetail;
    protected readonly CourseSpy courseSpy;

    public CoursesServiceTests()
    {
        getCourseDetail = new Mock<IGetTheCourseDetail>();
        getCourseSummaries = new Mock<IGetTheCourseSummaries>();
        courseSpy = new();
        getCourseById = new Mock<IGetCourseById>();
        getCoachById = new Mock<IGetCoachById>();

        supervisor = new Mock<IAmASuperVisor>();

        repository = new CoursesRepository(
           supervisor.Object,
           getCourseById.Object,
           getCoachById.Object,
           getCourseSummaries.Object,
           getCourseDetail.Object);

        service = new CoursesService(repository);
    }
}
