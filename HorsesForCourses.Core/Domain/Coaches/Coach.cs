using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Coaches.InvalidationReasons;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Skills;
using HorsesForCourses.Core.Extensions;

namespace HorsesForCourses.Core.Domain.Coaches;

public class Coach(string name, string email) : DomainEntity<Coach>
{
    public string Name { get; init; } = name.IsValidDefaultString<CoachNameCanNotBeEmpty, CoachNameCanNotBeTooLong>();
    public string Email { get; init; } = email.IsValidDefaultString<CoachEmailCanNotBeEmpty, CoachEmailCanNotBeTooLong>();

    public HashSet<Skill> Skills { get; init; } = [];

    private readonly List<Course> assignedCourses = [];
    public IReadOnlyCollection<Course> AssignedCourses => assignedCourses.AsReadOnly();

    public virtual void UpdateSkills(IEnumerable<string> skills)
    {
        NotAllowedWhenThereAreDuplicateSkills(skills);
        OverwriteSkills(skills);
        // ------------------------------------------------------------------------------------------------
        static bool NotAllowedWhenThereAreDuplicateSkills(IEnumerable<string> skills)
            => skills.NoDuplicatesAllowed(a => new CoachAlreadyHasSkill(string.Join(",", a)));
        void OverwriteSkills(IEnumerable<string> skills)
        {
            Skills.Clear();
            skills.Select(Skill.From)
                .ToList()
                .ForEach(a => Skills.Add(a));
        }
    }

    public bool IsSuitableFor(Course course)
        => course.RequiredSkills.All(Skills.Contains);

    public void AssignCourse(Course course)
    {
        assignedCourses.Add(course);
    }
}
