using HorsesForCourses.Api.Warehouse;
using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Courses;
using Microsoft.EntityFrameworkCore;

namespace HorsesForCourses.Api.Courses.GetCourseDetail;

public interface IGetTheCourseDetail
{
    Task<CourseDetail?> One(int id);
}

public class GetCourseDetail(AppDbContext dbContext) : IGetTheCourseDetail
{
    private readonly AppDbContext dbContext = dbContext;

    public async Task<CourseDetail?> One(int id)
    {
        var courseId = Id<Course>.From(id);
        return await dbContext.Courses
            .AsNoTracking()
            .Where(a => a.Id == courseId)
            .Select(a => new CourseDetail
            {
                Id = a.Id.Value,
                Name = a.Name,
                Start = a.StartDate,
                End = a.EndDate,
                Skills = a.RequiredSkills.Select(b => b.Value),
                TimeSlots = a.TimeSlots.Select(b => new CourseDetail.TimeSlotInfo(b.Day, b.Start.Value, b.End.Value)),
                Coach = a.AssignedCoach == null ? null : new IdAndName(a.AssignedCoach.Id.Value, a.AssignedCoach.Name)

            }).SingleOrDefaultAsync();
    }
}