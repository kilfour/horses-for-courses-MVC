using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Courses;
using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Service.Courses.GetCourses;
using HorsesForCourses.Service.Warehouse;
using Microsoft.Extensions.DependencyInjection;

namespace HorsesForCourses.Service;

public static class BuilderExtensions
{
    public static IServiceCollection AddHorsesForCourses(this IServiceCollection services)
        => services
            .AddScoped<IAmASuperVisor, DataSupervisor>()

            .AddScoped<IGetCoachById, GetCoachById>()
            .AddScoped<IGetTheCoachSummaries, GetCoachSummaries>()
            .AddScoped<IGetTheCoachDetail, GetCoachDetail>()

            .AddScoped<IGetCourseById, GetCourseById>()
            .AddScoped<IGetTheCourseSummaries, GetCourseSummaries>()
            .AddScoped<IGetTheCourseDetail, GetCourseDetail>()

            .AddScoped<CoachesRepository>()
            .AddScoped<CoursesRepository>()

            .AddScoped<ICoachesService, CoachesService>()
            .AddScoped<ICoursesService, CoursesService>();
}