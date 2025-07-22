using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
[Route("Admin/[action]")]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        
        return View();
    }
}