using EntryLog.Entities.Entities;

namespace EntryLog.Data.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee?> GetByCodeAsync(int code);
}