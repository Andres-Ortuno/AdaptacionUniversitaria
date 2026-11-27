using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.QueryFilters;

namespace AdaptacionUniversitaria.Services.Interfaces
{
    public interface ICitaService
    {
        Task<IEnumerable<Usuario>> GetConsejerosAsync(int usuarioId);
        Task<IEnumerable<Cita>> GetCitasAsync(int usuarioId, CitaQueryFilters filters);
        Task<Cita> SolicitarAsync(int usuarioId, Cita cita);
        Task<Cita> AprobarAsync(int usuarioId, int id);
        Task<Cita> RechazarAsync(int usuarioId, int id, string motivoRechazo);
        Task<Cita> CancelarAsync(int usuarioId, int id);
    }
}
