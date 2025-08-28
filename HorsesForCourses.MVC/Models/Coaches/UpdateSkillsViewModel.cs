using HorsesForCourses.Service.Coaches.GetCoachDetail;

namespace HorsesForCourses.MVC.Models.Coaches;

public class UpdateSkillsViewModel
{
    public UpdateSkillsViewModel(CoachDetail detail)
    {
        Name = detail.Name;
        Email = detail.Email;
        Skills = detail.Skills;
    }

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = [];
}