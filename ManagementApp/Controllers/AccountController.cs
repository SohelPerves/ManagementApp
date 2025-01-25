using ManagementApp.Models;
using ManagementApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly UserService _userService;

    public AccountController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var user = _userService.Authenticate(email, password);
        if (user != null)
        {
            // Store user data in claims
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("UserId", user.Id.ToString())
        };

            // Create identity and principal
            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            // Sign in the user
            HttpContext.SignInAsync("CookieAuth", principal);

            HttpContext.Session.SetString("UserId", user.Id.ToString());
            return RedirectToAction("Index", "Admin");
        }

        ViewBag.Error = "Invalid credentials or account blocked.";
        return View();
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [HttpPost]
    public IActionResult Register(ApplicationUser user)
    {
        try
        {
            _userService.Register(user);
            return RedirectToAction("Login");
        }
        catch (DbUpdateException ex)
        {
           
            if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601) 
            {
                ViewBag.Error = "This email is already registered.";
            }
            else
            {
                ViewBag.Error = "An error occurred while registering. Please try again.";
            }
            return View(user);
        }
    }


    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
