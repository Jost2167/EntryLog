using EntryLog.Business.DTOs;
using EntryLog.Business.Interfaces;
using EntryLog.Web.Extensions;
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
    // ValidateAntiForgeryToken se usa para prevenir ataques CSRF
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterEmployeeUserAsync([FromBody] CreateEmployeeUserDTO user)
    {
        
        (bool success, string message, LoginResponseDTO? loginResponseDto) = await _appUserService.RegisterEmployeeAsync(user);

        if (success)
        {
            // Iniciar sesion
            await HttpContext.SignInCookieAsync(loginResponseDto!);
            
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

    [HttpGet]
    [AllowAnonymous]
    public IActionResult LoginEmployeeUser()
    {
        return View();
        
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginEmployeeUserAsync([FromBody] UserCredentialsDTO userCredentials)
    {
        (bool success, string message, LoginResponseDTO? loginResponseDto) = await _appUserService.UserLoginAsync(userCredentials);

        if (success)
        {
            // Iniciar sesión
            await HttpContext.SignInCookieAsync(loginResponseDto!);
            
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