using HorsesForCourses.Api.Courses.AssignCoach;
using HorsesForCourses.Api.Courses.CreateCourse;
using HorsesForCourses.Api.Courses.UpdateTimeSlots;
using HorsesForCourses.Api.Warehouse.Paging;
using HorsesForCourses.Core.Domain.Courses;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.Api.Courses;

[ApiController]
[Route("courses")]
public class CoursesController(CoursesRepository repository) : ControllerBase
{
    private readonly CoursesRepository repository = repository;

    [HttpPost]
    public async Task<IActionResult> CreateCourse(CreateCourseRequest request)
    {
        var course = new Course(request.Name, request.StartDate, request.EndDate);
        await repository.Supervisor.Enlist(course);
        await repository.Supervisor.Ship();
        return Ok(course.Id.Value);
    }

    [HttpPost("{id}/skills")]
    public async Task<IActionResult> UpdateRequiredSkills(int id, IEnumerable<string> skills)
    {
        var course = await repository.GetCourseById.Load(id);
        if (course == null) return NotFound();
        course.UpdateRequiredSkills(skills);
        await repository.Supervisor.Ship();
        return NoContent();
    }

    [HttpPost("{id}/timeslots")]
    public async Task<IActionResult> UpdateTimeSlots(int id, IEnumerable<TimeSlotRequest> timeSlots)
    {
        var course = await repository.GetCourseById.Load(id);
        if (course == null) return NotFound();
        course.UpdateTimeSlots(timeSlots, a => (a.Day, a.Start, a.End));
        await repository.Supervisor.Ship();
        return NoContent();
    }

    [HttpPost("{id}/confirm")]
    public async Task<IActionResult> ConfirmCourse(int id)
    {
        var course = await repository.GetCourseById.Load(id);
        if (course == null) return NotFound();
        course.Confirm();
        await repository.Supervisor.Ship();
        return NoContent();
    }


    [HttpPost("{id}/assign-coach")]
    public async Task<IActionResult> AssignCoach(int id, AssignCoachRequest request)
    {
        var course = await repository.GetCourseById.Load(id);
        var coach = await repository.GetCoachById.Load(request.CoachId);
        if (course == null || coach == null) return NotFound();
        course.AssignCoach(coach);
        await repository.Supervisor.Ship();
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses(int page = 1, int pageSize = 25)
        => Ok(await repository.GetCourseSummaries.All(new PageRequest(page, pageSize)));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseDetail(int id)
    {
        var courseDetail = await repository.GetCourseDetail.One(id);
        if (courseDetail == null) return NotFound();
        return Ok(courseDetail);
    }
}