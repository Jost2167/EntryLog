using EntryLog.Data.Pagination;

namespace EntryLog.Data.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<PagedResult<T>> GetAllPagingAsync(int pageNumber, int pageSize);
}