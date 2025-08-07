using System.Reflection;
using EntryLog.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntryLog.Data.SqlLegacy.DataContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    public DbSet<Employee> Employees { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Agrega todas las configuraciones de entidades desde el ensamblado actual
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}