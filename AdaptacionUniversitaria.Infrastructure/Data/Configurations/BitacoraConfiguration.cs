using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AdaptacionUniversitaria.Core.Entities;

namespace AdaptacionUniversitaria.Infrastructure.Data.Configurations
{
    public class BitacoraConfiguration :
        IEntityTypeConfiguration<Bitacora>
    {
        public void Configure(EntityTypeBuilder<Bitacora> entity) {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("bitacoras");

            entity.HasIndex(e => new { e.EstudianteId, e.Fecha }, "uk_bitacoras_estudiante_fecha").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreadoEn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("creado_en");
            entity.Property(e => e.EstadoAnimo).HasColumnName("estado_animo");
            entity.Property(e => e.EstudianteId).HasColumnName("estudiante_id");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.Nota)
                .HasColumnType("text")
                .HasColumnName("nota");
            entity.Property(e => e.Sintomas)
                .HasMaxLength(255)
                .HasColumnName("sintomas");

            entity.HasOne(d => d.Estudiante).WithMany(p => p.Bitacoras)
                .HasForeignKey(d => d.EstudianteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bitacoras_estudiante");
        }
    }
}
