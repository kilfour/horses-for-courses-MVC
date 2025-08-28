using HorsesForCourses.Api.Courses.UpdateTimeSlots;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.Tests.Courses.C_UpdateTimeSlots;


public class A_UpdateTimeSlotsApi : CoursesControllerTests
{
    private readonly IEnumerable<TimeSlotRequest> request =
        TheCanonical.TimeSlotsRequestFullDayMonday();

    [Fact]
    public async Task UpdateTimeSlots_uses_the_query_object()
    {
        var response = await controller.UpdateTimeSlots(TheCanonical.CourseId, request);
        getCourseById.Verify(a => a.Load(TheCanonical.CourseId));
    }

    [Fact]
    public async Task UpdateTimeSlots_calls_domain_entity()
    {
        await controller.UpdateTimeSlots(TheCanonical.CourseId, request);
        Assert.True(spy.TimeSlotsCalled);
        Assert.Equal(TheCanonical.TimeSlotsFullDayMondayExpected(), spy.TimeSlotsSeen);
    }

    [Fact]
    public async Task UpdateTimeSlots_calls_supervisor_ship()
    {
        await controller.UpdateTimeSlots(TheCanonical.CourseId, request);
        supervisor.Verify(a => a.Ship());
    }

    [Fact]
    public async Task UpdateTimeSlots_Returns_NoContent()
    {
        var response = await controller.UpdateTimeSlots(TheCanonical.CourseId, request);
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateTimeSlots_Returns_Not_Found_If_No_Course()
    {
        var response = await controller.UpdateTimeSlots(-1, request);
        Assert.IsType<NotFoundResult>(response);
    }
}