using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Tests.Tools;


namespace HorsesForCourses.Tests.Courses.G_GetCourseDetail;


public class D_GetCourseDetailData : DatabaseTests
{
    private IdPrimitive IdAssignedByDb;
    private async Task<CourseDetail?> Act()
        => await new GetCourseDetail(GetDbContext()).One(IdAssignedByDb);

    [Fact]
    public async Task With_Course()
    {

        IdAssignedByDb = AddToDb(TheCanonical.Course()
            .UpdateRequiredSkills(TheCanonical.Skills)
            .UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a)
            .Confirm());

        var detail = await Act();
        Assert.NotNull(detail);
        Assert.Equal(IdAssignedByDb, detail.Id);
        Assert.Equal(TheCanonical.CourseName, detail.Name);
        Assert.Equal(TheCanonical.CourseStart, detail.Start);
        Assert.Equal(TheCanonical.CourseEnd, detail.End);
        Assert.Equal(TheCanonical.Skills, detail.Skills);
        Assert.Equal(TheCanonical.TimeSlotsFullDayMondayInfo(), detail.TimeSlots);
        Assert.True(detail.IsConfirmed);
        Assert.Null(detail.Coach);
    }

    [Fact]
    public async Task NotThere_Returns_Null()
        => Assert.Null(await Act());
}