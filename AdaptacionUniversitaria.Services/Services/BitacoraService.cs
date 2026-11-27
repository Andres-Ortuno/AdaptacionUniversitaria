using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.Enums;
using AdaptacionUniversitaria.Core.Exceptions;
using AdaptacionUniversitaria.Core.Helpers;
using AdaptacionUniversitaria.Core.Interfaces;
using AdaptacionUniversitaria.Core.QueryFilters;
using AdaptacionUniversitaria.Services.Interfaces;
using System.Net;

namespace AdaptacionUniversitaria.Services.Services
{
    public class BitacoraService : ServiceBase, IBitacoraService
    {
        public BitacoraService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        // CU-01: Consultar historial de bitácoras
        public async Task<IEnumerable<Bitacora>> GetHistorialAsync(int usuarioId, BitacoraQueryFilters filters)
        {
            var usuario = await ObtenerUsuarioActualAsync(usuarioId, RolNombre.Estudiante, RolNombre.Consejero);

            if (filters.FechaDesde.HasValue && filters.FechaHasta.HasValue
                && filters.FechaDesde.Value > filters.FechaHasta.Value)
            {
                throw new BusinessException("La fecha inicial no puede ser posterior a la fecha final.");
            }

            int estudianteId;

            if (usuario.Rol.Nombre == RolNombre.Estudiante)
            {
                // RN-10: un estudiante solo ve su propia información
                if (filters.EstudianteId.HasValue && filters.EstudianteId.Value != usuario.Id)
                    throw new BusinessException("Solo puede consultar su propia información.", HttpStatusCode.Forbidden);

                estudianteId = usuario.Id;
            }
            else
            {
                if (!filters.EstudianteId.HasValue)
                    throw new BusinessException("Debe indicar el estudiante a consultar (estudianteId).");

                estudianteId = filters.EstudianteId.Value;

                // RN-10: el consejero solo ve estudiantes con los que tiene una cita aprobada
                var autorizado = await _unitOfWork.CitaRepository.ExisteCitaAprobadaAsync(usuario.Id, estudianteId);
                if (!autorizado)
                    throw new BusinessException(
                        "Acceso denegado: no tiene una cita aprobada con este estudiante.", HttpStatusCode.Forbidden);
            }

            return await _unitOfWork.BitacoraRepository.GetHistorialDapperAsync(
                estudianteId, filters.FechaDesde, filters.FechaHasta);
        }

        // CU-02: Registrar bitácora emocional
        public async Task<Bitacora> RegistrarAsync(int usuarioId, Bitacora bitacora)
        {
            var usuario = await ObtenerUsuarioActualAsync(usuarioId, RolNombre.Estudiante);

            var hoy = Procesos.HoyBolivia();

            // RN-01: una sola bitácora por día
            var existente = await _unitOfWork.BitacoraRepository.GetByEstudianteYFechaAsync(usuario.Id, hoy);
            if (existente != null)
                throw new BusinessException("La bitácora de hoy ya fue registrada.", HttpStatusCode.Conflict);

            bitacora.Id = 0;
            bitacora.EstudianteId = usuario.Id;
            bitacora.Fecha = hoy;
            bitacora.CreadoEn = Procesos.AhoraBolivia();

            await _unitOfWork.BitacoraRepository.Add(bitacora);
            await _unitOfWork.SaveChangesAsync();
            return bitacora;
        }

        // CU-06: Editar bitácora del día
        public async Task<Bitacora> EditarAsync(int usuarioId, int id, Bitacora cambios)
        {
            var usuario = await ObtenerUsuarioActualAsync(usuarioId, RolNombre.Estudiante);

            var bitacora = await _unitOfWork.BitacoraRepository.GetById(id);
            if (bitacora == null || bitacora.EstudianteId != usuario.Id)
                throw new BusinessException("Bitácora no encontrada.", HttpStatusCode.NotFound);

            // RN-11: solo se edita la bitácora del día actual
            if (bitacora.Fecha != Procesos.HoyBolivia())
                throw new BusinessException("Solo se puede editar la bitácora del día.", HttpStatusCode.Conflict);

            bitacora.EstadoAnimo = cambios.EstadoAnimo;
            bitacora.Sintomas = cambios.Sintomas;
            bitacora.Nota = cambios.Nota;

            await _unitOfWork.BitacoraRepository.Update(bitacora);
            await _unitOfWork.SaveChangesAsync();
            return bitacora;
        }


    }
}
