using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Service.Warehouse;
using HorsesForCourses.Tests.Tools;


namespace HorsesForCourses.Tests.Courses.D_ConfirmCourse;

public class D_ConfirmCourseCourseData : DatabaseTests
{
    private readonly Course course;

    public D_ConfirmCourseCourseData()
    {
        course = TheCanonical.Course();
        AddToDb(course);
    }

    private void Act()
    {
        var context = GetDbContext();

        var entity = Reload(context);
        entity.UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a);
        entity.Confirm();
        context.SaveChanges();
    }

    private Course Reload() => Reload(GetDbContext());
    private Course Reload(AppDbContext dbContext) => dbContext.Courses.Single(a => a.Id == course.Id);

    [Fact]
    public void Skills_can_be_updated()
    {
        Act();
        Assert.True(Reload().IsConfirmed);
    }
}