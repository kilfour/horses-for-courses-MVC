using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;

namespace HorsesForCourses.Tests.Courses.C_UpdateTimeSlots;

public class D_UpdateTimeSlotsData : CourseDatabaseTests
{
    private void Act()
    {
        var context = GetDbContext();
        Reload(context).UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a);
        context.SaveChanges();
    }

    [Fact]
    public void TimeSlots_can_be_updated()
    {
        Act();
        Assert.Equal(TheCanonical.TimeSlotsFullDayMondayExpected(), Reload().TimeSlots);
    }
}