using EntryLog.Business.DTOs;
using EntryLog.Business.Inraestructure;
using EntryLog.Business.Interfaces;
using EntryLog.Data.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace EntryLog.Api.Controllers;

[ApiController]
[Route("api/work-session")]
public class WorkSessionController(IWorkSessionService workSessionService) : ControllerBase
{
    private readonly IWorkSessionService _workSessionService = workSessionService;
    
    [HttpPost("open")]
    public async Task<IActionResult> OpenWorkSession([FromForm] CreateWorkSessionDTO createWorkSessionDto)
    {
        var (sucess, message) = await _workSessionService.OpenWorkSessionAsync(createWorkSessionDto);

        if (!sucess)
            return BadRequest(new { sucess = false, message });

        return Ok(new { sucess = true, message });
    }

    [HttpPost("close")]
    public async Task<IActionResult> CloseWorkSession([FromForm] CloseWorkSessionDTO closeWorkSessionDto)
    {
        var (sucess, message) = await _workSessionService.CloseWorkSessionAsync(closeWorkSessionDto);

        if (!sucess)
            return BadRequest(new { sucess = false, message });

        return Ok(new { sucess = true, message });
    }

    [HttpGet("get-all-paging")]
    public async Task<IActionResult> GetAllPaging([FromQuery] PaginationParameters paginationParameters)
    {
        var pagedResult = await _workSessionService.GetAllPagingAsync(paginationParameters);
        
        return Ok(pagedResult);
    }
    
    

    
    
    
    
    
    
}