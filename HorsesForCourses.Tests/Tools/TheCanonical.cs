using HorsesForCourses.Api.Courses;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;
using HorsesForCourses.Core.Domain.Skills;
using HorsesForCourses.MVC.Models.Courses;
using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Courses.GetCourseDetail;
using HorsesForCourses.Service.Courses.GetCourses;
using HorsesForCourses.Service.Warehouse.Paging;
using WibblyWobbly;
using static HorsesForCourses.Service.Courses.GetCourseDetail.CourseDetail;

namespace HorsesForCourses.Tests.Tools;

public static class TheCanonical
{
    public static readonly IdPrimitive BadId = -1;

    public static readonly IdPrimitive CoachId = 99;
    public const string CoachName = "The Coach";
    public const string CoachEmail = "coach@coaching.mcCoach";
    public static Coach Coach()
        => new(CoachName, CoachEmail);

    public static PagedResult<CoachSummary> CoachSummaryList()
        => new([new CoachSummary(CoachId, CoachName, CoachEmail, 0)], 1, 1, 25);

    public static CoachDetail CoachDetail()
        => new() { Id = CoachId, Name = CoachName, Email = CoachEmail };

    public readonly static List<string> Skills = ["one", "two"];
    public readonly static HashSet<Skill> HardSkills = [Skill.From("one"), Skill.From("two")];
    public readonly static List<Skill> HardSkillsList = [Skill.From("one"), Skill.From("two")];

    public static readonly IdPrimitive CourseId = 999;
    public const string CourseName = "The Course";
    public static readonly DateOnly CourseStart = 1.January(2025);
    public static readonly DateOnly CourseEnd = 31.January(2025);

    public static Course Course()
        => new(CourseName, CourseStart, CourseEnd);

    public static IEnumerable<TimeSlotRequest> TimeSlotsRequestFullDayMonday()
        => [new(CourseDay.Monday, 9, 17)];
    public static IEnumerable<(CourseDay, int, int)> TimeSlotsFullDayMonday()
        => [(CourseDay.Monday, 9, 17)];
    public static IEnumerable<TimeSlotViewModel> TimeSlotsFullDayMondayViewModel()
        => [new TimeSlotViewModel(CourseDay.Monday, 9, 17)];
    public static IEnumerable<TimeSlot> TimeSlotsFullDayMondayExpected()
        => [TimeSlot.From(CourseDay.Monday, 9, 17)];
    public static IEnumerable<TimeSlotInfo> TimeSlotsFullDayMondayInfo()
        => [new TimeSlotInfo(CourseDay.Monday, 9, 17)];

    public static IEnumerable<TimeSlotViewModel> TimeSlotsWithDuplicateViewModel()
        => [new TimeSlotViewModel(CourseDay.Monday, 9, 17), new TimeSlotViewModel(CourseDay.Monday, 9, 17)];

    public static PagedResult<CourseSummary> CourseSummaryList()
        => new([new CourseSummary(CourseId, CourseName, CourseStart, CourseEnd, false, false)], 1, 1, 25);

    public static CourseDetail CourseDetail()
        => new() { Id = CourseId, Name = CourseName, Start = CourseStart, End = CourseEnd };
}