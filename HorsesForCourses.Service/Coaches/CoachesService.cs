using HorsesForCourses.Core.Domain.Coaches;

namespace HorsesForCourses.Service.Coaches;

public interface ICoachesService
{
    Task<int> RegisterCoach(string name, string email);
}

public class CoachesService(CoachesRepository repository) : ICoachesService
{
    private readonly CoachesRepository repository = repository;

    public async Task<int> RegisterCoach(string name, string email)
    {
        var coach = new Coach(name, email);
        await repository.Supervisor.Enlist(coach);
        await repository.Supervisor.Ship();
        return coach.Id.Value;
    }
}