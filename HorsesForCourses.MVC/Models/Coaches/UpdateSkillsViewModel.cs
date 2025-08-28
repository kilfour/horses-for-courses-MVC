namespace HorsesForCourses.MVC.Models.Coaches;

public class UpdateSkillsViewModel
{
    public UpdateSkillsViewModel() { }
    public UpdateSkillsViewModel(List<string> skills) => Skills = skills;
    public List<string> Skills { get; } = [];
}