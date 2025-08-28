using HorsesForCourses.Core.Domain;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.MVC.Models.Coaches;
using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Warehouse.Paging;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.MVC.Controllers;

public class CoachesController(CoachesRepository repository) : Controller
{
    private readonly CoachesRepository repository = repository;

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 25)
        => View(await repository.GetTheCoachSummaries.All(new PageRequest(page, pageSize)));

    [HttpGet]
    public async Task<IActionResult> RegisterCoach()
        => await Task.Run(() => View(new RegisterCoachViewModel()));

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterCoach(string name, string email)
    {
        try
        {
            var coach = new Coach(name, email);
            await repository.Supervisor.Enlist(coach);
            await repository.Supervisor.Ship();
            return RedirectToAction(nameof(Index));
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.MessageFromType());
            return View(new RegisterCoachViewModel(name, email));
        }
    }
}