using HorsesForCourses.Api.Warehouse;
using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Coaches;

namespace HorsesForCourses.Api.Coaches;

public interface IGetCoachById
{
    Task<Coach?> Load(int id);
}

public class GetCoachById : IGetCoachById
{
    private readonly AppDbContext dbContext;

    public GetCoachById(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Coach?> Load(int id)
    {
        return await dbContext.FindAsync<Coach>(Id<Coach>.From(id));
    }
}