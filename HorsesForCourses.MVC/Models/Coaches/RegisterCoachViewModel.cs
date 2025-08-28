namespace HorsesForCourses.MVC.Models.Coaches;

public class RegisterCoachViewModel
{
    public RegisterCoachViewModel() { }
    public RegisterCoachViewModel(string name, string email) { Name = name; Email = email; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
