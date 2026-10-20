using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AdaptacionUniversitaria.Core.Entities;

namespace AdaptacionUniversitaria.Infrastructure.Data.Configurations
{
    public class CitaConfiguration :
        IEntityTypeConfiguration<Cita>
    {
        public void Configure(EntityTypeBuilder<Cita> entity)
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("citas");

            entity.HasIndex(e => e.ConsejeroId, "fk_citas_consejero");

            entity.HasIndex(e => e.EstudianteId, "fk_citas_estudiante");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConsejeroId).HasColumnName("consejero_id");
            entity.Property(e => e.CreadoEn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("creado_en");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Pendiente'")
                .HasColumnName("estado");
            entity.Property(e => e.EstudianteId).HasColumnName("estudiante_id");
            entity.Property(e => e.FechaHora)
                .HasColumnType("datetime")
                .HasColumnName("fecha_hora");
            entity.Property(e => e.Motivo)
                .HasMaxLength(500)
                .HasColumnName("motivo");
            entity.Property(e => e.MotivoRechazo)
                .HasMaxLength(500)
                .HasColumnName("motivo_rechazo");
            entity.Property(e => e.RespondidoEn)
                .HasColumnType("datetime")
                .HasColumnName("respondido_en");

            entity.HasOne(d => d.Consejero).WithMany(p => p.CitaConsejeros)
                .HasForeignKey(d => d.ConsejeroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_citas_consejero");

            entity.HasOne(d => d.Estudiante).WithMany(p => p.CitaEstudiantes)
                .HasForeignKey(d => d.EstudianteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_citas_estudiante");
        }
    }
}
