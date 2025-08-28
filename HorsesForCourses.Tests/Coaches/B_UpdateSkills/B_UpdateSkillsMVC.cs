using HorsesForCourses.Core.Domain.Coaches.InvalidationReasons;
using HorsesForCourses.MVC.Models.Coaches;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Coaches.B_UpdateSkills;

public class B_UpdateSkillsMVC : CoachesMVCControllerTests
{
    [Fact]
    public async Task UpdateSkills_GET_Passes_The_Model_To_The_View()
    {
        var result = await controller.UpdateSkills();
        var view = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<UpdateSkillsViewModel>(view.Model);
        Assert.Equal([], viewModel.Skills);
    }

    [Fact]
    public async Task UpdateSkills_POST_calls_the_service()
    {
        await controller.UpdateSkills(TheCanonical.CoachId, TheCanonical.Skills);
        service.Verify(a => a.UpdateSkills(TheCanonical.CoachId, TheCanonical.Skills));
    }

    [Fact]
    public async Task UpdateSkills_POST_Redirects_To_Index_On_Success()
    {
        var result = await controller.UpdateSkills(TheCanonical.CoachId, TheCanonical.Skills);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(controller.Index), redirect.ActionName);
    }

    [Fact]
    public async Task UpdateSkills_POST_Returns_View_On_Exception()
    {
        service.Setup(a => a.UpdateSkills(It.IsAny<int>(), It.IsAny<List<string>>()))
            .ThrowsAsync(new CoachAlreadyHasSkill("one"));
        var result = await controller.UpdateSkills(-1, ["one", "one"]);
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UpdateSkillsViewModel>(view.Model);
        Assert.Equal(["one", "one"], model.Skills);
    }

    [Fact]
    public async Task UpdateSkills_POST_Returns_View_With_ModelError_On_Exception()
    {
        service.Setup(a => a.UpdateSkills(It.IsAny<int>(), It.IsAny<List<string>>()))
            .ThrowsAsync(new CoachAlreadyHasSkill("one"));
        var result = await controller.UpdateSkills(-1, ["one", "one"]);
        Assert.False(controller.ModelState.IsValid);
        Assert.Contains(controller.ModelState, kvp => kvp.Value!.Errors.Any(e => e.ErrorMessage == "Coach already has skill."));
    }
}