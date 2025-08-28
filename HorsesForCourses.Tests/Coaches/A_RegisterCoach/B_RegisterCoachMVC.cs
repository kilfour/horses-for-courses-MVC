using HorsesForCourses.Core.Domain.Coaches.InvalidationReasons;
using HorsesForCourses.MVC.Models.Coaches;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Coaches.A_RegisterCoach;

public class B_RegisterCoachMVC : CoachesMVCControllerTests
{
    [Fact]
    public async Task RegisterCoach_GET_Passes_The_Model_To_The_View()
    {
        var result = await controller.RegisterCoach();
        var view = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<RegisterCoachViewModel>(view.Model);
        Assert.Equal(string.Empty, viewModel.Name);
        Assert.Equal(string.Empty, viewModel.Email);
    }

    [Fact]
    public async Task RegisterCoach_POST_Puts_The_Coach_In_Storage()
    {
        await controller.RegisterCoach(TheCanonical.CoachName, TheCanonical.CoachEmail);
        service.Verify(a => a.RegisterCoach(TheCanonical.CoachName, TheCanonical.CoachEmail));
    }

    [Fact]
    public async Task RegisterCoach_POST_Redirects_To_Index_On_Success()
    {
        var result = await controller.RegisterCoach(TheCanonical.CoachName, TheCanonical.CoachEmail);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(controller.Index), redirect.ActionName);
    }

    [Fact]
    public async Task RegisterCoach_POST_Returns_View_On_Exception()
    {
        service
            .Setup(a => a.RegisterCoach(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new CoachNameCanNotBeEmpty());
        var result = await controller.RegisterCoach("", TheCanonical.CoachEmail);
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<RegisterCoachViewModel>(view.Model);
        Assert.Equal("", model.Name);
        Assert.Equal(TheCanonical.CoachEmail, model.Email);
    }

    [Fact]
    public async Task RegisterCoach_POST_Returns_View_With_ModelError_On_Exception()
    {
        service
            .Setup(a => a.RegisterCoach(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new CoachNameCanNotBeEmpty());
        await controller.RegisterCoach("", TheCanonical.CoachEmail);
        Assert.False(controller.ModelState.IsValid);
        Assert.Contains(controller.ModelState, kvp => kvp.Value!.Errors.Any(e => e.ErrorMessage == "Coach name can not be empty."));
    }
}