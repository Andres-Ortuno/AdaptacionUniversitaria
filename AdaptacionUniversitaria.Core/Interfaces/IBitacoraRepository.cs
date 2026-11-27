using AdaptacionUniversitaria.Core.Entities;

namespace AdaptacionUniversitaria.Core.Interfaces
{
    public interface IBitacoraRepository : IBaseRepository<Bitacora>
    {
        Task<Bitacora?> GetByEstudianteYFechaAsync(int estudianteId, DateOnly fecha);
        Task<IEnumerable<Bitacora>> GetHistorialAsync(int estudianteId, DateOnly? desde, DateOnly? hasta);
        Task<IEnumerable<Bitacora>> GetHistorialDapperAsync(int estudianteId, DateOnly? desde, DateOnly? hasta);
    }
}
