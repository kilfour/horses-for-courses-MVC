using HorsesForCourses.Core.Domain.Coaches;

namespace HorsesForCourses.Tests.Tools.Coaches;

public class CoachSpy : Coach
{
    public CoachSpy() : base(TheCanonical.CoachName, TheCanonical.CoachEmail) { }
    public bool Called;
    public IEnumerable<string>? Seen;
    public override void UpdateSkills(IEnumerable<string> skills)
    {
        Called = true; Seen = skills;
    }
}