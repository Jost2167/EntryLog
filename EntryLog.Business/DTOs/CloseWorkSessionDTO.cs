using Microsoft.AspNetCore.Http;

namespace EntryLog.Business.DTOs;

public record CloseWorkSessionDTO(
    string SessionId,
    string EmployeeId,
    IFormFile Image,
    string Latitude,
    string Longitude,
    string? Note
    );