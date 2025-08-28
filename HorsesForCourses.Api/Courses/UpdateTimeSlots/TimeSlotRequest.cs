using HorsesForCourses.Core.Domain.Courses.TimeSlots;

namespace HorsesForCourses.Api.Courses.UpdateTimeSlots;

public record TimeSlotRequest(CourseDay Day, int Start, int End);