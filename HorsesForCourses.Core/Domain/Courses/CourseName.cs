using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;

namespace HorsesForCourses.Core.Domain.Courses;

public record CourseName : DefaultString<CourseNameCanNotBeEmpty, CourseNameCanNotBeTooLong>
{
    public CourseName(string value) : base(value) { }
    protected CourseName() { }
    public static CourseName Empty => new();
}
