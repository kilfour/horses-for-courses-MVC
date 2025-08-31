using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Service.Warehouse;

namespace HorsesForCourses.Service.Courses.GetCourseById;

public interface IGetCourseById
{
    Task<Course?> Load(IdPrimitive id);
}

public class GetCourseById(AppDbContext dbContext) : IGetCourseById
{
    private readonly AppDbContext dbContext = dbContext;

    public async Task<Course?> Load(IdPrimitive id) =>
        await dbContext.FindAsync<Course>(Id<Course>.From(id));
}