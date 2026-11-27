using System;
using System.Collections.Generic;
using System.Text;
using AdaptacionUniversitaria.Core.CustomEntities;
using AdaptacionUniversitaria.Core.CustomEntities.Reporteria;

namespace AdaptacionUniversitaria.Core.Interfaces
{
    public interface IReportesRepository
    {
        Task<IEnumerable<EstudianteAnimo>> GetEstudiantesAnimoBajoAsync(
            int consejeroId, DateTime desde, decimal umbral);

        Task<IEnumerable<CantidadCitasEstado>> GetCantidadCitasEstadoAsync();
        Task<IEnumerable<PromedioAnimoEstudiante>> GetPromedioAnimoEstudiantesAsync();
    }
}