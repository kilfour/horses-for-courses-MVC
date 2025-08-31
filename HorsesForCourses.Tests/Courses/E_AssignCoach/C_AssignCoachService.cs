using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Moq;

namespace HorsesForCourses.Tests.Courses.E_AssignCoach;

public class C_AssignCoachService : CoursesServiceTests
{

    private void SetUpMocks()
    {
        getCoachById.Setup(a => a.Load(TheCanonical.CoachId)).ReturnsAsync(TheCanonical.Coach());
        courseSpy.UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a);
        courseSpy.Confirm();
        getCourseById.Setup(a => a.Load(TheCanonical.CourseId)).ReturnsAsync(courseSpy);
    }

    [Fact]
    public async Task AssignCoach_uses_the_query_objects()
    {
        await service.AssignCoach(TheCanonical.CourseId, TheCanonical.CoachId);
        getCourseById.Verify(a => a.Load(TheCanonical.CourseId));
        getCoachById.Verify(a => a.Load(TheCanonical.CoachId));
    }

    [Fact]
    public async Task AssignCoach_calls_domain()
    {
        SetUpMocks();
        await service.AssignCoach(TheCanonical.CourseId, TheCanonical.CoachId);
        Assert.True(courseSpy.AssignCoachCalled);
        Assert.Equal(TheCanonical.CoachName, courseSpy.AssignCoachSeen!.Name.Value);
    }

    [Fact]
    public async Task AssignCoach_calls_supervisor_ship()
    {
        SetUpMocks();
        await service.AssignCoach(TheCanonical.CourseId, TheCanonical.CoachId);
        supervisor.Verify(a => a.Ship());
    }

    [Fact]
    public async Task AssignCoach_Success_returns_true()
    {
        SetUpMocks();
        Assert.True(await service.AssignCoach(TheCanonical.CourseId, TheCanonical.CoachId));
    }

    [Fact]
    public async Task AssignCoach_Returns_false_If_No_Course()
    {
        getCoachById.Setup(a => a.Load(TheCanonical.CoachId)).ReturnsAsync(TheCanonical.Coach());
        getCourseById.Setup(a => a.Load(TheCanonical.CourseId)).ReturnsAsync((Course)null!);
        Assert.False(await service.AssignCoach(TheCanonical.BadId, TheCanonical.CoachId));
    }

    [Fact]
    public async Task AssignCoach_Returns_false_If_No_Coach()
    {
        getCoachById.Setup(a => a.Load(TheCanonical.CoachId)).ReturnsAsync((Coach)null!);
        courseSpy.UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a);
        courseSpy.Confirm();
        getCourseById.Setup(a => a.Load(TheCanonical.CourseId)).ReturnsAsync(courseSpy);
        Assert.False(await service.AssignCoach(TheCanonical.CourseId, TheCanonical.CoachId));
    }
}