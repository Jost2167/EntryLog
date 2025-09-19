using EntryLog.Data.Interfaces;
using EntryLog.Data.MongoDb.Config;
using EntryLog.Data.MongoDb.Repositories;
using EntryLog.Data.MongoDb.Serializers;
using EntryLog.Data.SqlLegacy.DataContext;
using EntryLog.Data.SqlLegacy.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EntryLog.Data;

// Permite extender el contenedor de dependencias
public static class DataDependencies
{
    // IConfiguration permite acceder a la configuracion de la aplicacion (appsettings.json)
    public static IServiceCollection AddDataServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Cadena de conexion a la base de datos
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("EmployeesDB")));
        
        // Servicio de repositorio de usuarios
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        
        // Permite enlazar automaticamente los valores del archivo de configuracion a las propiedades de la clase
        // Vincula la seccion "EntryLogDbOptions" del archivo de appsettings.json a la clase EntryLogDbOptions
        // Se debe agregar el paquete de Microsoft.Extensions.Options.ConfigurationExtensions
        services.Configure<EntryLogDbOptions>(configuration.GetSection("EntryLogDbOptions"));

        // Registra la conexion a Mongo como servicio reutilizable
        services.AddScoped<IMongoDatabase>(sp =>
        {
            // Obtiene la instancia de EntryLogDbOptions con los valores de configuracion
            EntryLogDbOptions entryLogDb = sp.GetRequiredService<IOptions<EntryLogDbOptions>>().Value;

            // Crea una instancia de MongoClient con la URI de conexion
            var client = new MongoClient(entryLogDb.ConnectionUri);

            // Obtiene la base de datos especificada en las opciones y la retorna como IMongoDatabase
            // Esto permite que la base de datos se pueda inyectar en otros servicios
            return client.GetDatabase(entryLogDb.DatabaseName);
        });
        
        // Servicio de MongoDB para gestionar usuarios
        services.AddScoped<IAppUserRepository, AppUserRepository>();

        // Servicio de MongoDB para gestionar sesiones de trabajo
        services.AddScoped<IWorkSessionRepository, WorkSessionRepository>();
        
        // Servicio de MongoDB para gestionar registros de entradas y salidas
        AppUserSerializer.Init();
        WorkSessionSerializer.Init();
        CheckSerializer.Init();
        LocationSerializer.Init();
            
        
        return services;
    }
}