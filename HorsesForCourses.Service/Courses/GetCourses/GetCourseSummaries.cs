using HorsesForCourses.Service.Warehouse;
using HorsesForCourses.Service.Warehouse.Paging;
using Microsoft.EntityFrameworkCore;

namespace HorsesForCourses.Service.Courses.GetCourses;

public interface IGetCourseSummaries
{
    Task<PagedResult<CourseSummary>> Paged(PageRequest request);
}

public class GetCourseSummaries(AppDbContext dbContext) : IGetCourseSummaries
{
    private readonly AppDbContext dbContext = dbContext;

    public async Task<PagedResult<CourseSummary>> Paged(PageRequest request)
    {
        return await dbContext.Courses
            .AsNoTracking()
            .OrderBy(p => p.Name).ThenBy(p => p.Id)
            .Select(p => new CourseSummary(
                p.Id.Value,
                p.Name,
                p.StartDate,
                p.EndDate,
                p.TimeSlots.Any(),
                p.AssignedCoach != null))
            .ToPagedResultAsync(request);
    }
}