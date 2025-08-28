using HorsesForCourses.Api.Coaches;
using HorsesForCourses.Api.Courses;
using HorsesForCourses.Api.Courses.GetCourseDetail;
using HorsesForCourses.Api.Courses.GetCourses;
using HorsesForCourses.Api.Warehouse;
using HorsesForCourses.Api.Warehouse.Paging;
using HorsesForCourses.Core.Domain.Courses;
using Moq;

namespace HorsesForCourses.Tests.Tools.Courses;

public abstract class CoursesControllerTests
{
    protected readonly CoursesController controller;
    protected readonly CoursesRepository repository;
    protected readonly Mock<IAmASuperVisor> supervisor;
    protected readonly Mock<IGetCourseById> getCourseById;
    protected readonly Mock<IGetCoachById> getCoachById;
    protected readonly Mock<IGetTheCourseSummaries> getCourseSummaries;
    protected readonly Mock<IGetTheCourseDetail> getCourseDetail;

    protected readonly CourseSpy spy;

    public CoursesControllerTests()
    {
        getCourseDetail = new Mock<IGetTheCourseDetail>();

        getCourseSummaries = new Mock<IGetTheCourseSummaries>();
        getCourseSummaries.Setup(a => a.All(It.IsAny<PageRequest>()))
            .ReturnsAsync(new PagedResult<CourseSummary>([], 0, 1, 15));

        spy = new();
        ManipulateEntity(spy);
        getCourseById = new Mock<IGetCourseById>();
        getCourseById.Setup(a => a.Load(TheCanonical.CourseId)).ReturnsAsync(spy);

        getCoachById = new Mock<IGetCoachById>();
        getCoachById.Setup(a => a.Load(TheCanonical.CoachId)).ReturnsAsync(TheCanonical.Coach());

        supervisor = new Mock<IAmASuperVisor>();

        repository = new CoursesRepository(
           supervisor.Object,
           getCourseById.Object,
           getCoachById.Object,
           getCourseSummaries.Object,
           getCourseDetail.Object);

        controller = new CoursesController(repository);
    }

    protected virtual void ManipulateEntity(Course entity) { }
}