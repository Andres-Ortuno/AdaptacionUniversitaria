using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.QueryFilters;

namespace AdaptacionUniversitaria.Services.Interfaces
{
    public interface IBitacoraService
    {
        Task<IEnumerable<Bitacora>> GetHistorialAsync(int usuarioId, BitacoraQueryFilters filters);
        Task<Bitacora> RegistrarAsync(int usuarioId, Bitacora bitacora);
        Task<Bitacora> EditarAsync(int usuarioId, int id, Bitacora cambios);
    }
}
