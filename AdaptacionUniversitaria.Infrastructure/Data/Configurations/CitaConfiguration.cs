using AdaptacionUniversitaria.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdaptacionUniversitaria.Infrastructure.Data.Configurations
{
    public class CitaConfiguration : IEntityTypeConfiguration<Cita>
    {
        public void Configure(EntityTypeBuilder<Cita> builder)
        {
            builder.ToTable("citas");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.EstudianteId).HasColumnName("estudiante_id");
            builder.Property(e => e.ConsejeroId).HasColumnName("consejero_id");
            builder.Property(e => e.FechaHora).HasColumnName("fecha_hora").HasColumnType("datetime");
            builder.Property(e => e.Motivo).HasColumnName("motivo").HasMaxLength(500).IsRequired();
            // El enum se guarda como texto: Pendiente, Aprobada, Rechazada o Cancelada
            builder.Property(e => e.Estado).HasColumnName("estado").HasMaxLength(20)
                .HasConversion(
                    x => x.ToString(),
                    x => Enum.Parse<AdaptacionUniversitaria.Core.Enums.EstadoCita>(x));
            builder.Property(e => e.MotivoRechazo).HasColumnName("motivo_rechazo").HasMaxLength(500);
            builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(e => e.RespondidoEn).HasColumnName("respondido_en").HasColumnType("datetime");

            builder.HasOne(d => d.Estudiante).WithMany(p => p.CitasComoEstudiante)
                .HasForeignKey(d => d.EstudianteId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_citas_estudiante");

            builder.HasOne(d => d.Consejero).WithMany(p => p.CitasComoConsejero)
                .HasForeignKey(d => d.ConsejeroId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_citas_consejero");
        }
    }
}
