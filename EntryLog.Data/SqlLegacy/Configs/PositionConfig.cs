using EntryLog.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntryLog.Data.SqlLegacy.Configs;

public class PositionConfig : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("cargo");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("codigo_cargo");
        builder.Property(p => p.Name).HasColumnName("nombre");
        builder.Property(p => p.Description).HasColumnName("descripcion");
    }
}