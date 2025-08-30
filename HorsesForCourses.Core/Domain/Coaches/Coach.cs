using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Coaches.InvalidationReasons;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Skills;
using HorsesForCourses.Core.ValidationHelpers;

namespace HorsesForCourses.Core.Domain.Coaches;

public class Coach : DomainEntity<Coach>
{
    public CoachName Name { get; init; } = CoachName.Empty;
    public CoachEmail Email { get; init; } = CoachEmail.Empty;

    public HashSet<Skill> Skills { get; init; } = [];

    public IReadOnlyCollection<Course> AssignedCourses => assignedCourses.AsReadOnly();
    private readonly List<Course> assignedCourses = [];

    private Coach() { /*** EFC Was Here ****/ }
    public Coach(string name, string email)
    {
        Name = new CoachName(name);
        Email = new CoachEmail(email);
    }

    public virtual void UpdateSkills(IEnumerable<string> skills)
    {
        NotAllowedWhenThereAreDuplicateSkills(skills);
        OverwriteSkills(skills);
        // ------------------------------------------------------------------------------------------------
        // --
        static bool NotAllowedWhenThereAreDuplicateSkills(IEnumerable<string> skills)
            => skills.NoDuplicatesAllowed(a => new CoachAlreadyHasSkill(string.Join(",", a)));
        void OverwriteSkills(IEnumerable<string> skills)
        {
            Skills.Clear();
            skills.Select(Skill.From)
                .ToList()
                .ForEach(a => Skills.Add(a));
        }
        // ------------------------------------------------------------------------------------------------
    }

    public bool IsSuitableFor(Course course)
        => course.RequiredSkills.All(Skills.Contains);

    public bool IsAvailableFor(Course course)
        => CheckIf.ImAvailable(this).For(course);

    public void AssignCourse(Course course)
        => assignedCourses.Add(course);
}
