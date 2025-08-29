using System.Text.Json.Serialization;
using HorsesForCourses.Service.Warehouse;
using Microsoft.EntityFrameworkCore;

namespace HorsesForCourses.MVC;

public static class BuilderExtensions
{
    public static IServiceCollection AddHorsesForCoursesMVC(this IServiceCollection services)
        => services

            .AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=horsesforcourses.db"))

            .AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter(null, allowIntegerValues: false)))

            .Services;
}