using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using WibblyWobbly;


namespace HorsesForCourses.Tests.Courses.E_AssignCoach;

public class E_AssignCoachDomain : CourseDomainTests
{
    [Fact]
    public void AssignCoach_WithValidData_ShouldSucceed()
    {
        Entity
            .UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a)
            .Confirm()
            .AssignCoach(TheCanonical.Coach());
        Assert.NotNull(Entity.AssignedCoach);
        Assert.Equal(TheCanonical.CoachName, Entity.AssignedCoach.Name.Value);
    }

    [Fact]
    public void AssignCoach_When_Unconfirmed_Throws()
        => Assert.Throws<CourseNotYetConfirmed>(() =>
            Entity
                .UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a)
                .AssignCoach(TheCanonical.Coach()));

    [Fact]
    public void AssignCoach_Twice_Throws()
    {
        Entity
            .UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a)
            .Confirm().AssignCoach(TheCanonical.Coach());
        Assert.Throws<CourseAlreadyHasCoach>(() => Entity.AssignCoach(TheCanonical.Coach()));
    }

    [Fact]
    public void Coach_Lacking_Skill_Throws()
    {
        Entity
            .UpdateRequiredSkills(["not this one"])
            .UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a)
            .Confirm();
        Assert.Throws<CoachNotSuitableForCourse>(() => Entity.AssignCoach(TheCanonical.Coach()));
    }

    [Fact]
    public void CoachUnavailable_Throws()
    {
        var coach = TheCanonical.Coach();
        var course = TheCanonical.Course()
            .UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a)
            .Confirm()
            .AssignCoach(coach);
        Entity.UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a).Confirm();
        Assert.Throws<CoachNotAvailableForCourse>(() => Entity.AssignCoach(coach));
    }

    private static void AssignTheCoach(Course courseA, Course courseB)
    {
        var coach = TheCanonical.Coach();
        courseA.Confirm().AssignCoach(coach);
        courseB.Confirm().AssignCoach(coach);
    }

    [Fact]
    public void CoachUnavailable_Case_1_Succeeds()
    {
        // Checking the Arrangements
        Assert.Equal(DayOfWeek.Tuesday, 19.August(2025).DayOfWeek);
        // --
        var courseA = new CourseA(19.August(2025), 19.August(2025)).FullDayOnMonday();
        var courseB = new CourseB(19.August(2025), 19.August(2025)).FullDayOnMonday();
        AssignTheCoach(courseA, courseB);
    }

    [Fact]
    public void CoachUnavailable_Case_2_Succeeds()
    {
        // Checking the Arrangements
        Assert.Equal(DayOfWeek.Tuesday, 19.August(2025).DayOfWeek);
        // --
        var courseA = new CourseA(19.August(2025), 19.August(2025)).FullDayOnTuesday();
        var courseB = new CourseB(20.August(2025), 25.August(2025)).FullDayOnTuesday();
        AssignTheCoach(courseA, courseB);
    }

    [Fact]
    public void CoachUnavailable_Case_3_Succeeds()
    {
        // Checking the Arrangements
        Assert.Equal(DayOfWeek.Tuesday, 19.August(2025).DayOfWeek);
        Assert.Equal(DayOfWeek.Tuesday, 26.August(2025).DayOfWeek);
        // --
        var courseA = new CourseA(19.August(2025), 22.August(2025)).FullDayOnTuesday();
        var courseB = new CourseB(20.August(2025), 30.August(2025)).FullDayOnTuesday();
        AssignTheCoach(courseA, courseB);
    }
}
