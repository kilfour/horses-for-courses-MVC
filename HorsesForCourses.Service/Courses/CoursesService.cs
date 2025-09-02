using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;
using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Service.Courses.GetCourses;
using HorsesForCourses.Service.Courses.Repository;
using HorsesForCourses.Service.Warehouse.Paging;

namespace HorsesForCourses.Service.Courses;

public interface ICoursesService
{
    Task<IdPrimitive> CreateCourse(string name, DateOnly startDate, DateOnly endDate);
    Task<bool> UpdateRequiredSkills(IdPrimitive id, IEnumerable<string> skills);
    Task<bool> UpdateTimeSlots<T>(
        IdPrimitive id,
        IEnumerable<T> timeSlotInfo,
        Func<T, (CourseDay Day, int Start, int End)> getTimeSlot);
    Task<bool> ConfirmCourse(IdPrimitive id);
    Task<bool> AssignCoach(IdPrimitive courseId, IdPrimitive coachId);
    Task<PagedResult<CourseSummary>> GetCourses(int page, int pageSize);
    Task<CourseDetail?> GetCourseDetail(IdPrimitive id);
}

public class CoursesService(CoursesRepository Repository) : ICoursesService
{
    public async Task<IdPrimitive> CreateCourse(string name, DateOnly start, DateOnly end)
    {
        var course = new Course(name, start, end);
        await Repository.Supervisor.Enlist(course);
        await Repository.Supervisor.Ship();
        return course.Id.Value;
    }

    public async Task<bool> UpdateRequiredSkills(IdPrimitive id, IEnumerable<string> skills)
    {
        var course = await Repository.GetCourseById.Load(id);
        if (course == null) return false;
        course.UpdateRequiredSkills(skills);
        await Repository.Supervisor.Ship();
        return true;
    }

    public async Task<bool> UpdateTimeSlots<T>(
        IdPrimitive id,
        IEnumerable<T> timeSlotInfo,
        Func<T, (CourseDay Day, int Start, int End)> getTimeSlot)
    {
        var course = await Repository.GetCourseById.Load(id);
        if (course == null) return false;
        course.UpdateTimeSlots(timeSlotInfo, getTimeSlot);
        await Repository.Supervisor.Ship();
        return true;
    }

    public async Task<bool> ConfirmCourse(IdPrimitive id)
    {
        var course = await Repository.GetCourseById.Load(id);
        if (course == null) return false;
        course.Confirm();
        await Repository.Supervisor.Ship();
        return true;
    }

    public async Task<bool> AssignCoach(IdPrimitive courseId, IdPrimitive coachId)
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

    public async Task<CourseDetail?> GetCourseDetail(IdPrimitive id)
        => await Repository.GetCourseDetail.One(id);
}