using EntryLog.Business.DTOs;
using EntryLog.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EntryLog.Api.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController(IAppUserService appUserService) : ControllerBase
{
    private readonly IAppUserService _appUserService = appUserService;
    
    [HttpPost("register-user")]
    public async Task<IActionResult> RegisterUser([FromBody] CreateEmployeeUserDTO userDTO)
    {
        var tupla = await _appUserService.RegisterEmployeeAsync(userDTO);

        if (!tupla.sucess)
            return BadRequest(new { sucess = false, message = tupla.message });

        return Ok(new { sucess = true, message = tupla.message, data = tupla.loginResponseDTO });
    }

    [HttpPost("login-user")]
    public async Task<IActionResult> LoginUser([FromBody] UserCredentialsDTO userCredenTO)
    {
        var tupla = await _appUserService.UserLoginAsync(userCredenTO);
        
        if (!tupla.sucess)
            return BadRequest(new { sucess = false, message = tupla.message });
        
        return Ok(new { sucess = true, message = tupla.message, data = tupla.loginResponseDTO });
    }

    [HttpPost("start-account-recovery")]
    public async Task<IActionResult> StartAccountRecovery([FromBody] string email)
    {
        var tupla = await _appUserService.AccountRecoveryStartAsync(email);
        
        if (!tupla.sucess)
            return BadRequest(new { sucess = false, message = tupla.message });
        
        return Ok( new { sucess = true, message = tupla.message });
    }

    [HttpPost("complete-account-recovery")]
    public async Task<IActionResult> CompleteAccountRecovery([FromBody] AccountRecoveryDTO accountRecoveryDTO)
    {

        var tupla = await _appUserService.AccountRecoveryCompleteAsync(accountRecoveryDTO);
        
        if (!tupla.sucess)
            return BadRequest(new { sucess = false, message = tupla.message });
        
        return Ok( new { sucess = true, message = tupla.message });
    }
    
}