using HorsesForCourses.Api.Courses.AssignCoach;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.Tests.Courses.E_AssignCoach;


public class A_AssignCoachApi : CoursesControllerTests
{
    protected override void ManipulateEntity(Course entity)
    {
        entity.UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a);
        entity.Confirm();
    }

    [Fact]
    public async Task AssignCoach_uses_the_query_objects()
    {
        await controller.AssignCoach(1, new AssignCoachRequest(1));
        getCourseById.Verify(a => a.Load(TheCanonical.CourseId));
        getCoachById.Verify(a => a.Load(TheCanonical.CoachId));
    }

    [Fact]
    public async Task AssignCoach_calls_domain()
    {
        await controller.AssignCoach(TheCanonical.CourseId, new AssignCoachRequest(TheCanonical.CoachId));
        Assert.True(spy.AssignCoachCalled);
        Assert.Equal("a", spy.AssignCoachSeen!.Name);
    }

    [Fact]
    public async Task AssignCoach_calls_supervisor_ship()
    {
        await controller.AssignCoach(TheCanonical.CourseId, new AssignCoachRequest(TheCanonical.CoachId));
        supervisor.Verify(a => a.Ship());
    }

    [Fact]
    public async Task AssignCoach_NoContent()
    {
        var response = await controller.AssignCoach(TheCanonical.CourseId, new AssignCoachRequest(TheCanonical.CoachId));
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateTimeSlots_Returns_Not_Found_If_No_Course()
    {
        var response = await controller.AssignCoach(-1, new AssignCoachRequest(TheCanonical.CoachId));
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task UpdateTimeSlots_Returns_Not_Found_If_No_Coach()
    {
        var response = await controller.AssignCoach(1, new AssignCoachRequest(-1));
        Assert.IsType<NotFoundResult>(response);
    }
}