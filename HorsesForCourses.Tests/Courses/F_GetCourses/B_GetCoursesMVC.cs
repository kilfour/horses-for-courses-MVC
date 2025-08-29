using HorsesForCourses.Service.Courses.GetCourses;
using HorsesForCourses.Service.Warehouse.Paging;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Courses.F_GetCourses;

public class B_GetCoursesMVC : CoursesMVCControllerTests
{
    [Fact]
    public async Task GetCourses_uses_the_service()
    {
        var result = await controller.Index();
        service.Verify(a => a.GetCourses(1, 25));
    }

    [Fact]
    public async Task GetCourses_uses_the_query_object_with_page_info()
    {
        var result = await controller.Index(3, 15);
        service.Verify(a => a.GetCourses(3, 15));
    }

    [Fact]
    public async Task GetCourses_Passes_The_List_To_The_View()
    {
        var paged = TheCanonical.CourseSummaryList();
        service.Setup(a => a.GetCourses(1, 25)).ReturnsAsync(paged);

        var result = await controller.Index();

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<PagedResult<CourseSummary>>(view.Model);
        Assert.Single(model.Items);
        Assert.Equal(1, model.TotalCount);
        Assert.Equal(TheCanonical.CourseName, model.Items[0].Name);
        Assert.Equal(TheCanonical.CourseStart, model.Items[0].StartDate);
        Assert.Equal(TheCanonical.CourseEnd, model.Items[0].EndDate);
    }
}