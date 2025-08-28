using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;

namespace HorsesForCourses.Core.Domain.Courses;

public static class Is
{
    public static CoachPlanner TheCoach(Coach coach) => new(coach);
}

public class CoachPlanner(Coach coach)
{
    private readonly Coach coach = coach;

    public bool AvailableFor(Course course)
    {
        foreach (var assigned in coach.AssignedCourses)
        {
            if (CoursesOverlap(course, assigned))
            {
                return false;
            }
        }
        return true;
    }

    private static bool CoursesOverlap(Course courseOne, Course courseTwo)
    {
        var start = Max(courseOne.StartDate, courseTwo.StartDate);
        var end = Min(courseOne.EndDate, courseTwo.EndDate);
        if (end < start) return false;

        var courseOneByDay = GetTimeSlotsByDay(courseOne);
        var courseTwoByDay = GetTimeSlotsByDay(courseTwo);

        foreach (CourseDay day in Enum.GetValues(typeof(CourseDay)))
        {
            if (!courseOneByDay.TryGetValue(day, out var slotsOne))
                continue;

            if (!courseTwoByDay.TryGetValue(day, out var slotsTwo))
                continue;

            var firstOccurrence = NextOnOrAfter(start, ToDayOfWeek(day));
            if (firstOccurrence > end)
                continue;

            foreach (var slotOne in slotsOne)
                foreach (var slotTwo in slotsTwo)
                    if (slotOne.OverlapsWith(slotTwo))
                        return true;
        }

        return false;
    }

    private static Dictionary<CourseDay, List<TimeSlot>> GetTimeSlotsByDay(Course courseOne)
    {
        return courseOne.TimeSlots
            .GroupBy(t => t.Day)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    private static DayOfWeek ToDayOfWeek(CourseDay day)
    {
        return day switch
        {
            CourseDay.Monday => DayOfWeek.Monday,
            CourseDay.Tuesday => DayOfWeek.Tuesday,
            CourseDay.Wednesday => DayOfWeek.Wednesday,
            CourseDay.Thursday => DayOfWeek.Thursday,
            CourseDay.Friday => DayOfWeek.Friday,
            _ => throw new NotImplementedException()
        };
    }

    private static DateOnly NextOnOrAfter(DateOnly date, DayOfWeek target)
    {
        var diff = ((int)target - (int)date.DayOfWeek + 7) % 7;
        return date.AddDays(diff);
    }

    private static DateOnly Max(DateOnly x, DateOnly y) => x > y ? x : y;
    private static DateOnly Min(DateOnly x, DateOnly y) => x < y ? x : y;
}