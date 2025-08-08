using EntryLog.Entities.Entities;

namespace EntryLog.Data.Interfaces;

public interface IWorkSessionRepository
{
    Task CreateAsync(WorkSession workSession);
    Task UpdateAsync(WorkSession workSession);
    Task<WorkSession?> GetByIdAsync(Guid id);
    Task<WorkSession?> GetByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<WorkSession>> GetAllAsync();
}