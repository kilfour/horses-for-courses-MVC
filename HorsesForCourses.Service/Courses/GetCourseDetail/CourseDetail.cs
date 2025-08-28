using HorsesForCourses.Core.Domain.Courses.TimeSlots;

namespace HorsesForCourses.Service.Courses.GetCourseDetail;

public record CourseDetail
{
    public record TimeSlotInfo(CourseDay Day, int Start, int End);
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
    public IEnumerable<string> Skills { get; set; } = [];
    public IEnumerable<TimeSlotInfo> TimeSlots { get; set; } = [];
    public IdAndName? Coach { get; set; }
}