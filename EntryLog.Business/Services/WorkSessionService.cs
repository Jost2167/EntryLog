using EntryLog.Business.DTOs;
using EntryLog.Business.ImageBB;
using EntryLog.Business.Inraestructure;
using EntryLog.Business.Interfaces;
using EntryLog.Business.Mappers;
using EntryLog.Business.QueryFilters;
using EntryLog.Business.Specs;
using EntryLog.Data.Interfaces;
using EntryLog.Data.Pagination;
using EntryLog.Entities.Entities;
using EntryLog.Entities.Enums;
using Microsoft.AspNetCore.Http;

namespace EntryLog.Business.Services;

public class WorkSessionService : IWorkSessionService
{
    private readonly IAppUserRepository _appUserService;
    private readonly IWorkSessionRepository _workSessionRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILoadImagesService _loadImagesService;
    private readonly IUriService _uriService;

    public WorkSessionService(
        IAppUserRepository appUserRepository,
        IWorkSessionRepository workSessionRepository,
        IEmployeeRepository employeeRepository,
        ILoadImagesService loadImagesService,
        IUriService uriService)
    {
        _appUserService = appUserRepository;
        _workSessionRepository = workSessionRepository;
        _employeeRepository = employeeRepository;    
        _loadImagesService = loadImagesService;
        _uriService = uriService;
    }
    
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
        
        // Subir la imagen y obtener la URL
        string imageUrl = await UploadImageAsync(createWorkSessionDTO.Image);
        
        // Crear una nueva sesión de trabajo
        WorkSession workSession = new WorkSession()
        {
            EmployeeId = int.Parse(createWorkSessionDTO.EmployeeId),
            Status = SessionStatus.InProgress,
            CheckIn = new Check
            {
                Method = _uriService.UserAgent,
                DeviceName = _uriService.Platform,
                PhotoUrl = imageUrl,
                Note = createWorkSessionDTO.Note,
                Date = DateTime.UtcNow,
                Location = new Location
                {
                    Latitude = createWorkSessionDTO.Latitude,
                    Longitude = createWorkSessionDTO.Longitude,
                    IpAddress = _uriService.RemoteIpAddress
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
        
        // Subir la imagen y obtener la URL
        string imageUrl = await UploadImageAsync(closeWorkSessionDTO.Image);

        // Actualizar la sesión de trabajo con los datos de cierre
        // Unicamente se actulizan los campos de CheckOut y Status porque los demás campos ya estan cargados
        activeSession.CheckOut ??= new Check();
        activeSession.CheckOut.Method = _uriService.UserAgent;
        activeSession.CheckOut.DeviceName = _uriService.Platform;
        activeSession.CheckOut.PhotoUrl = imageUrl;
        activeSession.CheckOut.Note = closeWorkSessionDTO.Note;
        activeSession.CheckOut.Date = DateTime.UtcNow;
        activeSession.CheckOut.Location.Longitude = closeWorkSessionDTO.Longitude;
        activeSession.CheckOut.Location.Latitude = closeWorkSessionDTO.Latitude;
        activeSession.CheckOut.Location.IpAddress = _uriService.RemoteIpAddress;
        activeSession.Status = SessionStatus.Completed;

        await _workSessionRepository.UpdateAsync(activeSession);

        return (true, "Sesión de trabajo cerrada correctamente");
    }

    public async Task<PagedResult<WorkSession>> GetAllPagingAsync(PaginationParameters paginationParameters)
    {
        return await _workSessionRepository.GetAllPagingAsync(paginationParameters.PageNumber, paginationParameters.PageSize);
    }

    /*
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
    */
    
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
    
    private async Task<string> UploadImageAsync(IFormFile file)
    {
        // Leer la imagen del formulario
        Stream imageStream = file.OpenReadStream();
        // Obtener el nombre de la imagen
        var nombreImagen = file.FileName;
        // Obtener la extensión de la imagen
        var extension = Path.GetExtension(nombreImagen);
        // Obtener el tipo de contenido de la imagen
        var contentType = file.ContentType;
        
        // Servicio para cargar la imagen y obtener la URL
        return await _loadImagesService.UploadImageAsync(imageStream, nombreImagen, extension, contentType);
    }
}