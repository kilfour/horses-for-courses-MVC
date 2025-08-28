using HorsesForCourses.Api.Courses.UpdateTimeSlots;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;
using WibblyWobbly;

namespace HorsesForCourses.Tests.Tools;

public static class TheCanonical
{
    public const int CoachId = 1;
    public const string CoachName = "a";
    public const string CoachEmail = "a@a.a";
    public static Coach Coach()
        => new(CoachName, CoachEmail);


    public const int CourseId = 1;
    public const string CourseName = "A";
    public static readonly DateOnly CourseStart = 1.January(2025);
    public static readonly DateOnly CourseEnd = 31.January(2025);

    public static Course Course()
        => new(CourseName, CourseStart, CourseEnd);

    public static IEnumerable<TimeSlotRequest> TimeSlotsRequestFullDayMonday()
        => [new(CourseDay.Monday, 9, 17)];
    public static IEnumerable<(CourseDay, int, int)> TimeSlotsFullDayMonday()
        => [(CourseDay.Monday, 9, 17)];
    public static IEnumerable<TimeSlot> TimeSlotsFullDayMondayExpected()
        => [TimeSlot.From(CourseDay.Monday, 9, 17)];
}