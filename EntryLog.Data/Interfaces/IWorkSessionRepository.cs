using EntryLog.Data.Specifications;
using EntryLog.Entities.Entities;

namespace EntryLog.Data.Interfaces;

public interface IWorkSessionRepository : IBaseRepository<WorkSession>
{
    Task CreateAsync(WorkSession workSession);
    Task UpdateAsync(WorkSession workSession);
    Task<WorkSession?> GetByIdAsync(Guid id);
    Task<WorkSession?> GetByEmployeeIdAsync(int employeeId);
    Task<WorkSession?> GetActiveSessionByEmployeeIdAsync(int employeeId);
}