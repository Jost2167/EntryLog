using Microsoft.AspNetCore.Http;

namespace EntryLog.Business.DTOs;

public record CloseWorkSessionDTO(
    string SessionId,
    string EmployeeId,
    string Method,
    string? DeviceName,
    IFormFile Image,
    string Latitude,
    string Longitude,
    string IpAddress,
    string? Note
    );