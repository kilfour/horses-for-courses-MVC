
using HorsesForCourses.Service.Courses.GetCourseDetail;

namespace HorsesForCourses.MVC.Models.Courses;

public class UpdateRequiredSkillsViewModel(CourseDetail detail)
{
    public string Name { get; set; } = detail?.Name ?? string.Empty;
    public List<string> Skills { get; set; } = detail?.Skills == null ? [] : [.. detail.Skills];
}
