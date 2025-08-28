using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;

namespace HorsesForCourses.Tests.Tools.Courses;

public class SlipperyCourse : Course
{
    public SlipperyCourse(string name, DateOnly startDate, DateOnly endDate)
        : base(name, startDate, endDate) { }

    public SlipperyHour On(CourseDay day) => new(this, day);

    public record SlipperyHour(Course Course, CourseDay Day)
    {
        public SlipperyHours From(int hour) => new(Course, Day, hour);
    }

    public record SlipperyHours(Course Course, CourseDay Day, int Start)
    {
        public Course To(int end) =>
            Course.UpdateTimeSlots([(Day, Start, end)], a => a);
    }

    public Course FullDayOnMonday()
    {
        On(CourseDay.Monday).From(9).To(17);
        return this;
    }

    public Course FullDayOnTuesday()
    {
        On(CourseDay.Tuesday).From(9).To(17);
        return this;
    }
}
