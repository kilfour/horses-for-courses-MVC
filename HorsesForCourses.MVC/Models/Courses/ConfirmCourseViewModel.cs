using HorsesForCourses.Service.Courses.GetCourseDetail;

namespace HorsesForCourses.MVC.Models.Courses;

public class ConfirmCourseViewModel(CourseDetail detail)
{
    public string Name { get; set; } = detail?.Name ?? string.Empty;
    public bool IsConfirmed { get; set; } = detail?.IsConfirmed ?? false;
}
