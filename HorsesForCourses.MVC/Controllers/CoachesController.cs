using HorsesForCourses.MVC.Models.Coaches;
using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Warehouse.Paging;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.MVC.Controllers;

public class CoachesController(CoachesRepository Repository, ICoachesService Service) : MvcController
{
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 25)
        => View(await Repository.GetTheCoachSummaries.All(new PageRequest(page, pageSize)));

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
}


