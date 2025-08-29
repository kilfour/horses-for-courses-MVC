using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;
using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Service.Courses.GetCourses;
using HorsesForCourses.Service.Warehouse.Paging;

namespace HorsesForCourses.Service.Courses;

public interface ICoursesService
{
    Task<int> CreateCourse(string name, DateOnly startDate, DateOnly endDate);
    Task<bool> UpdateRequiredSkills(int id, IEnumerable<string> skills);
    Task<bool> UpdateTimeSlots<T>(
        int id,
        IEnumerable<T> timeSlotInfo,
        Func<T, (CourseDay Day, int Start, int End)> getTimeSlot);
    Task<bool> ConfirmCourse(int id);
    Task<bool> AssignCoach(int courseId, int coachId);
    Task<PagedResult<CourseSummary>> GetCourses(int page, int pageSize);
    Task<CourseDetail?> GetCourseDetail(int id);
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

    public async Task<bool> UpdateTimeSlots<T>(
        int id,
        IEnumerable<T> timeSlotInfo,
        Func<T, (CourseDay Day, int Start, int End)> getTimeSlot)
    {
        var course = await Repository.GetCourseById.Load(id);
        if (course == null) return false;
        course.UpdateTimeSlots(timeSlotInfo, getTimeSlot);
        await Repository.Supervisor.Ship();
        return true;
    }

    public async Task<bool> ConfirmCourse(int id)
    {
        var course = await Repository.GetCourseById.Load(id);
        if (course == null) return false;
        course.Confirm();
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

    public async Task<PagedResult<CourseSummary>> GetCourses(int page, int pageSize)
        => await Repository.GetCourseSummaries.Paged(new PageRequest(page, pageSize));

    public async Task<CourseDetail?> GetCourseDetail(int id)
        => await Repository.GetCourseDetail.One(id);
}