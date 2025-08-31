using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
using HorsesForCourses.MVC.Models.Courses;
using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Courses.E_AssignCoach;

public class C_AssignCoachMVC : CoursesMVCControllerTests
{
    [Fact]
    public async Task AssignCoach_GET_Passes_The_Model_To_The_View()
    {
        service.Setup(a => a.GetCourseDetail(TheCanonical.CourseId)).ReturnsAsync(
            TheCanonical.CourseDetail() with { Coach = new IdAndName(TheCanonical.CoachId, TheCanonical.CoachName) });
        var result = await controller.AssignCoach(TheCanonical.CourseId);
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<AssignCoachViewModel>(view.Model);
        Assert.Equal(TheCanonical.CourseName, model.Name);
        Assert.Equal(TheCanonical.CoachName, model.CoachName);
    }

    [Fact]
    public async Task AssignCoach_POST_calls_the_service()
    {
        await controller.AssignCoach(TheCanonical.CourseId, TheCanonical.CoachId);
        service.Verify(a => a.AssignCoach(TheCanonical.CourseId, TheCanonical.CoachId));
    }

    [Fact]
    public async Task AssignCoach_POST_Redirects_To_Index_On_Success()
    {
        service.Setup(a => a.GetCourseDetail(TheCanonical.CourseId)).ReturnsAsync(TheCanonical.CourseDetail());
        var result = await controller.AssignCoach(TheCanonical.CourseId, TheCanonical.CoachId);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(controller.Index), redirect.ActionName);
    }

    [Fact]
    public async Task AssignCoach_POST_Returns_View_On_Exception()
    {
        service.Setup(a => a.GetCourseDetail(TheCanonical.CourseId)).ReturnsAsync(TheCanonical.CourseDetail());
        service.Setup(a => a.AssignCoach(It.IsAny<IdPrimitive>(), It.IsAny<IdPrimitive>())).ThrowsAsync(new CourseAlreadyConfirmed());
        var result = await controller.AssignCoach(TheCanonical.CourseId);
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<AssignCoachViewModel>(view.Model);
        Assert.Equal(TheCanonical.CourseName, model.Name);
        Assert.Equal(string.Empty, model.CoachName);
    }

    [Fact]
    public async Task AssignCoach_POST_Returns_View_With_ModelError_On_Exception()
    {
        service.Setup(a => a.AssignCoach(It.IsAny<IdPrimitive>(), It.IsAny<IdPrimitive>())).ThrowsAsync(new CourseNotYetConfirmed());
        var result = await controller.AssignCoach(TheCanonical.BadId, TheCanonical.CoachId);
        Assert.False(controller.ModelState.IsValid);
        Assert.Contains(controller.ModelState, kvp => kvp.Value!.Errors.Any(e => e.ErrorMessage == "Course not yet confirmed."));
    }
}