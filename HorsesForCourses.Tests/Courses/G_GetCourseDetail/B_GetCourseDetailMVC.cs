using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HorsesForCourses.Tests.Courses.G_GetCourseDetail;

public class B_GetCourseDetailMVC : CoursesMVCControllerTests
{
    [Fact]
    public async Task GetCourseDetail_uses_the_service()
    {
        var result = await controller.GetCourseDetail(TheCanonical.CourseId);
        service.Verify(a => a.GetCourseDetail(TheCanonical.CourseId));
    }

    [Fact]
    public async Task GetCourseDetail_Passes_The_Model_To_The_View()
    {
        var expected = TheCanonical.CourseDetail();
        service.Setup(a => a.GetCourseDetail(TheCanonical.CourseId)).ReturnsAsync(expected);

        var result = await controller.GetCourseDetail(TheCanonical.CourseId);

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<CourseDetail>(view.Model);
        Assert.Equal(TheCanonical.CourseId, model.Id);
        Assert.Equal(TheCanonical.CourseName, model.Name);
        Assert.Equal(TheCanonical.CourseStart, model.Start);
        Assert.Equal(TheCanonical.CourseEnd, model.End);
    }
}