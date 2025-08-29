
using HorsesForCourses.MVC.Controllers.Abstract;
using HorsesForCourses.MVC.Models.Coaches;
using HorsesForCourses.Service.Coaches;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.MVC.Controllers;

public class CoachesController(ICoachesService Service) : MvcController
{
    [HttpGet]
    public async Task<IActionResult> RegisterCoach()
        => await Task.Run(() => View(new RegisterCoachViewModel()));

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterCoach(string name, string email)
    {
        return await This(async () => await Service.RegisterCoach(name, email))
            .OnSuccess(() => RedirectToAction(nameof(Index)))
            .OnException(() => View(new RegisterCoachViewModel(name, email)));
    }

    [HttpGet("UpdateSkills/{id}")]
    public async Task<IActionResult> UpdateSkills(int id)
        => ViewOrNotFoundIfNull(await Service.GetCoachDetail(id), a => new UpdateSkillsViewModel(a!));

    [HttpPost("UpdateSkills/{id}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateSkills(int id, List<string> skills)
        => await This(async () => await Service.UpdateSkills(id, skills))
            .OnSuccess(() => RedirectToAction(nameof(Index)))
            .OnException(async () =>
                ViewOrNotFoundIfNull(await Service.GetCoachDetail(id), a => new UpdateSkillsViewModel(a!)));

    [HttpGet("Coaches/")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 25)
        => View(await Service.GetCoaches(page, pageSize));

    [HttpGet("Coaches/{id}")]
    public async Task<IActionResult> GetCoachDetail(int id)
        => ViewOrNotFoundIfNull(await Service.GetCoachDetail(id), a => a);
}


