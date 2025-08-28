using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Warehouse.Paging;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Coaches.C_GetCoaches;

public class B_GetCoachesMVC : CoachesMVCControllerTests
{
    [Fact]
    public async Task GetCoaches_uses_the_query_object()
    {
        var result = await controller.Index();
        getCoachSummaries.Verify(a => a.All(It.Is<PageRequest>(a => a.Page == 1 && a.PageSize == 25)));
    }

    [Fact]
    public async Task GetCoaches_uses_the_query_object_with_page_info()
    {
        var result = await controller.Index(3, 15);
        getCoachSummaries.Verify(a => a.All(It.Is<PageRequest>(a => a.Page == 3 && a.PageSize == 15)));
    }

    [Fact]
    public async Task GetCoaches_Passes_The_List_To_The_View()
    {
        var paged = TheCanonical.CoachSummaryList();
        getCoachSummaries.Setup(q => q.All(It.IsAny<PageRequest>())).ReturnsAsync(paged);

        var result = await controller.Index();

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<PagedResult<CoachSummary>>(view.Model);
        Assert.Single(model.Items);
        Assert.Equal(1, model.TotalCount);
        Assert.Equal(TheCanonical.CoachName, model.Items[0].Name);
        Assert.Equal(TheCanonical.CoachEmail, model.Items[0].Email);
    }
}