using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
using HorsesForCourses.MVC.Models.Courses;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Courses.B_UpdateRequiredSkills;

public class B_UpdateRequiredSkillsMVC : CoursesMVCControllerTests
{
    [Fact]
    public async Task UpdateRequiredSkills_GET_Passes_The_Model_To_The_View()
    {
        service.Setup(a => a.GetCourseDetail(TheCanonical.CourseId)).ReturnsAsync(TheCanonical.CourseDetail());
        var result = await controller.UpdateRequiredSkills(TheCanonical.CourseId);
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UpdateRequiredSkillsViewModel>(view.Model);
        Assert.Equal(TheCanonical.CourseName, model.Name);
        Assert.Equal([], model.Skills);
    }

    [Fact]
    public async Task UpdateRequiredSkills_POST_calls_the_service()
    {
        await controller.UpdateRequiredSkills(TheCanonical.CourseId, TheCanonical.Skills);
        service.Verify(a => a.UpdateRequiredSkills(TheCanonical.CourseId, TheCanonical.Skills));
    }

    [Fact]
    public async Task UpdateRequiredSkills_POST_Redirects_To_Index_On_Success()
    {
        service.Setup(a => a.GetCourseDetail(TheCanonical.CourseId)).ReturnsAsync(TheCanonical.CourseDetail());
        var result = await controller.UpdateRequiredSkills(TheCanonical.CourseId, TheCanonical.Skills);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(controller.Index), redirect.ActionName);
    }

    [Fact]
    public async Task UpdateRequiredSkills_POST_Returns_View_On_Exception()
    {
        service.Setup(a => a.GetCourseDetail(TheCanonical.CourseId)).ReturnsAsync(TheCanonical.CourseDetail());
        service.Setup(a => a.UpdateRequiredSkills(It.IsAny<IdPrimitive>(), It.IsAny<List<string>>()))
            .ThrowsAsync(new CourseAlreadyHasSkill("one"));
        var result = await controller.UpdateRequiredSkills(TheCanonical.CourseId, ["one", "one"]);
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UpdateRequiredSkillsViewModel>(view.Model);
        Assert.Equal(TheCanonical.CourseName, model.Name);
        Assert.Equal([], model.Skills);
    }

    [Fact]
    public async Task UpdateRequiredSkills_POST_Returns_View_With_ModelError_On_Exception()
    {
        service.Setup(a => a.UpdateRequiredSkills(It.IsAny<IdPrimitive>(), It.IsAny<List<string>>()))
            .ThrowsAsync(new CourseAlreadyHasSkill("one"));
        var result = await controller.UpdateRequiredSkills(TheCanonical.BadId, ["one", "one"]);
        Assert.False(controller.ModelState.IsValid);
        Assert.Contains(controller.ModelState, kvp => kvp.Value!.Errors.Any(e => e.ErrorMessage == "Course already has skill."));
    }
}