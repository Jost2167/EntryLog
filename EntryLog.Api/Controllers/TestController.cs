using EntryLog.Business.DTOs;
using EntryLog.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EntryLog.Api.Controllers;

[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    private readonly IAppUserService _appUserService;
    
    public TestController(IAppUserService appUserService)
    {
        _appUserService=appUserService;
    }

    [HttpPost("registrar-usuario")]
    public async Task<IActionResult> RegistrarAppUser([FromBody] CreateEmployeeUserDTO userDTO)
    {
        var tupla = await _appUserService.RegisterEmployeeAsync(userDTO);

        if (!tupla.sucess)
            return BadRequest(new { sucess = false, message = tupla.message });

        return Ok(new { sucess = true, message = tupla.message, data = tupla.loginResponseDTO });
    }

    [HttpPost("iniciar-sesion-usuario")]
    public async Task<IActionResult> IniciarSesionAppUser([FromBody] UserCredentialsDTO userCredenTO)
    {
        var tupla = _appUserService.UserLoginAsync(userCredenTO);
        
        if (!tupla.Result.sucess)
            return BadRequest(new { sucess = false, message = tupla.Result.message });
        
        return Ok(new { sucess = true, message = tupla.Result.message, data = tupla.Result.loginResponseDTO });
    }

    [HttpPost("iniciar-recuperacion-usuario")]
    public async Task<IActionResult> IniciarRecuperacion([FromBody] string email)
    {
        var tupla = await _appUserService.AccountRecoveryStartAsync(email);
        
        if (!tupla.sucess)
            return BadRequest(new { sucess = false, message = tupla.message });
        
        return Ok( new { sucess = true, message = tupla.message });
    }

    [HttpPost("completar-recuperacion-usuario")]
    public async Task<IActionResult> CompletarRecueperacion([FromBody] AccountRecoveryDTO accountRecoveryDTO)
    {

        var tupla = await _appUserService.AccountRecoveryCompleteAsync(accountRecoveryDTO);
        
        if (!tupla.sucess)
            return BadRequest(new { sucess = false, message = tupla.message });
        
        return Ok( new { sucess = true, message = tupla.message });
    }
}