using EntryLog.Data.Interfaces;
using EntryLog.Data.SqlLegacy.DataContext;
using EntryLog.Entities.Entities;

namespace EntryLog.Data.SqlLegacy.Repositories;

internal class EmployeeRepository(AppDbContext context) : IEmployeeRepository
{
    public async Task<Employee?> GetByCodeAsync(int code)
    {
        return await context.Employees.FindAsync(code);
    }
}