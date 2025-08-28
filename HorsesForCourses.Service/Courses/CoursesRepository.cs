using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Service.Courses.GetCourses;
using HorsesForCourses.Service.Warehouse;

namespace HorsesForCourses.Service.Courses;

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