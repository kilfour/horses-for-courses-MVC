using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;
using HorsesForCourses.MVC.Models.Courses;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Courses.C_UpdateTimeSlots;

public class C_UpdateTimeSlotsMVC : CoursesMVCControllerTests
{
    [Fact]
    public async Task UpdateTimeSlots_GET_Passes_The_Model_To_The_View()
    {
        service.Setup(a => a.GetCourseDetail(TheCanonical.CourseId)).ReturnsAsync(TheCanonical.CourseDetail());
        var result = await controller.UpdateTimeSlots(TheCanonical.CourseId);
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UpdateTimeSlotsViewModel>(view.Model);
        Assert.Equal(TheCanonical.CourseName, model.Name);
        Assert.Equal([], model.TimeSlots);
    }

    [Fact]
    public async Task UpdateTimeSlots_POST_calls_the_service()
    {
        await controller.UpdateTimeSlots(TheCanonical.CourseId, TheCanonical.TimeSlotsFullDayMondayViewModel());
        service.Verify(a => a.UpdateTimeSlots(
            TheCanonical.CourseId,
            TheCanonical.TimeSlotsFullDayMondayViewModel(),
            It.IsAny<Func<TimeSlotViewModel, (CourseDay, int, int)>>()));
    }

    [Fact]
    public async Task UpdateTimeSlots_POST_Redirects_To_Index_On_Success()
    {
        service.Setup(a => a.GetCourseDetail(TheCanonical.CourseId)).ReturnsAsync(TheCanonical.CourseDetail());
        var result = await controller.UpdateTimeSlots(TheCanonical.CourseId, TheCanonical.TimeSlotsFullDayMondayViewModel());
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(controller.Index), redirect.ActionName);
    }

    [Fact]
    public async Task UpdateTimeSlots_POST_Returns_View_On_Exception()
    {
        service.Setup(a => a.GetCourseDetail(TheCanonical.CourseId)).ReturnsAsync(TheCanonical.CourseDetail());
        service.Setup(a => a.UpdateTimeSlots(
            It.IsAny<IdPrimitive>(),
            It.IsAny<IEnumerable<TimeSlotViewModel>>(),
            It.IsAny<Func<TimeSlotViewModel, (CourseDay, int, int)>>()))
            .ThrowsAsync(new OverlappingTimeSlots());
        var result = await controller.UpdateTimeSlots(TheCanonical.CourseId, TheCanonical.TimeSlotsWithDuplicateViewModel());
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UpdateTimeSlotsViewModel>(view.Model);
        Assert.Equal(TheCanonical.CourseName, model.Name);
        Assert.Equal([], model.TimeSlots);
    }

    [Fact]
    public async Task UpdateTimeSlots_POST_Returns_View_With_ModelError_On_Exception()
    {
        service.Setup(a => a.UpdateTimeSlots(
            It.IsAny<IdPrimitive>(),
            It.IsAny<IEnumerable<TimeSlotViewModel>>(),
            It.IsAny<Func<TimeSlotViewModel, (CourseDay, int, int)>>()))
            .ThrowsAsync(new OverlappingTimeSlots());
        var result = await controller.UpdateTimeSlots(TheCanonical.BadId, TheCanonical.TimeSlotsWithDuplicateViewModel());
        Assert.False(controller.ModelState.IsValid);
        Assert.Contains(controller.ModelState, kvp => kvp.Value!.Errors.Any(e => e.ErrorMessage == "Overlapping time slots."));
    }
}