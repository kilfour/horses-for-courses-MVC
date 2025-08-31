namespace HorsesForCourses.Service.Courses.GetCourses;

public record CourseSummary(
    IdPrimitive Id,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    bool HasSchedule,
    bool HasCoach);
