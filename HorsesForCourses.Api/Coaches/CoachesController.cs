using Microsoft.AspNetCore.Mvc;
using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Warehouse.Paging;

namespace HorsesForCourses.Api.Coaches;

[ApiController]
[Route("coaches")]
public class CoachesController(CoachesRepository repository, ICoachesService service) : ControllerBase
{
    private readonly CoachesRepository repository = repository;
    private readonly ICoachesService service = service;

    [HttpPost]
    public async Task<IActionResult> RegisterCoach(RegisterCoachRequest request)
    {
        var id = await service.RegisterCoach(request.Name, request.Email);
        return Ok(id);
    }

    [HttpPost("{id}/skills")]
    public async Task<IActionResult> UpdateSkills(int id, UpdateSkillsRequest request)
    {
        var success = await service.UpdateSkills(id, request.Skills);
        return success ? NoContent() : NotFound();
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
