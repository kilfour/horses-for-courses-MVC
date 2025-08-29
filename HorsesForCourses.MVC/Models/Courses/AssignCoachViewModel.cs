using HorsesForCourses.Service.Courses.GetCourseDetail;

namespace HorsesForCourses.MVC.Models.Courses;

public class AssignCoachViewModel(CourseDetail detail)
{
    public string Name { get; set; } = detail?.Name ?? string.Empty;
    public string CoachName { get; set; } = detail?.Coach?.Name ?? string.Empty;
}
