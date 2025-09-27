using EntryLog.Data.Interfaces;
using EntryLog.Data.Pagination;
using MongoDB.Driver;

namespace EntryLog.Data.MongoDb.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly IMongoCollection<T> _collection;
    
    // IMongoCollection<T> es una interfaz que representa una colección de documentos en MongoDB
    // Esta colección representa la colección de sesiones de trabajo en MongoDB 
    public BaseRepository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<T>(collectionName);
    }    
    
    public async Task<PagedResult<T>> GetAllPagingAsync(int pageNumber, int pageSize)
    {
        int totalItems = (int)await _collection.CountDocumentsAsync(_ => true);
        
        var items = await _collection
            .Find(_ => true)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return new PagedResult<T>(items, pageNumber, pageSize, totalItems);
    }
}