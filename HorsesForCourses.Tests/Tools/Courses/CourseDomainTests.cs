using HorsesForCourses.Core.Domain.Courses;

namespace HorsesForCourses.Tests.Tools.Courses;

public class CourseDomainTests : DomainTests<Course>
{
    protected override Course CreateCannonicalEntity() => TheCanonical.Course();
}