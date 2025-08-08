using EntryLog.Data.Constants;
using EntryLog.Data.Interfaces;
using EntryLog.Entities.Entities;
using MongoDB.Driver;

namespace EntryLog.Data.MongoDb.Repositories;

public class AppUserRepository : IAppUserRepository
{
    private readonly IMongoCollection<AppUser> _collection; 
        
    public AppUserRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<AppUser>(CollectionNames.Users);
    }
    
    public async Task CreateAsync(AppUser appUser)
    { 
        await _collection.InsertOneAsync(appUser);
    }

    public async Task UpdateAsync(AppUser appUser)
    {
        // Reemplaza el usuario existente con el nuevo a traves de su Id
        await _collection.ReplaceOneAsync(u=>u.Id == appUser.Id, appUser);
    }

    public async Task<AppUser?> GetByIdAsync(Guid id)
    {
        return await _collection.Find(u=>u.Id == id).FirstOrDefaultAsync();
    }

    public async Task<AppUser?> GetByUserNameAsync(string email)
    {
        return await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();
    }
}