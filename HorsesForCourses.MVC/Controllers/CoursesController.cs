
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
        => await This(async () => await Service.CreateCourse(name, startDate, endDate))
            .OnSuccess(() => RedirectToAction(nameof(Index)))
            .OnException(() => View(new CreateCourseViewModel(name, startDate, endDate)));

    [HttpGet("UpdateRequiredSkills/{id}")]
    public async Task<IActionResult> UpdateRequiredSkills(IdPrimitive id)
        => ViewOrNotFoundIfNull(await Service.GetCourseDetail(id), a => new UpdateRequiredSkillsViewModel(a!));

    [HttpPost("UpdateRequiredSkills/{id}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRequiredSkills(IdPrimitive id, List<string> skills)
        => await This(async () => await Service.UpdateRequiredSkills(id, skills))
            .OnSuccess(() => RedirectToAction(nameof(Index)))
            .OnException(async () =>
                ViewOrNotFoundIfNull(await Service.GetCourseDetail(id), a => new UpdateRequiredSkillsViewModel(a!)));

    [HttpGet("UpdateTimeSlots/{id}")]
    public async Task<IActionResult> UpdateTimeSlots(IdPrimitive id)
        => ViewOrNotFoundIfNull(await Service.GetCourseDetail(id), a => new UpdateTimeSlotsViewModel(a!));

    [HttpPost("UpdateTimeSlots/{id}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateTimeSlots(IdPrimitive id, IEnumerable<TimeSlotViewModel> timeSlots)
        => await This(async () => await Service.UpdateTimeSlots(id, timeSlots, a => (a.Day, a.Start, a.End)))
            .OnSuccess(() => RedirectToAction(nameof(Index)))
            .OnException(async () =>
                ViewOrNotFoundIfNull(await Service.GetCourseDetail(id), a => new UpdateTimeSlotsViewModel(a!)));

    [HttpGet("ConfirmCourse/{id}")]
    public async Task<IActionResult> GetConfirmCourseInfo(IdPrimitive id)
        => ViewOrNotFoundIfNull(await Service.GetCourseDetail(id), a => new ConfirmCourseViewModel(a!));

    [HttpPost("ConfirmCourse/{id}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmCourse(IdPrimitive id)
        => await This(async () => await Service.ConfirmCourse(id))
            .OnSuccess(() => RedirectToAction(nameof(Index)))
            .OnException(async () =>
                ViewOrNotFoundIfNull(await Service.GetCourseDetail(id), a => new ConfirmCourseViewModel(a!)));

    [HttpGet("AssignCoach/{id}")]
    public async Task<IActionResult> AssignCoach(IdPrimitive id)
        => ViewOrNotFoundIfNull(await Service.GetCourseDetail(id), a => new AssignCoachViewModel(a!));

    [HttpPost("AssignCoach/{id}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignCoach(IdPrimitive id, IdPrimitive coachId)
        => await This(async () => await Service.AssignCoach(id, coachId))
            .OnSuccess(() => RedirectToAction(nameof(Index)))
            .OnException(async () =>
                ViewOrNotFoundIfNull(await Service.GetCourseDetail(id), a => new AssignCoachViewModel(a!)));

    [HttpGet("Courses/")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 25)
        => View(await Service.GetCourses(page, pageSize));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseDetail(IdPrimitive id)
        => ViewOrNotFoundIfNull(await Service.GetCourseDetail(id), a => a);
}


