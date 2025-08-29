using HorsesForCourses.Core.Domain;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Moq;

namespace HorsesForCourses.Tests.Courses.C_UpdateTimeSlots;

public class C_UpdateTimeSlotsService : CoursesServiceTests
{
    [Fact]
    public async Task UpdateTimeSlots_uses_the_query_object()
    {
        await service.UpdateTimeSlots(TheCanonical.CourseId, TheCanonical.TimeSlotsFullDayMonday(), a => a);
        getCourseById.Verify(a => a.Load(TheCanonical.CourseId));
    }

    [Fact]
    public async Task UpdateTimeSlots_calls_domain()
    {
        getCourseById.Setup(a => a.Load(TheCanonical.CourseId)).ReturnsAsync(courseSpy);
        await service.UpdateTimeSlots(TheCanonical.CourseId, TheCanonical.TimeSlotsFullDayMonday(), a => a);
        Assert.True(courseSpy.TimeSlotsCalled);
        Assert.Equal(TheCanonical.TimeSlotsFullDayMondayExpected(), courseSpy.TimeSlotsSeen);
    }

    [Fact]
    public async Task UpdateTimeSlots_calls_supervisor_ship()
    {
        getCourseById.Setup(a => a.Load(TheCanonical.CourseId)).ReturnsAsync(courseSpy);
        await service.UpdateTimeSlots(TheCanonical.CourseId, TheCanonical.TimeSlotsFullDayMonday(), a => a);
        supervisor.Verify(a => a.Ship());
    }

    [Fact]
    public async Task UpdateTimeSlots_Does_Not_Ship_On_Exception()
    {
        var invalidSlots = TheCanonical.TimeSlotsFullDayMonday().Concat(TheCanonical.TimeSlotsFullDayMonday());
        getCourseById.Setup(a => a.Load(TheCanonical.CourseId)).ReturnsAsync(courseSpy);
        await Assert.ThrowsAnyAsync<DomainException>(
            async () => await service.UpdateTimeSlots(TheCanonical.CourseId, invalidSlots, a => a));
        supervisor.Verify(a => a.Ship(), Times.Never);
    }

    [Fact]
    public async Task UpdateTimeSlots_success_returns_true()
    {
        getCourseById.Setup(a => a.Load(TheCanonical.CourseId)).ReturnsAsync(courseSpy);
        var success = await service.UpdateTimeSlots(TheCanonical.CourseId, TheCanonical.TimeSlotsFullDayMonday(), a => a);
        Assert.True(success);
    }

    [Fact]
    public async Task UpdateTimeSlots_failure_returns_false()
    {
        var success = await service.UpdateTimeSlots(TheCanonical.CourseId, TheCanonical.TimeSlotsFullDayMonday(), a => a);
        Assert.False(success);
    }
}