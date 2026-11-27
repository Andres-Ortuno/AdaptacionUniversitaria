using AdaptacionUniversitaria.Core.Entities;

namespace AdaptacionUniversitaria.Core.Interfaces
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<Usuario?> GetByIdConRolAsync(int id);
        Task<Usuario?> GetByEmailAsync(string email);
        Task<IEnumerable<Usuario>> GetAllConRolAsync();
        Task<IEnumerable<Usuario>> GetConsejerosActivosAsync();
    }
}
