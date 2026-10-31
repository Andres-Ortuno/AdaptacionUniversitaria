using AdaptacionUniversitaria.Core.Entities;

namespace AdaptacionUniversitaria.Services.Interfaces
{
    public interface ICitaService
    {
        Task<IEnumerable<Cita>> GetCitasAsync(int? estudianteId, int? consejeroId, string? estado);
        Task<Cita> GetCitaByIdAsync(int id);
        Task SolicitarCita(Cita cita);
        Task<Cita> AprobarCita(int id, int consejeroId);
    }
}
