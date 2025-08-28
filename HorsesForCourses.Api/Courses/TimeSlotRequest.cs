using HorsesForCourses.Core.Domain.Courses.TimeSlots;

namespace HorsesForCourses.Api.Courses;

public record TimeSlotRequest(CourseDay Day, int Start, int End);