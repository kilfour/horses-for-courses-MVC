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
}