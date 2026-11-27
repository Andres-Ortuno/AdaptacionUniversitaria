using AdaptacionUniversitaria.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AdaptacionUniversitaria.Infrastructure.Data;

public partial class AdaptacionUniversitariaContext : DbContext
{
    public AdaptacionUniversitariaContext()
    {
    }

    public AdaptacionUniversitariaContext(DbContextOptions<AdaptacionUniversitariaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Rol> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Bitacora> Bitacoras { get; set; }

    public virtual DbSet<Cita> Citas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
