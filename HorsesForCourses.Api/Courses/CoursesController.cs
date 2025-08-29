using HorsesForCourses.Api.Abstract;
using HorsesForCourses.Service.Courses;
using HorsesForCourses.Service.Warehouse.Paging;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.Api.Courses;

[ApiController]
[Route("courses")]
public class CoursesController(CoursesRepository Repository, ICoursesService Service) : WebApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateCourse(CreateCourseRequest request)
        => Ok(await Service.CreateCourse(request.Name, request.StartDate, request.EndDate));

    [HttpPost("{id}/skills")]
    public async Task<IActionResult> UpdateRequiredSkills(int id, IEnumerable<string> skills)
        => NoContentNotFoundIfFalse(await Service.UpdateRequiredSkills(id, skills));

    [HttpPost("{id}/timeslots")]
    public async Task<IActionResult> UpdateTimeSlots(int id, IEnumerable<TimeSlotRequest> timeSlots)
    {
        var course = await Repository.GetCourseById.Load(id);
        if (course == null) return NotFound();
        course.UpdateTimeSlots(timeSlots, a => (a.Day, a.Start, a.End));
        await Repository.Supervisor.Ship();
        return NoContent();
    }

    [HttpPost("{id}/confirm")]
    public async Task<IActionResult> ConfirmCourse(int id)
    {
        var course = await Repository.GetCourseById.Load(id);
        if (course == null) return NotFound();
        course.Confirm();
        await Repository.Supervisor.Ship();
        return NoContent();
    }


    [HttpPost("{id}/assign-coach")]
    public async Task<IActionResult> AssignCoach(int id, AssignCoachRequest request)
    {
        var course = await Repository.GetCourseById.Load(id);
        var coach = await Repository.GetCoachById.Load(request.CoachId);
        if (course == null || coach == null) return NotFound();
        course.AssignCoach(coach);
        await Repository.Supervisor.Ship();
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses(int page = 1, int pageSize = 25)
        => Ok(await Repository.GetCourseSummaries.Paged(new PageRequest(page, pageSize)));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseDetail(int id)
    {
        var courseDetail = await Repository.GetCourseDetail.One(id);
        if (courseDetail == null) return NotFound();
        return Ok(courseDetail);
    }
}