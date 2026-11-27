using AdaptacionUniversitaria.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdaptacionUniversitaria.Infrastructure.Data.Configurations
{
    public class BitacoraConfiguration : IEntityTypeConfiguration<Bitacora>
    {
        public void Configure(EntityTypeBuilder<Bitacora> builder)
        {
            builder.ToTable("bitacoras");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.EstudianteId).HasColumnName("estudiante_id");
            builder.Property(e => e.Fecha).HasColumnName("fecha").HasColumnType("date");
            builder.Property(e => e.EstadoAnimo).HasColumnName("estado_animo").HasColumnType("tinyint");
            builder.Property(e => e.Sintomas).HasColumnName("sintomas").HasMaxLength(255);
            builder.Property(e => e.Nota).HasColumnName("nota").HasColumnType("text");
            builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // RN-01: una bitácora por estudiante y por día
            builder.HasIndex(e => new { e.EstudianteId, e.Fecha })
                .IsUnique().HasDatabaseName("uk_bitacoras_estudiante_fecha");

            builder.HasOne(d => d.Estudiante).WithMany(p => p.Bitacoras)
                .HasForeignKey(d => d.EstudianteId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bitacoras_estudiante");
        }
    }
}
