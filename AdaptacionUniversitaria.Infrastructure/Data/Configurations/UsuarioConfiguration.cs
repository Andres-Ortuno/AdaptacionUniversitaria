using AdaptacionUniversitaria.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdaptacionUniversitaria.Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuarios");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.RolId).HasColumnName("rol_id");
            builder.Property(e => e.Nombres).HasColumnName("nombres").HasMaxLength(100).IsRequired();
            builder.Property(e => e.Apellidos).HasColumnName("apellidos").HasMaxLength(100).IsRequired();
            builder.Property(e => e.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
            builder.Property(e => e.PasswordHash).HasColumnName("password_hash").HasMaxLength(255).IsRequired();
            // Sin valor por defecto en EF: el servicio siempre fija Activo de forma explícita.
            builder.Property(e => e.Activo).HasColumnName("activo").HasColumnType("tinyint(1)");
            builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasIndex(e => e.Email).IsUnique().HasDatabaseName("uk_usuarios_email");

            builder.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_usuarios_rol");
        }
    }
}
