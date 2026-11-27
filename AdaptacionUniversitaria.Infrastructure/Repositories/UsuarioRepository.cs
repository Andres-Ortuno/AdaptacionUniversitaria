using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.Enums;
using AdaptacionUniversitaria.Core.Interfaces;
using AdaptacionUniversitaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AdaptacionUniversitaria.Infrastructure.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AdaptacionUniversitariaContext context) : base(context) { }

        public async Task<Usuario?> GetByIdConRolAsync(int id)
        {
            return await _entities.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _entities.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<Usuario>> GetAllConRolAsync()
        {
            return await _entities.AsNoTracking().Include(u => u.Rol).OrderBy(u => u.Id).ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> GetConsejerosActivosAsync()
        {
            return await _entities.AsNoTracking()
                .Include(u => u.Rol)
                .Where(u => u.Activo && u.Rol.Nombre == RolNombre.Consejero)
                .OrderBy(u => u.Apellidos).ThenBy(u => u.Nombres)
                .ToListAsync();
        }
    }
}
