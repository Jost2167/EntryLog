using EntryLog.Business.DTOs;
using EntryLog.Business.Interfaces;
using EntryLog.Business.Mappers;
using EntryLog.Business.QueryFilters;
using EntryLog.Business.Specs;
using EntryLog.Data.Interfaces;
using EntryLog.Entities.Entities;
using EntryLog.Entities.Enums;

namespace EntryLog.Business.Services;

public class WorkSessionService(
    IAppUserRepository appUserRepository,
    IWorkSessionRepository workSessionRepository,
    IEmployeeRepository employeeRepository)
    : IWorkSessionService
{

    private readonly IAppUserRepository _appUserService = appUserRepository;
    private readonly IWorkSessionRepository _workSessionRepository = workSessionRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;

    public async Task<(bool sucess, string message)> OpenWorkSessionAsync(CreateWorkSessionDTO createWorkSessionDTO)
    {
        // Convertir el UserId a int
        int code = int.Parse(createWorkSessionDTO.EmployeeId);

        // Validar que el empleado y el usuario existan
        var (sucess, message) = await ValidateEmployeeUserAsync(code);

        if (!sucess)
            return (false, message);

        // Verificar si ya existe una sesión de trabajo activa para el empleado
        WorkSession? activeSession = await _workSessionRepository.GetActiveSessionByEmployeeIdAsync(code);

        if (activeSession != null)
            return (false, "Ya existe una sesión de trabajo activa para el empleado");

        // Crear una nueva sesión de trabajo
        WorkSession workSession = new WorkSession()
        {
            EmployeeId = int.Parse(createWorkSessionDTO.EmployeeId),
            Status = SessionStatus.InProgress,
            CheckIn = new Check
            {
                Method = createWorkSessionDTO.Method,
                DeviceName = createWorkSessionDTO.DeviceName,
                PhotoUrl = "",
                Note = createWorkSessionDTO.Note,
                Date = DateTime.UtcNow,
                Location = new Location
                {
                    Latitude = createWorkSessionDTO.Latitude,
                    Longitude = createWorkSessionDTO.Longitude,
                    IpAddress = createWorkSessionDTO.IpAddress
                }
            }
        };

        // Guardar la sesión de trabajo en la base de datos
        await _workSessionRepository.CreateAsync(workSession);

        return (true, "Sesión de trabajo registrada correctamente");
    }

    public async Task<(bool sucess, string message)> CloseWorkSessionAsync(CloseWorkSessionDTO closeWorkSessionDTO)
    {
        // Convertir el UserId a int
        int code = int.Parse(closeWorkSessionDTO.EmployeeId);

        // Validar que el empleado y el usuario existan
        var (sucess, message) = await ValidateEmployeeUserAsync(code);

        if (!sucess)
            return (false, message);

        // Verificar que la sesión de trabajo esté en progreso
        // Si existe se trae la sesión activa con toda su información
        WorkSession? activeSession = await _workSessionRepository.GetActiveSessionByEmployeeIdAsync(code);

        if (activeSession == null)
            return (false, "No hay una sesión de trabajo activa para el empleado");

        // Actualizar la sesión de trabajo con los datos de cierre
        // Unicamente se actulizan los campos de CheckOut y Status porque los demás campos ya estan cargados
        activeSession.CheckOut ??= new Check();
        activeSession.CheckOut.Method = closeWorkSessionDTO.Method;
        activeSession.CheckOut.DeviceName = closeWorkSessionDTO.DeviceName;
        activeSession.CheckOut.PhotoUrl = "";
        activeSession.CheckOut.Note = closeWorkSessionDTO.Note;
        activeSession.CheckOut.Date = DateTime.UtcNow;
        activeSession.CheckOut.Location.Longitude = closeWorkSessionDTO.Longitude;
        activeSession.CheckOut.Location.Latitude = closeWorkSessionDTO.Latitude;
        activeSession.CheckOut.Location.IpAddress = closeWorkSessionDTO.IpAddress;
        activeSession.Status = SessionStatus.Completed;

        await _workSessionRepository.UpdateAsync(activeSession);

        return (true, "Sesión de trabajo cerrada correctamente");
    }

    private async Task<(bool sucess, string message)> ValidateEmployeeUserAsync(int code)
    {

        // Verificar que el empleado exista
        Employee? employee = await _employeeRepository.GetByCodeAsync(code);

        if (employee == null)
            return (false, "El empleado no existe en el sistema");

        // Verificar que exista el usuario del empleado
        AppUser? appUser = await _appUserService.GetByCodeAsync(code);

        if (appUser == null)
            return (false, "El usuario del empleado no existe en el sistema");

        return (true, "");
    }

    public async Task<IEnumerable<GetWorkSessionDTO>> GetSessionsByFilterAsync(WorkSessionQueryFilter filter)
    {
        WorkSessionSpec spec = new WorkSessionSpec();

        if (filter.EmployeeId.HasValue)
        {
            // Filtrar Worksessions por EmployeeId
            spec.AndAlso(x => x.EmployeeId == filter.EmployeeId.Value);
        }

        IEnumerable<WorkSession> sessions = await _workSessionRepository.GetAllAsync(spec);

        // Mapear las sesiones de trabajo a DTOs
        IEnumerable<GetWorkSessionDTO> sessionDTOs = sessions.Select(w=> WorkSessionMapper.MapToGetWorkSessionDTO(w));

        return sessionDTOs;
    }       

    
}