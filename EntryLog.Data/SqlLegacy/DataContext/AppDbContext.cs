using EntryLog.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntryLog.Data.SqlLegacy.DataContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("EMPLEADOS");
            entity.HasKey(e => e.Code);
            entity.Property(e => e.Code).HasColumnName("CODIGO");
            entity.Property(e => e.FullName).HasColumnName("NOMBRES");
            entity.Property(e => e.Position).HasColumnName("CARGO");
            entity.Property(e => e.OrganizationId).HasColumnName("EMPRESA");
            entity.Property(e => e.BranchOffice).HasColumnName("SURCURSAL");
            entity.Property(e => e.TownName).HasColumnName("CIUDAD");
            entity.Property(e => e.CostCenter).HasColumnName("CENTRO_COSTO");
        });
        
        base.OnModelCreating(modelBuilder);
    }
}