using EntryLog.Business.DTOs;
using EntryLog.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntryLog.Web.Controllers;

public class AccountController : Controller
{
    private readonly IAppUserService _appUserService;
    
    public AccountController(IAppUserService appUserService)
    {
        _appUserService = appUserService;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult RegisterEmployeeUser()
    {
        return View();
    }
    
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterEmployeeUserAsync([FromBody] CreateEmployeeUserDTO user)
    {
        
        (bool success, string message, LoginResponseDTO? loginResponseDto) = await _appUserService.RegisterEmployeeAsync(user);

        if (success)
        {
            return Json(new
            {
                success,
                path = "/main/index"
            });
        }
        else
        {
            return Json(new
            {
                success,
                message
            });
        }
    }

}