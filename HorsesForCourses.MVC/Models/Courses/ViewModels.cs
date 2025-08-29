using HorsesForCourses.Core.Domain.Courses.TimeSlots;
using HorsesForCourses.Service.Courses.GetCourseDetail;

namespace HorsesForCourses.MVC.Models.Courses;

public record TimeSlotViewModel(CourseDay Day, int Start, int End);

public class UpdateTimeSlotsViewModel(CourseDetail detail)
{
    public string Name { get; set; } = detail?.Name ?? string.Empty;
    public List<TimeSlotViewModel> TimeSlots { get; set; } =
        detail?.TimeSlots == null ? []
            : [.. detail.TimeSlots.Select(a => new TimeSlotViewModel(a.Day, a.Start, a.End))];
}

public class ConfirmCourseViewModel
{
}

public class AssignCoachViewModel
{
}
