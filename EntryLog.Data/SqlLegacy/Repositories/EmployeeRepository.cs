using EntryLog.Data.Interfaces;
using EntryLog.Data.SqlLegacy.DataContext;
using EntryLog.Entities.Entities;

namespace EntryLog.Data.SqlLegacy.Repositories;

internal class EmployeeRepository(AppDbContext context) : IEmployeeRepository
{
    private readonly AppDbContext _context = context;
    
    public async Task<Employee?> GetByCodeAsync(int code)
    {
        return await _context.Employees.FindAsync(code);
    }
}