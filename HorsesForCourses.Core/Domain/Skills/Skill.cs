namespace HorsesForCourses.Core.Domain.Skills;

public record Skill
{
    private string value = string.Empty;

    public string Value => value;

    private Skill() { }

    private Skill(string value)
    {
        this.value = value;
    }

    public static Skill From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new SkillValueCanNotBeEmpty();
        return new Skill(value);
    }
}
