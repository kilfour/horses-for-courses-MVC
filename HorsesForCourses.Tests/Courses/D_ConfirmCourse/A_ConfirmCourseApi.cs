using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.Tests.Courses.D_ConfirmCourse;


public class A_ConfirmCourseApi : CoursesControllerTests
{
    private readonly List<string> request = ["one", "two"];

    protected override void ManipulateEntity(Course entity)
    {
        entity.UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a);
    }

    [Fact]
    public async Task ConfirmCourse_uses_the_query_object()
    {
        await controller.ConfirmCourse(TheCanonical.CourseId);
        getCourseById.Verify(a => a.Load(TheCanonical.CourseId));
    }

    [Fact]
    public async Task ConfirmCourse_calls_confirm()
    {
        await controller.ConfirmCourse(TheCanonical.CourseId);
        Assert.True(spy.IsConfirmed);
    }

    [Fact]
    public async Task ConfirmCourse_calls_supervisor_ship()
    {
        await controller.ConfirmCourse(TheCanonical.CourseId);
        supervisor.Verify(a => a.Ship());
    }

    [Fact]
    public async Task ConfirmCourse_NoContent()
    {
        var response = await controller.ConfirmCourse(TheCanonical.CourseId);
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateTimeSlots_Returns_Not_Found_If_No_Course()
    {
        var response = await controller.ConfirmCourse(-1);
        Assert.IsType<NotFoundResult>(response);
    }
}