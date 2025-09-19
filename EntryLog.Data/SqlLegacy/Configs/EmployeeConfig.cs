using EntryLog.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntryLog.Data.SqlLegacy.Configs;

// Permite configurar la entidad Employee en el contexto de Entity Framework Core
public class EmployeeConfig: IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        // Configura la tabla y la clave primaria de la entidad Employee
        builder.ToTable("empleado");
        builder.HasKey(e => e.Code);
        
        // Configura las propiedades de la entidad Employee
        builder.Property(e => e.Code).HasColumnName("codigo_empleado");
        builder.Property(e => e.FullName).HasColumnName("nombres");
        builder.Property(e => e.DateOfBirthday)
            .HasColumnName("fecha_nacimiento")
            .HasColumnType("datetime2"); // fecha de tipo mes/año/dia
        builder.Property(e => e.TownName).HasColumnName("ciudad");
        builder.Property(e => e.PositionId).HasColumnName("id_cargo");
        
        // Configura la relación con la entidad Position
        builder.HasOne(e => e.Position)
            .WithMany(p => p.Employees)
            .HasForeignKey(e => e.PositionId);
    }
}