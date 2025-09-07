using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using HorsesForCourses.MVC.Models.Account;

namespace HorsesForCourses.MVC.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email)
    {
        var claims = new List<Claim> {
            new(ClaimTypes.Name, email),
            new("skill", "Agile Coaching")
        };
        var id = new ClaimsIdentity(claims, "Cookies");
        await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(id));
        return Redirect("/");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return Redirect("/");
    }

    [HttpGet]
    public IActionResult AccessDenied(string? returnUrl = null)
        => View(model: returnUrl);
}