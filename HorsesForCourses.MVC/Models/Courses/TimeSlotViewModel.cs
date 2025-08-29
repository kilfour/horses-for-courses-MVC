using HorsesForCourses.Core.Domain.Courses.TimeSlots;

namespace HorsesForCourses.MVC.Models.Courses;

public record TimeSlotViewModel(CourseDay Day, int Start, int End);
