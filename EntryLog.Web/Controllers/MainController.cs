using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntryLog.Web.Controllers;

public class MainController : Controller
{
    
    [HttpGet]
    [Authorize(Roles = "Employee")]
    public IActionResult Index()
    {
        return View();
    }
}