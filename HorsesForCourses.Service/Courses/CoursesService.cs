using HorsesForCourses.Core.Domain.Courses;

namespace HorsesForCourses.Service.Courses;

public interface ICoursesService
{
    Task<int> CreateCourse(string name, DateOnly startDate, DateOnly endDate);
    Task<bool> UpdateRequiredSkills(int id, IEnumerable<string> skills);
    Task<bool> AssignCoach(int courseId, int coachId);
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

    public async Task<bool> UpdateRequiredSkills(int id, IEnumerable<string> skills)
    {
        var course = await Repository.GetCourseById.Load(id);
        if (course == null) return false;
        course.UpdateRequiredSkills(skills);
        await Repository.Supervisor.Ship();
        return true;
    }

    public async Task<bool> AssignCoach(int courseId, int coachId)
    {
        var course = await Repository.GetCourseById.Load(courseId);
        var coach = await Repository.GetCoachById.Load(coachId);
        if (course == null || coach == null) return false;
        course.AssignCoach(coach);
        await Repository.Supervisor.Ship();
        return true;
    }
}