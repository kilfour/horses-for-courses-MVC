using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Service.Warehouse;
using Microsoft.EntityFrameworkCore;

namespace HorsesForCourses.Service.Coaches.GetCoachDetail;

public interface IGetCoachDetail
{
    Task<CoachDetail?> One(IdPrimitive id);
}

public class GetCoachDetail(AppDbContext dbContext) : IGetCoachDetail
{
    private readonly AppDbContext dbContext = dbContext;

    public async Task<CoachDetail?> One(IdPrimitive id)
    {
        var coachId = Id<Coach>.From(id);
        return await dbContext.Coaches
            .AsNoTracking()
            .Where(a => a.Id == coachId)
            .Select(a => new CoachDetail
            {
                Id = a.Id.Value,
                Name = a.Name.Value,
                Email = a.Email.Value,
                Skills = a.Skills.Select(a => a.Value).ToList(),
                Courses = a.AssignedCourses.Select(
                    b => new CoachDetail.CourseInfo(b.Id.Value, b.Name.Value)).ToList()
            }).SingleOrDefaultAsync();
    }
}