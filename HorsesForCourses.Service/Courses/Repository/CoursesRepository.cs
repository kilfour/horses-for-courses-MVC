using HorsesForCourses.Service.Coaches.GetCoachById;
using HorsesForCourses.Service.Courses.GetCourseById;
using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Service.Courses.GetCourses;
using HorsesForCourses.Service.Warehouse;

namespace HorsesForCourses.Service.Courses.Repository;

public record CoursesRepository(
    IAmASuperVisor Supervisor,
    IGetCourseById GetCourseById,
    IGetCoachById GetCoachById,
    IGetCourseSummaries GetCourseSummaries,
    IGetCourseDetail GetCourseDetail);