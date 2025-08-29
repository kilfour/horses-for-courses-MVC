using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;


namespace HorsesForCourses.Tests.Courses.D_ConfirmCourse;

public class E_ConfirmCourseCourseDomain : CourseDomainTests
{
    protected override Course ManipulateEntity(Course entity)
        => entity.UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a);

    [Fact]
    public void ConfirmCourseCourse_WithValidData_ShouldSucceed()
    {
        Entity.Confirm();
        Assert.True(Entity.IsConfirmed);
    }

    [Fact]
    public void ConfirmCourseCourse_Twice_Throws()
    {
        Entity.Confirm();
        Assert.Throws<CourseAlreadyConfirmed>(() => Entity.Confirm());
    }

    [Fact]
    public void ConfirmCourseCourse_Without_TimeSlots_Throws()
    {
        Entity.UpdateTimeSlots((IEnumerable<(CourseDay, int, int)>)[], a => a);
        Assert.Throws<AtLeastOneTimeSlotRequired>(() => Entity.Confirm());
    }
}
