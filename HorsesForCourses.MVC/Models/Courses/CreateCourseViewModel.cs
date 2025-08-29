
namespace HorsesForCourses.MVC.Models.Courses;

public class CreateCourseViewModel(string name, DateOnly? startDate, DateOnly? endDate)
{
    public string Name { get; } = name;
    public DateOnly? StartDate { get; } = startDate;
    public DateOnly? EndDate { get; } = endDate;
}
