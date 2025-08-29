using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
using HorsesForCourses.MVC.Models.Courses;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Courses.A_CreateCourse;

public class B_CreateCourseMVC : CoursesMVCControllerTests
{

    [Fact]
    public async Task CreateCourse_GET_Passes_The_Model_To_The_View()
    {
        var result = await controller.CreateCourse();
        var view = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<CreateCourseViewModel>(view.Model);
        Assert.Equal(string.Empty, viewModel.Name);
        Assert.Null(viewModel.StartDate);
        Assert.Null(viewModel.EndDate);
    }

    private async Task<IActionResult> CreateCourse_POST()
        => await controller.CreateCourse(TheCanonical.CourseName, TheCanonical.CourseStart, TheCanonical.CourseEnd);

    [Fact]
    public async Task CreateCourse_POST_Puts_The_Course_In_Storage()
    {
        await CreateCourse_POST();
        service.Verify(a => a.CreateCourse(TheCanonical.CourseName, TheCanonical.CourseStart, TheCanonical.CourseEnd));
    }

    [Fact]
    public async Task CreateCourse_POST_Redirects_To_Index_On_Success()
    {
        var result = await CreateCourse_POST();
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(controller.Index), redirect.ActionName);
    }

    [Fact]
    public async Task CreateCourse_POST_Returns_View_On_Exception()
    {
        service
            .Setup(a => a.CreateCourse("", TheCanonical.CourseStart, TheCanonical.CourseEnd))
            .ThrowsAsync(new CourseNameCanNotBeEmpty());
        var result = await controller.CreateCourse("", TheCanonical.CourseStart, TheCanonical.CourseEnd);
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<CreateCourseViewModel>(view.Model);
        Assert.Equal("", model.Name);
        Assert.Equal(TheCanonical.CourseStart, model.StartDate);
        Assert.Equal(TheCanonical.CourseEnd, model.EndDate);
    }

    [Fact]
    public async Task CreateCourse_POST_Returns_View_With_ModelError_On_Exception()
    {
        service
            .Setup(a => a.CreateCourse("", TheCanonical.CourseStart, TheCanonical.CourseEnd))
            .ThrowsAsync(new CourseNameCanNotBeEmpty());
        var result = await controller.CreateCourse("", TheCanonical.CourseStart, TheCanonical.CourseEnd);
        Assert.False(controller.ModelState.IsValid);
        Assert.Contains(controller.ModelState, kvp => kvp.Value!.Errors.Any(e => e.ErrorMessage == "Course name can not be empty."));
    }
}