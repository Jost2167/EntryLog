using EntryLog.Business.DTOs;
using EntryLog.Business.QueryFilters;
using EntryLog.Data.Pagination;
using EntryLog.Entities.Entities;
using Microsoft.AspNetCore.Http;

namespace EntryLog.Business.Interfaces;

public interface IWorkSessionService
{
    Task<(bool sucess, string message)> OpenWorkSessionAsync(CreateWorkSessionDTO createWorkSessionDTO);
    Task<(bool sucess, string message)> CloseWorkSessionAsync(CloseWorkSessionDTO closeWorkSessionDTO);
    Task<PagedResult<WorkSession>> GetAllPagingAsync(PaginationParameters paginationParameters);
}