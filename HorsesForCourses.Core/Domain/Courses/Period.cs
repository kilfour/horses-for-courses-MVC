using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;

namespace HorsesForCourses.Core.Domain.Courses;

public record Period
{
    public DateOnly Start { get; }
    public DateOnly End { get; }

    private Period() { /*** EFC Was Here ****/ }
    private Period(DateOnly start, DateOnly end)
    {
        Start = start;
        End = end;
    }

    public static Period From(DateOnly start, DateOnly end)
    {
        if (start > end)
            throw new CourseEndDateCanNotBeBeforeStartDate();
        return new Period(start, end);
    }

    public static Period Empty => new();
}
