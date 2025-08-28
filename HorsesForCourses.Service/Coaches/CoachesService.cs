using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Warehouse.Paging;

namespace HorsesForCourses.Service.Coaches;

public interface ICoachesService
{
    Task<int> RegisterCoach(string name, string email);
    Task<bool> UpdateSkills(int id, IEnumerable<string> skills);
    Task<PagedResult<CoachSummary>> GetCoaches(int page, int pageSize);
}

public class CoachesService(CoachesRepository Repository) : ICoachesService
{
    public async Task<int> RegisterCoach(string name, string email)
    {
        var coach = new Coach(name, email);
        await Repository.Supervisor.Enlist(coach);
        await Repository.Supervisor.Ship();
        return coach.Id.Value;
    }

    public async Task<bool> UpdateSkills(int id, IEnumerable<string> skills)
    {
        var coach = await Repository.GetCoachById.Load(id);
        if (coach == null) return false;
        coach.UpdateSkills(skills);
        await Repository.Supervisor.Ship();
        return true;
    }

    public async Task<PagedResult<CoachSummary>> GetCoaches(int page, int pageSize)
        => await Repository.GetTheCoachSummaries.All(new PageRequest(page, pageSize));
}