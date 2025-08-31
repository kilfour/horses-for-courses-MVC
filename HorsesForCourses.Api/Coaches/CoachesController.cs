using Microsoft.AspNetCore.Mvc;
using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Api.Abstract;

namespace HorsesForCourses.Api.Coaches;

[ApiController]
[Route("coaches")]
public class CoachesController(ICoachesService Service) : WebApiController
{
    [HttpPost]
    public async Task<IActionResult> RegisterCoach(RegisterCoachRequest request)
        => Ok(await Service.RegisterCoach(request.Name, request.Email));

    [HttpPost("{id}/skills")]
    public async Task<IActionResult> UpdateSkills(IdPrimitive id, UpdateSkillsRequest request)
        => NoContentNotFoundIfFalse(await Service.UpdateSkills(id, request.Skills));

    [HttpGet]
    public async Task<IActionResult> GetCoaches(int page = 1, int pageSize = 25)
        => Ok(await Service.GetCoaches(page, pageSize));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCoachDetail(IdPrimitive id)
        => OkNotFoundIfNull(await Service.GetCoachDetail(id));
}
