using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.Enums;
using AdaptacionUniversitaria.Core.Exceptions;
using AdaptacionUniversitaria.Core.Helpers;
using AdaptacionUniversitaria.Core.Interfaces;
using AdaptacionUniversitaria.Core.QueryFilters;
using AdaptacionUniversitaria.Services.Interfaces;
using System.Net;
using System.Globalization;

namespace AdaptacionUniversitaria.Services.Services
{
    public class CitaService : ServiceBase, ICitaService
    {
        public CitaService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        // CU-03 (paso 2): lista de consejeros disponibles
        public async Task<IEnumerable<Usuario>> GetConsejerosAsync(int usuarioId)
        {
            await ObtenerUsuarioActualAsync(usuarioId, RolNombre.Estudiante);
            return await _unitOfWork.UsuarioRepository.GetConsejerosActivosAsync();
        }

        // CU-04 / CU-05 / CU-07: el estudiante ve las suyas y el consejero las que se le asignan
        public async Task<IEnumerable<Cita>> GetCitasAsync(int usuarioId, CitaQueryFilters filters)
        {
            var usuario = await ObtenerUsuarioActualAsync(usuarioId, RolNombre.Estudiante, RolNombre.Consejero);

            return usuario.Rol.Nombre == RolNombre.Estudiante
                ? await _unitOfWork.CitaRepository.GetByEstudianteAsync(usuario.Id, filters.Estado)
                : await _unitOfWork.CitaRepository.GetByConsejeroAsync(usuario.Id, filters.Estado);
        }

        // CU-03: Solicitar cita de apoyo
        public async Task<Cita> SolicitarAsync(int usuarioId, Cita cita)
        {
            var estudiante = await ObtenerUsuarioActualAsync(usuarioId, RolNombre.Estudiante);

            var consejero = await _unitOfWork.UsuarioRepository.GetByIdConRolAsync(cita.ConsejeroId);
            if (consejero == null || consejero.Rol.Nombre != RolNombre.Consejero || !consejero.Activo)
                throw new BusinessException("El consejero seleccionado no existe o no está disponible.",
                    HttpStatusCode.NotFound);

            // RN-04: la fecha y hora deben ser posteriores al momento de la solicitud
            var ahora = Procesos.AhoraBolivia();
            if (cita.FechaHora <= ahora)
                throw new BusinessException("La fecha y hora de la cita deben ser posteriores al momento actual.");

            // RN-N01 La cita debe ser para un día futuro
            if (!HorarioAtencion.EsDiaFuturo(cita.FechaHora, ahora))
                throw new BusinessException(
                    "La cita debe solicitarse para un día futuro (a partir de mañana).",
                    HttpStatusCode.BadRequest,
                    new { fechaHoraSolicitada = cita.FechaHora, horarioPermitido = HorarioAtencion.Descripcion });

            // El día debe ser de atención
            if (!HorarioAtencion.EsDiaDeAtencion(cita.FechaHora))
                throw new BusinessException(
                    $"No se atiende los días {HorarioAtencion.NombreDia(cita.FechaHora.DayOfWeek)}. " +
                    "Las citas se atienden de lunes a viernes.",
                    HttpStatusCode.BadRequest,
                    new { fechaHoraSolicitada = cita.FechaHora, horarioPermitido = HorarioAtencion.Descripcion });

            // La hora debe estar dentro del horario
            if (!HorarioAtencion.EstaEnHorario(cita.FechaHora))
                throw new BusinessException(
                    $"La hora solicitada ({cita.FechaHora.Hour:00}:{cita.FechaHora.Minute:00}) está fuera del horario de atención: " +
                    $"de {HorarioAtencion.Apertura:HH\\:mm} a {HorarioAtencion.Cierre:HH\\:mm}.",
                    HttpStatusCode.BadRequest,
                    new { fechaHoraSolicitada = cita.FechaHora, horarioPermitido = HorarioAtencion.Descripcion });

            // RN-03: no puede tener más de una cita pendiente
            if (await _unitOfWork.CitaRepository.ExistePendienteAsync(estudiante.Id))
                throw new BusinessException(
                    "Ya tiene una cita pendiente: espere la respuesta o cancélela antes de solicitar otra.",
                    HttpStatusCode.Conflict);

            cita.Id = 0;
            cita.EstudianteId = estudiante.Id;
            cita.Estado = EstadoCita.Pendiente;
            cita.MotivoRechazo = null;
            cita.RespondidoEn = null;
            cita.CreadoEn = ahora;

            await _unitOfWork.CitaRepository.Add(cita);
            await _unitOfWork.SaveChangesAsync();
            return cita;
        }

        // CU-04: Aprobar cita
        public async Task<Cita> AprobarAsync(int usuarioId, int id)
        {
            var cita = await ObtenerCitaParaResponderAsync(usuarioId, id);

            cita.Estado = EstadoCita.Aprobada;
            cita.RespondidoEn = Procesos.AhoraBolivia();

            await _unitOfWork.CitaRepository.Update(cita);
            await _unitOfWork.SaveChangesAsync();
            return cita;
        }

        // CU-05: Rechazar cita
        public async Task<Cita> RechazarAsync(int usuarioId, int id, string motivoRechazo)
        {
            // RN-06: el motivo del rechazo es obligatorio
            if (string.IsNullOrWhiteSpace(motivoRechazo))
                throw new BusinessException("Debe indicar el motivo del rechazo.");

            var cita = await ObtenerCitaParaResponderAsync(usuarioId, id);

            cita.Estado = EstadoCita.Rechazada;
            cita.MotivoRechazo = motivoRechazo.Trim();
            cita.RespondidoEn = Procesos.AhoraBolivia();

            await _unitOfWork.CitaRepository.Update(cita);
            await _unitOfWork.SaveChangesAsync();
            return cita;
        }

        // CU-07: Cancelar cita pendiente
        public async Task<Cita> CancelarAsync(int usuarioId, int id)
        {
            var estudiante = await ObtenerUsuarioActualAsync(usuarioId, RolNombre.Estudiante);

            var cita = await _unitOfWork.CitaRepository.GetByIdConUsuariosAsync(id);
            if (cita == null || cita.EstudianteId != estudiante.Id)
                throw new BusinessException("Cita no encontrada.", HttpStatusCode.NotFound);

            // RN-07: solo se cancelan las citas pendientes
            if (cita.Estado != EstadoCita.Pendiente)
                throw new BusinessException("Solo se pueden cancelar las citas pendientes.", HttpStatusCode.Conflict);

            cita.Estado = EstadoCita.Cancelada;

            await _unitOfWork.CitaRepository.Update(cita);
            await _unitOfWork.SaveChangesAsync();
            return cita;
        }

        // RN-05: el consejero solo responde citas dirigidas a él y que sigan pendientes
        private async Task<Cita> ObtenerCitaParaResponderAsync(int usuarioId, int id)
        {
            var consejero = await ObtenerUsuarioActualAsync(usuarioId, RolNombre.Consejero);

            var cita = await _unitOfWork.CitaRepository.GetByIdConUsuariosAsync(id);
            if (cita == null)
                throw new BusinessException("Cita no encontrada.", HttpStatusCode.NotFound);

            if (cita.ConsejeroId != consejero.Id)
                throw new BusinessException("Esta cita no está dirigida a usted.", HttpStatusCode.Forbidden);

            if (cita.Estado != EstadoCita.Pendiente)
                throw new BusinessException("La cita ya no está pendiente.", HttpStatusCode.Conflict);

            return cita;
        }
    }
}
