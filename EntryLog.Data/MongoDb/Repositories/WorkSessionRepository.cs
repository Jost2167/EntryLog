using EntryLog.Data.Constants;
using EntryLog.Data.Interfaces;
using EntryLog.Entities.Entities;
using MongoDB.Driver;

namespace EntryLog.Data.MongoDb.Repositories;

// Se deber descargar la dependencia de MongoDB.Driver
// IMongoDatabase es una interfaz que representa la base de datos MongoDB
public class WorkSessionRepository(IMongoDatabase database) : IWorkSessionRepository
{
    // IMongoCollection<T> es una interfaz que representa una colección de documentos en MongoDB
    // Esta colección representa la colección de sesiones de trabajo en MongoDB 
    private readonly IMongoCollection<WorkSession> _collection = 
        database.GetCollection<WorkSession>(CollectionNames.WorkSessions);
    
    // Crea una nueva sesión de trabajo en la colección
    public async Task CreateAsync(WorkSession workSession)
    {
        await _collection.InsertOneAsync(workSession);
    }

    // Actualiza todos los campos de una sesión de trabajo existente en la colección
    public async Task UpdateAsync(WorkSession workSession)
    {
        await _collection.ReplaceOneAsync(w=>w.Id == workSession.Id, workSession);
    }

    // Busca una sesión de trabajo por su Id en la colección
    public async Task<WorkSession?> GetByIdAsync(Guid id)
    {
        return await _collection.Find(w => w.Id == id).FirstOrDefaultAsync();
    }

    // Busca una sesión de trabajo por el Id del empleado en la colección
    public async Task<WorkSession?> GetByEmployeeIdAsync(int employeeId)
    {
        return await _collection.Find(w => w.EmployeeId == employeeId).FirstOrDefaultAsync();
    }

    public Task<IEnumerable<WorkSession>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}