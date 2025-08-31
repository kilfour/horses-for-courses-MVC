using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Service.Warehouse;

namespace HorsesForCourses.Service.Coaches.GetCoachById;

public interface IGetCoachById
{
    Task<Coach?> Load(IdPrimitive id);
}

public class GetCoachById : IGetCoachById
{
    private readonly AppDbContext dbContext;

    public GetCoachById(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Coach?> Load(IdPrimitive id)
    {
        return await dbContext.FindAsync<Coach>(Id<Coach>.From(id));
    }
}