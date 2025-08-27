using EntryLog.Business.DTOs;
using EntryLog.Business.QueryFilters;

namespace EntryLog.Business.Interfaces;

public interface IWorkSessionService
{
    Task<(bool sucess, string message)> OpenWorkSessionAsync(CreateWorkSessionDTO createWorkSessionDTO);
    Task<(bool sucess, string message)> CloseWorkSessionAsync(CloseWorkSessionDTO closeWorkSessionDTO);
    Task<IEnumerable<GetWorkSessionDTO>> GetSessionsByFilterAsync(WorkSessionQueryFilter filter);
}