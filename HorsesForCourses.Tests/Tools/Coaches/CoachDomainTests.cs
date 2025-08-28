using HorsesForCourses.Core.Domain.Coaches;

namespace HorsesForCourses.Tests.Tools.Coaches;

public class CoachDomainTests : DomainTests<Coach>
{
    protected override Coach CreateCannonicalEntity() => TheCanonical.Coach();
}