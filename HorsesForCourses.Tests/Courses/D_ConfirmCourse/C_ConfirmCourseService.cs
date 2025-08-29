using HorsesForCourses.Core.Domain;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Moq;

namespace HorsesForCourses.Tests.Courses.D_ConfirmCourse;

public class C_ConfirmCourseService : CoursesServiceTests
{
    [Fact]
    public async Task ConfirmCourse_uses_the_query_object()
    {
        await service.ConfirmCourse(TheCanonical.CourseId);
        getCourseById.Verify(a => a.Load(TheCanonical.CourseId));
    }

    [Fact]
    public async Task ConfirmCourse_calls_domain()
    {
        courseSpy.UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a);
        getCourseById.Setup(a => a.Load(TheCanonical.CourseId)).ReturnsAsync(courseSpy);
        await service.ConfirmCourse(TheCanonical.CourseId);
        Assert.True(courseSpy.IsConfirmed);
    }

    [Fact]
    public async Task ConfirmCourse_calls_supervisor_ship()
    {
        courseSpy.UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a);
        getCourseById.Setup(a => a.Load(TheCanonical.CourseId)).ReturnsAsync(courseSpy);
        await service.ConfirmCourse(TheCanonical.CourseId);
        supervisor.Verify(a => a.Ship());
    }

    [Fact]
    public async Task ConfirmCourse_Does_Not_Ship_On_Exception()
    {
        getCourseById.Setup(a => a.Load(TheCanonical.CourseId)).ReturnsAsync(courseSpy);
        await Assert.ThrowsAnyAsync<DomainException>(
            async () => await service.ConfirmCourse(TheCanonical.CourseId));
        supervisor.Verify(a => a.Ship(), Times.Never);
    }

    [Fact]
    public async Task ConfirmCourse_success_returns_true()
    {
        courseSpy.UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a);
        getCourseById.Setup(a => a.Load(TheCanonical.CourseId)).ReturnsAsync(courseSpy);
        var success = await service.ConfirmCourse(TheCanonical.CourseId);
        Assert.True(success);
    }

    [Fact]
    public async Task ConfirmCourse_failure_returns_false()
    {
        var success = await service.ConfirmCourse(TheCanonical.CourseId);
        Assert.False(success);
    }
}