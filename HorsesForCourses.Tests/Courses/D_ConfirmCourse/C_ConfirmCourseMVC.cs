using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
using HorsesForCourses.MVC.Models.Courses;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Courses.D_ConfirmCourse;

public class C_ConfirmCourseMVC : CoursesMVCControllerTests
{

    [Fact]
    public async Task ConfirmCourse_GET_Passes_The_Model_To_The_View()
    {
        service.Setup(a => a.GetCourseDetail(TheCanonical.CourseId))
            .ReturnsAsync(TheCanonical.CourseDetail() with { IsConfirmed = true });
        var result = await controller.GetConfirmCourseInfo(TheCanonical.CourseId);
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ConfirmCourseViewModel>(view.Model);
        Assert.Equal(TheCanonical.CourseName, model.Name);
        Assert.True(model.IsConfirmed);
    }

    [Fact]
    public async Task ConfirmCourse_POST_calls_the_service()
    {
        await controller.ConfirmCourse(TheCanonical.CourseId);
        service.Verify(a => a.ConfirmCourse(TheCanonical.CourseId));
    }

    [Fact]
    public async Task ConfirmCourse_POST_Redirects_To_Index_On_Success()
    {
        service.Setup(a => a.GetCourseDetail(TheCanonical.CourseId)).ReturnsAsync(TheCanonical.CourseDetail());
        var result = await controller.ConfirmCourse(TheCanonical.CourseId);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(controller.Index), redirect.ActionName);
    }

    [Fact]
    public async Task ConfirmCourse_POST_Returns_View_On_Exception()
    {
        service.Setup(a => a.GetCourseDetail(TheCanonical.CourseId)).ReturnsAsync(TheCanonical.CourseDetail());
        service.Setup(a => a.ConfirmCourse(It.IsAny<IdPrimitive>())).ThrowsAsync(new CourseAlreadyConfirmed());
        var result = await controller.ConfirmCourse(TheCanonical.CourseId);
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ConfirmCourseViewModel>(view.Model);
        Assert.Equal(TheCanonical.CourseName, model.Name);
        Assert.False(model.IsConfirmed);
    }

    [Fact]
    public async Task ConfirmCourse_POST_Returns_View_With_ModelError_On_Exception()
    {
        service.Setup(a => a.ConfirmCourse(It.IsAny<IdPrimitive>())).ThrowsAsync(new CourseAlreadyConfirmed());
        var result = await controller.ConfirmCourse(TheCanonical.BadId);
        Assert.False(controller.ModelState.IsValid);
        Assert.Contains(controller.ModelState, kvp => kvp.Value!.Errors.Any(e => e.ErrorMessage == "Course already confirmed."));
    }
}