using HorsesForCourses.Service.Warehouse;
using HorsesForCourses.Service.Warehouse.Paging;
using Microsoft.EntityFrameworkCore;

namespace HorsesForCourses.Service.Coaches.GetCoaches;

public interface IGetTheCoachSummaries
{
    Task<PagedResult<CoachSummary>> Paged(PageRequest request);
}

public class GetCoachSummaries(AppDbContext dbContext) : IGetTheCoachSummaries
{
    private readonly AppDbContext dbContext = dbContext;

    public async Task<PagedResult<CoachSummary>> Paged(PageRequest request)
    {
        return await dbContext.Coaches
            .AsNoTracking()
            .OrderBy(p => p.Name).ThenBy(p => p.Id)
            .Select(p => new CoachSummary(
                p.Id.Value,
                p.Name,
                p.Email,
                p.AssignedCourses.Count))
            .ToPagedResultAsync(request);
    }
}