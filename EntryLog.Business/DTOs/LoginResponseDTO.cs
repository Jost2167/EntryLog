namespace EntryLog.Business.DTOs
{
    public record LoginResponseDTO(
        int DocumentNumber,
        string Rol,
        string Email
    );
    
}