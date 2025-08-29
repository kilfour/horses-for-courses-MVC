using HorsesForCourses.Core.Domain.Courses;

namespace HorsesForCourses.Service.Courses;

public interface ICoursesService
{
    Task<int> CreateCourse(string name, DateOnly startDate, DateOnly endDate);
}

public class CoursesService(CoursesRepository Repository) : ICoursesService
{
    public async Task<int> CreateCourse(string name, DateOnly startDate, DateOnly endDate)
    {
        var course = new Course(name, startDate, endDate);
        await Repository.Supervisor.Enlist(course);
        await Repository.Supervisor.Ship();
        return course.Id.Value;
    }
}