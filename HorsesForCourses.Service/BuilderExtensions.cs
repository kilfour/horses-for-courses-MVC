using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Coaches.GetCoachById;
using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Coaches.Repository;
using HorsesForCourses.Service.Courses;
using HorsesForCourses.Service.Courses.GetCourseById;
using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Service.Courses.GetCourses;
using HorsesForCourses.Service.Courses.Repository;
using HorsesForCourses.Service.Warehouse;
using Microsoft.Extensions.DependencyInjection;

namespace HorsesForCourses.Service;

public static class BuilderExtensions
{
    public static IServiceCollection AddHorsesForCourses(this IServiceCollection services)
        => services
            .AddScoped<IAmASuperVisor, DataSupervisor>()

            .AddScoped<IGetCoachById, GetCoachById>()
            .AddScoped<IGetCoachSummaries, GetCoachSummaries>()
            .AddScoped<IGetCoachDetail, GetCoachDetail>()

            .AddScoped<IGetCourseById, GetCourseById>()
            .AddScoped<IGetCourseSummaries, GetCourseSummaries>()
            .AddScoped<IGetCourseDetail, GetCourseDetail>()

            .AddScoped<CoachesRepository>()
            .AddScoped<CoursesRepository>()

            .AddScoped<ICoachesService, CoachesService>()
            .AddScoped<ICoursesService, CoursesService>();
}