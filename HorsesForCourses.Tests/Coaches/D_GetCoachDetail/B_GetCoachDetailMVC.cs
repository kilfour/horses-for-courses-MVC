using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Coaches.D_GetCoachDetail;

public class B_GetCoachDetailMVC : CoachesMVCControllerTests
{
    [Fact]
    public async Task GetCoachDetail_uses_the_service()
    {
        var result = await controller.GetCoachDetail(TheCanonical.CoachId);
        service.Verify(a => a.GetCoachDetail(TheCanonical.CoachId));
    }

    [Fact]
    public async Task GetCoachDetail_Passes_The_Model_To_The_View()
    {
        var expected = TheCanonical.CoachDetail();
        service.Setup(a => a.GetCoachDetail(TheCanonical.CoachId)).ReturnsAsync(expected);

        var result = await controller.GetCoachDetail(TheCanonical.CoachId);

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<CoachDetail>(view.Model);
        Assert.Equal(TheCanonical.CoachId, model.Id);
        Assert.Equal(TheCanonical.CoachName, model.Name);
        Assert.Equal(TheCanonical.CoachEmail, model.Email);
    }
}