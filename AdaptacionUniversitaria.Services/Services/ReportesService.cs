using System;
using System.Collections.Generic;
using System.Text;
using AdaptacionUniversitaria.Core.CustomEntities;
using AdaptacionUniversitaria.Core.CustomEntities.Reporteria;
using AdaptacionUniversitaria.Core.Enums;
using AdaptacionUniversitaria.Core.Exceptions;
using AdaptacionUniversitaria.Core.Helpers;
using AdaptacionUniversitaria.Core.Interfaces;
using AdaptacionUniversitaria.Services.Interfaces;

namespace AdaptacionUniversitaria.Services.Services
{
    public class ReportesService : ServiceBase, IReportesService
    {
        public ReportesService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<IEnumerable<CantidadCitasEstado>> GetCantidadCitasEstadoAsyn()
        {
            return await _unitOfWork.ReportesRepository.GetCantidadCitasEstadoAsync();
        }

        public async Task<IEnumerable<EstudianteAnimo>> GetEstudiantesAnimoBajoAsync(
            int usuarioId, int dias, decimal umbral)
        {
            var consejero = await ObtenerUsuarioActualAsync(usuarioId, RolNombre.Consejero);

            if (dias < 1 || dias > 90)
                throw new BusinessException("El parámetro 'dias' debe estar entre 1 y 90.");
            if (umbral < 1 || umbral > 5)
                throw new BusinessException("El parámetro 'umbral' debe estar entre 1 y 5.");

            var desde = Procesos.AhoraBolivia().Date.AddDays(-dias);

            return await _unitOfWork.ReportesRepository
                .GetEstudiantesAnimoBajoAsync(consejero.Id, desde, umbral);
        }

        public async Task<IEnumerable<PromedioAnimoEstudiante>> GetPromedioAnimoEstudiantesAsync()
        {
            return await _unitOfWork.ReportesRepository.GetPromedioAnimoEstudiantesAsync();
        }
    }
}