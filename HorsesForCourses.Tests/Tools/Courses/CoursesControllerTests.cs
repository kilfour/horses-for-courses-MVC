using HorsesForCourses.Api.Courses;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Courses;
using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Service.Courses.GetCourses;
using HorsesForCourses.Service.Warehouse;
using HorsesForCourses.Service.Warehouse.Paging;
using Moq;

namespace HorsesForCourses.Tests.Tools.Courses;

public abstract class CoursesControllerTests
{
    protected readonly CoursesController controller;
    protected readonly Mock<ICoursesService> service;
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
        getCourseSummaries.Setup(a => a.Paged(It.IsAny<PageRequest>()))
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
        service = new Mock<ICoursesService>();

        controller = new CoursesController(repository, service.Object);
    }

    protected virtual void ManipulateEntity(Course entity) { }
}