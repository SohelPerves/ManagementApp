using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var users = _context.Users.OrderByDescending(u => u.LastLoginTime).ToList();
        return View(users);
    }

    [HttpPost]
    public IActionResult Block(List<int> ids)
    {
        var users = _context.Users.Where(u => ids.Contains(u.Id)).ToList();
        foreach (var user in users) user.IsBlocked = true;
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Unblock(List<int> ids)
    {
        var users = _context.Users.Where(u => ids.Contains(u.Id)).ToList();
        foreach (var user in users) user.IsBlocked = false;
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(List<int> ids)
    {
        var users = _context.Users.Where(u => ids.Contains(u.Id)).ToList();
        _context.Users.RemoveRange(users);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
