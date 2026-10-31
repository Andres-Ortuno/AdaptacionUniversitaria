using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.Exceptions;
using AdaptacionUniversitaria.Core.Interfaces;
using AdaptacionUniversitaria.Services.Interfaces;
using System.Net;

namespace AdaptacionUniversitaria.Services.Services
{
    public class CitaService : ICitaService
    {
        private readonly IBaseRepository<Cita> _citaRepository;
        private readonly IBaseRepository<Usuario> _usuarioRepository;

        public CitaService(
            IBaseRepository<Cita> citaRepository,
            IBaseRepository<Usuario> usuarioRepository)
        {
            _citaRepository = citaRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<Cita>> GetCitasAsync(int? estudianteId, int? consejeroId, string? estado)
        {
            var citas = (await _citaRepository.GetAll()).AsEnumerable();

            if (estudianteId.HasValue)
                citas = citas.Where(c => c.EstudianteId == estudianteId.Value);
            if (consejeroId.HasValue)
                citas = citas.Where(c => c.ConsejeroId == consejeroId.Value);
            if (!string.IsNullOrWhiteSpace(estado))
                citas = citas.Where(c => string.Equals(c.Estado, estado, StringComparison.OrdinalIgnoreCase));

            return citas.OrderBy(c => c.FechaHora).ToList();
        }

        public async Task<Cita> GetCitaByIdAsync(int id)
        {
            return await _citaRepository.GetById(id);
        }

        // CU-03: solicitar cita de apoyo
        public async Task SolicitarCita(Cita cita)
        {
            var estudiante = await _usuarioRepository.GetById(cita.EstudianteId);
            if (estudiante == null || estudiante.RolId != RolUsuario.Estudiante)
            {
                throw new BusinessException("El estudiante no existe", HttpStatusCode.NotFound);
            }

            var consejero = await _usuarioRepository.GetById(cita.ConsejeroId);
            if (consejero == null || consejero.RolId != RolUsuario.Consejero)
            {
                throw new BusinessException("El consejero no existe", HttpStatusCode.NotFound);
            }

            // RN-03: una sola cita pendiente por estudiante
            var citas = await _citaRepository.GetAll();
            if (citas.Any(c => c.EstudianteId == cita.EstudianteId && c.Estado == EstadoCita.Pendiente))
            {
                throw new BusinessException(
                    "Ya tiene una cita pendiente: espere la respuesta del consejero", HttpStatusCode.Conflict);
            }

            cita.Id = 0;
            cita.Estado = EstadoCita.Pendiente;
            cita.MotivoRechazo = null;
            cita.CreadoEn = DateTime.Now;
            cita.RespondidoEn = null;

            await _citaRepository.Add(cita);
        }

        // CU-04: aprobar cita
        public async Task<Cita> AprobarCita(int id, int consejeroId)
        {
            var cita = await _citaRepository.GetById(id);
            if (cita == null)
            {
                throw new BusinessException("La cita no existe", HttpStatusCode.NotFound);
            }

            // RN-05: el consejero solo responde citas dirigidas a él y que sigan pendientes
            if (cita.ConsejeroId != consejeroId)
            {
                throw new BusinessException(
                    "La cita no está dirigida a este consejero", HttpStatusCode.Forbidden);
            }

            if (cita.Estado != EstadoCita.Pendiente)
            {
                throw new BusinessException(
                    "La cita ya fue respondida o cancelada", HttpStatusCode.Conflict);
            }

            cita.Estado = EstadoCita.Aprobada;
            cita.RespondidoEn = DateTime.Now;

            await _citaRepository.Update(cita);
            return cita;
        }
    }
}
