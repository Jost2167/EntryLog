using EntryLog.Business.DTOs;
using EntryLog.Business.Interfaces;
using EntryLog.Data.Interfaces;
using EntryLog.Entities.Entities;
using EntryLog.Entities.Enums;

namespace EntryLog.Business.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IPasswordHasherService _passwordHasherService;

        public AppUserService(
            IEmployeeRepository employeeRepository, 
            IAppUserRepository appUserRepository, 
            IPasswordHasherService passwordHasherService)
        {
            _appUserRepository = appUserRepository;
            _employeeRepository = employeeRepository;
            _passwordHasherService = passwordHasherService;
        }

        public async Task<(bool sucess, string message, LoginResponseDTO? loginResponseDTO)> RegisterEmployeeAsync(CreateEmployeeUserDTO userDTO)
        {
            int code = int.Parse(userDTO.DocumentNumber);

            // Verificar que el empleado exista
            Employee? employee = await _employeeRepository.GetByCodeAsync(code);

            if (employee == null)
                return (false, "El empleado no existe en el sistema", null);

            // Verificar que exista el usuario del empleado
            AppUser? appUser = await _appUserRepository.GetByCodeAsync(code);

            if (appUser != null)
                return (false, "El usuario del empleado ya existe en el sistema", null);
            
            // Validar que el email no esté registrado
            appUser = await _appUserRepository.GetByUserNameAsync(userDTO.Username);
            
            if (appUser != null)
                return (false, "El email ya está registrado en el sistema", null);
            
            // Validar que las constraseñas coincidan
            if (userDTO.Password != userDTO.PasswordConf)
                return (false, "Las contraseñas no coinciden", null);
            
            // Mapear
            var appUserNew = new AppUser()
            {
                Code = code,
                Role = RoleType.Employee,
                Email = userDTO.Username,
                CellPhone = userDTO.CellPhone,
                Attempts = 0,
                RecoveryTokenActive = false,
                Active = true,
                Password = _passwordHasherService.Hash(userDTO.Password)
            };
            
            // Crear el usuario del empleado
            await _appUserRepository.CreateAsync(appUserNew);
            
            return (true, "Registro exitoso", new LoginResponseDTO(appUserNew.Code, appUserNew.Role.ToString(), appUserNew.Email));
        }

        public Task<(bool sucess, string message, LoginResponseDTO? loginResponseDTO)> UserLoginAsync(UserCredentialsDTO userDTO)
        {
            
        }

        public Task<(bool sucess, string message)> AccountRecoveryStartAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<(bool sucess, string message)> AccountRecoveryCompleteAsync(AccountRecoveryDTO accountRecoveryDTO)
        {
            throw new NotImplementedException();
        }

        

    }
}