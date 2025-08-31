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
        service.Setup(a => a.GetCoachDetail(TheCanonical.CoachId)).ReturnsAsync(TheCanonical.CoachDetail());
        var result = await controller.UpdateSkills(TheCanonical.CoachId);
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UpdateSkillsViewModel>(view.Model);
        Assert.Equal(TheCanonical.CoachName, model.Name);
        Assert.Equal(TheCanonical.CoachEmail, model.Email);
        Assert.Equal([], model.Skills);
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
        service.Setup(a => a.GetCoachDetail(TheCanonical.CoachId)).ReturnsAsync(TheCanonical.CoachDetail());
        var result = await controller.UpdateSkills(TheCanonical.CoachId, TheCanonical.Skills);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(controller.Index), redirect.ActionName);
    }

    [Fact]
    public async Task UpdateSkills_POST_Returns_View_On_Exception()
    {
        service.Setup(a => a.GetCoachDetail(TheCanonical.CoachId)).ReturnsAsync(TheCanonical.CoachDetail());
        service.Setup(a => a.UpdateSkills(It.IsAny<IdPrimitive>(), It.IsAny<List<string>>()))
            .ThrowsAsync(new CoachAlreadyHasSkill("one"));
        var result = await controller.UpdateSkills(TheCanonical.CoachId, ["one", "one"]);
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UpdateSkillsViewModel>(view.Model);
        Assert.Equal(TheCanonical.CoachName, model.Name);
        Assert.Equal(TheCanonical.CoachEmail, model.Email);
        Assert.Equal([], model.Skills);
    }

    [Fact]
    public async Task UpdateSkills_POST_Returns_View_With_ModelError_On_Exception()
    {
        service.Setup(a => a.UpdateSkills(It.IsAny<IdPrimitive>(), It.IsAny<List<string>>()))
            .ThrowsAsync(new CoachAlreadyHasSkill("one"));
        var result = await controller.UpdateSkills(TheCanonical.BadId, ["one", "one"]);
        Assert.False(controller.ModelState.IsValid);
        Assert.Contains(controller.ModelState, kvp => kvp.Value!.Errors.Any(e => e.ErrorMessage == "Coach already has skill."));
    }
}