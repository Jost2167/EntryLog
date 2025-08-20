using EntryLog.Entities.Entities;

namespace EntryLog.Data.Interfaces;

public interface IAppUserRepository
{
    Task CreateAsync(AppUser appUser);
    Task UpdateAsync(AppUser appUser);
    Task<AppUser?> GetByIdAsync(Guid id);
    Task<AppUser?> GetByCodeAsync(int code);
    Task<AppUser?> GetByUserNameAsync(string email);
    
    
}