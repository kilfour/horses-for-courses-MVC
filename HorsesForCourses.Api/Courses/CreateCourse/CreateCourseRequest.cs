namespace HorsesForCourses.Api.Courses.CreateCourse;

public record CreateCourseRequest(string Name, DateOnly StartDate, DateOnly EndDate);
