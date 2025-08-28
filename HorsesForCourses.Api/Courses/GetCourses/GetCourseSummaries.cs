using HorsesForCourses.Api.Warehouse;
using HorsesForCourses.Api.Warehouse.Paging;
using Microsoft.EntityFrameworkCore;

namespace HorsesForCourses.Api.Courses.GetCourses;

public interface IGetTheCourseSummaries
{
    Task<PagedResult<CourseSummary>> All(PageRequest request);
}

public class GetCourseSummaries(AppDbContext dbContext) : IGetTheCourseSummaries
{
    private readonly AppDbContext dbContext = dbContext;

    public async Task<PagedResult<CourseSummary>> All(PageRequest request)
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