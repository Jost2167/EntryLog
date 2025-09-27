using Microsoft.AspNetCore.Http;

namespace EntryLog.Business.DTOs
{
    public record CreateWorkSessionDTO
    (
        string EmployeeId,
        IFormFile Image,
        string Latitude,
        string Longitude,
        string? Note        
    );
}