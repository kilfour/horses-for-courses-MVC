using HorsesForCourses.Api.Coaches;
using HorsesForCourses.Api.Courses.GetCourseDetail;
using HorsesForCourses.Api.Courses.GetCourses;
using HorsesForCourses.Api.Warehouse;

namespace HorsesForCourses.Api.Courses;

public class CoursesRepository
{
    public CoursesRepository(
        IAmASuperVisor supervisor,
        IGetCourseById getCourseById,
        IGetCoachById getCoachById,
        IGetTheCourseSummaries getCourseSummaries,
        IGetTheCourseDetail getCourseDetail)
    {
        Supervisor = supervisor;
        GetCourseById = getCourseById;
        GetCoachById = getCoachById;
        GetCourseSummaries = getCourseSummaries;
        GetCourseDetail = getCourseDetail;
    }

    public IAmASuperVisor Supervisor { get; }
    public IGetCourseById GetCourseById { get; }
    public IGetCoachById GetCoachById { get; }
    public IGetTheCourseSummaries GetCourseSummaries { get; }
    public IGetTheCourseDetail GetCourseDetail { get; }
}