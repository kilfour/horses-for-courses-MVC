using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Coaches.Repository;
using HorsesForCourses.Service.Warehouse.Paging;

namespace HorsesForCourses.Service.Coaches;

public interface ICoachesService
{
    Task<IdPrimitive> RegisterCoach(string name, string email);
    Task<bool> UpdateSkills(IdPrimitive id, IEnumerable<string> skills);
    Task<PagedResult<CoachSummary>> GetCoaches(int page, int pageSize);
    Task<CoachDetail?> GetCoachDetail(IdPrimitive id);
}

public class CoachesService(CoachesRepository Repository) : ICoachesService
{
    public async Task<IdPrimitive> RegisterCoach(string name, string email)
    {
        var coach = new Coach(name, email);
        await Repository.Supervisor.Enlist(coach);
        await Repository.Supervisor.Ship();
        return coach.Id.Value;
    }

    public async Task<bool> UpdateSkills(IdPrimitive id, IEnumerable<string> skills)
    {
        var coach = await Repository.GetCoachById.Load(id);
        if (coach == null) return false;
        coach.UpdateSkills(skills);
        await Repository.Supervisor.Ship();
        return true;
    }

    public async Task<PagedResult<CoachSummary>> GetCoaches(int page, int pageSize)
        => await Repository.GetCoachSummaries.Paged(new PageRequest(page, pageSize));

    public async Task<CoachDetail?> GetCoachDetail(IdPrimitive id)
        => await Repository.GetCoachDetail.One(id);
}