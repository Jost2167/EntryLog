namespace EntryLog.Business.DTOs
{
    public record CreateEmployeeUserDTO(
        string DocumentNumber,
        // Username es el email del usuario
        string Username,
        string CellPhone,
        string Password,
        string PasswordConf
    );
}