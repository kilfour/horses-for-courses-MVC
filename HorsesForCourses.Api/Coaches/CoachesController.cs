using HorsesForCourses.Core.Domain.Coaches;
using Microsoft.AspNetCore.Mvc;
using HorsesForCourses.Api.Coaches.RegisterCoach;
using HorsesForCourses.Api.Coaches.UpdateSkills;
using HorsesForCourses.Api.Warehouse.Paging;

namespace HorsesForCourses.Api.Coaches;

[ApiController]
[Route("coaches")]
public class CoachesController(CoachesRepository repository) : ControllerBase
{
    private readonly CoachesRepository repository = repository;

    [HttpPost]
    public async Task<IActionResult> RegisterCoach(RegisterCoachRequest request)
    {
        var coach = new Coach(request.Name, request.Email);
        await repository.Supervisor.Enlist(coach);
        await repository.Supervisor.Ship();
        return Ok(coach.Id.Value);
    }

    [HttpPost("{id}/skills")]
    public async Task<IActionResult> UpdateSkills(int id, UpdateSkillsRequest request)
    {
        var coach = await repository.GetCoachById.Load(id);
        if (coach == null) return NotFound();
        coach.UpdateSkills(request.Skills);
        await repository.Supervisor.Ship();
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetCoaches(int page = 1, int pageSize = 25)
        => Ok(await repository.GetTheCoachSummaries.All(new PageRequest(page, pageSize)));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCoachDetail(int id)
    {
        var coachDetail = await repository.GetTheCoachDetail.One(id);
        if (coachDetail == null) return NotFound();
        return Ok(coachDetail);
    }
}
