using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Service.Warehouse;
using Microsoft.EntityFrameworkCore;

namespace HorsesForCourses.Service.Courses.GetCourseDetail;

public interface IGetCourseDetail
{
    Task<CourseDetail?> One(IdPrimitive id);
}

public class GetCourseDetail(AppDbContext dbContext) : IGetCourseDetail
{
    private readonly AppDbContext dbContext = dbContext;

    public async Task<CourseDetail?> One(IdPrimitive id)
    {
        var courseId = Id<Course>.From(id);
        return await dbContext.Courses
            .AsNoTracking()
            .Where(a => a.Id == courseId)
            .Select(a => new CourseDetail
            {
                Id = a.Id.Value,
                Name = a.Name.Value,
                Start = a.Period.Start,
                End = a.Period.End,
                Skills = a.RequiredSkills.Select(b => b.Value),
                TimeSlots = a.TimeSlots.Select(b => new CourseDetail.TimeSlotInfo(b.Day, b.Start.Value, b.End.Value)),
                IsConfirmed = a.IsConfirmed,
                Coach = a.AssignedCoach == null ? null : new IdAndName(a.AssignedCoach.Id.Value, a.AssignedCoach.Name.Value)

            }).SingleOrDefaultAsync();
    }
}