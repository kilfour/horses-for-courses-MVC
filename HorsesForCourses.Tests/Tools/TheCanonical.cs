using HorsesForCourses.Api.Courses;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;
using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Warehouse.Paging;
using WibblyWobbly;

namespace HorsesForCourses.Tests.Tools;

public static class TheCanonical
{
    public const int CoachId = 1;
    public const string CoachName = "a";
    public const string CoachEmail = "a@a.a";
    public static Coach Coach()
        => new(CoachName, CoachEmail);

    public static CoachDetail CoachDetail()
        => new() { Id = CoachId, Name = CoachName, Email = CoachEmail };

    public static PagedResult<CoachSummary> CoachSummaryList()
        => new([new CoachSummary(CoachId, CoachName, CoachEmail, 0)], 1, 1, 25);

    public readonly static List<string> Skills = ["one", "two"];

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