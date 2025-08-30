using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Coaches.InvalidationReasons;

namespace HorsesForCourses.Core.Domain.Coaches;

public record CoachName : DefaultString<CoachNameCanNotBeEmpty, CoachNameCanNotBeTooLong>
{
    public CoachName(string value) : base(value) { }
    protected CoachName() { }
    public static CoachName Empty => new();
}
