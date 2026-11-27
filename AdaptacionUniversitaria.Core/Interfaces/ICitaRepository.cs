using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.Enums;

namespace AdaptacionUniversitaria.Core.Interfaces
{
    public interface ICitaRepository : IBaseRepository<Cita>
    {
        Task<Cita?> GetByIdConUsuariosAsync(int id);
        Task<bool> ExistePendienteAsync(int estudianteId);
        Task<IEnumerable<Cita>> GetByEstudianteAsync(int estudianteId, EstadoCita? estado);
        Task<IEnumerable<Cita>> GetByConsejeroAsync(int consejeroId, EstadoCita? estado);

        // Dapper: ¿el consejero tiene al menos una cita aprobada con el estudiante? (RN-10)
        Task<bool> ExisteCitaAprobadaAsync(int consejeroId, int estudianteId);
    }
}
