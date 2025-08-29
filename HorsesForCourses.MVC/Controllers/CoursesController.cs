
using HorsesForCourses.MVC.Controllers.Abstract;
using HorsesForCourses.MVC.Models.Courses;
using HorsesForCourses.Service.Courses;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.MVC.Controllers;

public class CoursesController(ICoursesService Service) : MvcController
{
    [HttpGet]
    public async Task<IActionResult> CreateCourse()
        => await Task.Run(() => View(new CreateCourseViewModel("", null, null)));

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCourse(string name, DateOnly startDate, DateOnly endDate)
    {
        return await This(async () => await Service.CreateCourse(name, startDate, endDate))
            .OnSuccess(() => RedirectToAction(nameof(Index)))
            .OnException(() => View(new CreateCourseViewModel(name, startDate, endDate)));
    }

    [HttpGet("Courses/")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 25)
        => View(await Service.GetCourses(page, pageSize));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseDetail(int id)
        => ViewOrNotFoundIfNull(await Service.GetCourseDetail(id), a => a);
}


