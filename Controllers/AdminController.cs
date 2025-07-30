using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
[Route("Admin/[action]")]
public class AdminController : Controller
{
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        
        return View();
    }
}