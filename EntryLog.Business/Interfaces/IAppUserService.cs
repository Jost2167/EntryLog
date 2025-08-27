using EntryLog.Business.DTOs;

namespace EntryLog.Business.Interfaces
{
    public interface IAppUserService
    {
        // Registra un nuevo usuario de la aplicación
        Task<(bool sucess, string message, LoginResponseDTO? loginResponseDTO)> RegisterEmployeeAsync(CreateEmployeeUserDTO userDTO);

        // Inicia sesión de un usuario de la aplicación
        Task<(bool sucess, string message, LoginResponseDTO? loginResponseDTO)> UserLoginAsync(UserCredentialsDTO userDTO);

        // Iniciar recuperacion
        Task<(bool sucess, string message)> AccountRecoveryStartAsync(string email);

        // Completar recuperacion
        Task<(bool sucess, string message)> AccountRecoveryCompleteAsync(AccountRecoveryDTO accountRecoveryDTO);
        
    }
}