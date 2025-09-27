using EntryLog.Business.DTOs;
using EntryLog.Business.Inraestructure;
using EntryLog.Business.Interfaces;
using EntryLog.Business.Mailtrap.Models;
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
        private readonly IEncryptionService _encryptionService;
        private readonly IEmailSendService _emailSendService;
        private readonly IUriService _uriService;

        public AppUserService(
            IEmployeeRepository employeeRepository, 
            IAppUserRepository appUserRepository, 
            IPasswordHasherService passwordHasherService,
            IEncryptionService encryptionService,
            IEmailSendService emailSendService,
            IUriService uriService)
        {
            _appUserRepository = appUserRepository;
            _employeeRepository = employeeRepository;
            _passwordHasherService = passwordHasherService;
            _encryptionService = encryptionService;
            _emailSendService = emailSendService;
            _uriService = uriService;
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
                Name = employee.FullName,
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

        public async Task<(bool sucess, string message, LoginResponseDTO? loginResponseDTO)> UserLoginAsync(UserCredentialsDTO userCredentialsDTO)
        {
            // Verificar que el usuario exista
            AppUser? appUser = await _appUserRepository.GetByUserNameAsync(userCredentialsDTO.Username);   
            
            if (appUser == null)
                return (false, "Usuario y/o contraseña incorrecta", null);
            
            // Verificar que el usuario este activo
            if (!appUser.Active)
                return (false, "Ha ocurrido un error. Contacte con el administrador", null);

            // Verificar que la contraseña sea correcta
            if (!_passwordHasherService.Verify(userCredentialsDTO.Password, appUser.Password))
            {
                return (false, "Contraseña incorrecta", null);
            }
            
            // Login exitoso
            return (true, "Login exitoso", new LoginResponseDTO(appUser.Code, appUser.Role.ToString(), appUser.Email));
        }

        public async Task<(bool sucess, string message)> AccountRecoveryStartAsync(string email)
        {
            // Verificar que el usuario exista
            AppUser? appUser = await _appUserRepository.GetByUserNameAsync(email);
            
            if (appUser == null)
                return (false, "No se ha podido iniciar la recuperación de cuenta");
            
            // Verificar que el usuario este activo
            if (!appUser.Active)
                return (false, "No se ha podido iniciar la recuperación de cuenta");
            
            // Generar un token de recuperación con Ticks
            // Los Ticks son un valor numérico que representa la hora actual
            string recoryTokenPlain = $"{DateTime.UtcNow.Ticks.ToString()}:{appUser.Email}";
            
            // Encriptar el token antes de guardarlo
            string recoveryTokenEncrypted = _encryptionService.Encrypt(recoryTokenPlain);
            
            // Guardar el token en el usuario
            appUser.RecoveryToken = recoveryTokenEncrypted;
            appUser.RecoveryTokenActive = true;
            
            // Actualizar el usuario
            await _appUserRepository.UpdateAsync(appUser);
            
            // Variables para la plantilla del email
            RecoveryAccountVariables variables = new RecoveryAccountVariables
            {
                 Name = appUser.Name,
                 Url = $"{_uriService.ApplicationUrl}/account/recovery?token={recoveryTokenEncrypted}"
            };
            
            // Enviar el token por email
            bool isSend = await _emailSendService.SendEmailWithTemplateAsync("RecoveryToken", "u20222208566@usco.edu.co", variables);
            
            return (isSend, isSend ? $"Se ha enviado un email a {appUser.Email} con las instrucciones para recuperar la cuenta" : "No se ha podido enviar el email de recuperación de cuenta");
        }
        
        public async Task<(bool sucess, string message)> AccountRecoveryCompleteAsync(AccountRecoveryDTO accountRecoveryDTO)
        {
            // Validar token
            if (string.IsNullOrEmpty(accountRecoveryDTO.token) || string.IsNullOrWhiteSpace(accountRecoveryDTO.token))
                return (false, "El token no es valido"); 
            
            // Desencriptar el token
            string tokenDecrypted;

            try
            {
                tokenDecrypted = _encryptionService.Decrypt(accountRecoveryDTO.token);
            }
            catch 
            {
                return (false, "El token no es valido");
            }
            
            // Validar formato del token 1
            if (!tokenDecrypted.Contains(':'))
                return (false, "El token no es valido");
            
            // Validar formato del token 2
            string[] tokenParts = tokenDecrypted.Split(':');

            if (tokenParts.Length != 2)
                return (false, "El token no es valido");
            
            // Validar formato del token 3
            if (!long.TryParse(tokenParts[0], out long tokenTicks))
                return (false, "El token no es valido");
            
            // Extraer el email del token
            string email = tokenParts[1];
            
            // Obtener el usuario a traves de token
            AppUser? appUser = await _appUserRepository.GetByRecoveryTokenAsync(accountRecoveryDTO.token);
            
            if (appUser == null || !string.Equals(appUser.Email, email, StringComparison.OrdinalIgnoreCase))
                return (false, "El token no es valido");
            
            // convertir los Ticks a DateTime
            DateTime tokenDateTime = DateTime.FromBinary(tokenTicks);
            
            // Validar que el token no haya expirado (1 hora)
            DateTime dateTimeNow = DateTime.UtcNow;
            const int expirationMinutes = 180;
            
            var totalMinutes = (dateTimeNow - tokenDateTime).TotalMinutes;

            // Si el token ha expirado, desactivar el token de recuperación
            if (totalMinutes < 0 || totalMinutes > expirationMinutes)
            {
                await FinalizeRecoveryTokenAsync(appUser); 
                return (false, "El token ha expirado");
            }
            else
            {
                await FinalizeRecoveryTokenAsync(appUser); 
                
                // Actualizar la contraseña del usuario
                appUser.Password = _passwordHasherService.Hash(accountRecoveryDTO.password);

                await _appUserRepository.UpdateAsync(appUser);
                
                return (true, "Se ha actualizado la contraseña correctamente");
            }
        }

        private Task FinalizeRecoveryTokenAsync(AppUser appUser)
        {
            // Desactivar el token de recuperación
            appUser.RecoveryToken = null;
            appUser.RecoveryTokenActive = false;
            return Task.CompletedTask;
        }
    }
}