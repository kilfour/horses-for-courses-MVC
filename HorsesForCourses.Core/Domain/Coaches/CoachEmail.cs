using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Coaches.InvalidationReasons;

namespace HorsesForCourses.Core.Domain.Coaches;

public record CoachEmail : DefaultString<CoachEmailCanNotBeEmpty, CoachEmailCanNotBeTooLong>
{
    public CoachEmail(string value) : base(value) { }
    protected CoachEmail() { }
    public static CoachEmail Empty => new();
}
