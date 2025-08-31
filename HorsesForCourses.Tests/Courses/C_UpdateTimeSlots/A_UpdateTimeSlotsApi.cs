using HorsesForCourses.Api.Courses;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Courses.C_UpdateTimeSlots;


public class A_UpdateTimeSlotsApi : CoursesApiControllerTests
{
    private readonly IEnumerable<TimeSlotRequest> request =
        TheCanonical.TimeSlotsRequestFullDayMonday();

    [Fact]
    public async Task UpdateTimeSlots_uses_the_service()
    {
        var response = await controller.UpdateTimeSlots(TheCanonical.CourseId, request);
        service.Verify(a =>
            a.UpdateTimeSlots(
                TheCanonical.CourseId,
                request, It.IsAny<Func<TimeSlotRequest, (CourseDay, int, int)>>()));
    }

    [Fact]
    public async Task UpdateTimeSlots_Returns_NoContent()
    {
        service.Setup(a =>
            a.UpdateTimeSlots(
                TheCanonical.CourseId,
                request, It.IsAny<Func<TimeSlotRequest, (CourseDay, int, int)>>())).ReturnsAsync(true);
        var response = await controller.UpdateTimeSlots(TheCanonical.CourseId, request);
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task UpdateTimeSlots_Returns_Not_Found_If_No_Course()
    {
        var response = await controller.UpdateTimeSlots(TheCanonical.BadId, request);
        Assert.IsType<NotFoundResult>(response);
    }
}