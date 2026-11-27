using System;
using System.Collections.Generic;
using System.Text;

using AdaptacionUniversitaria.Core.CustomEntities.Reporteria;

namespace AdaptacionUniversitaria.Services.Interfaces
{
    public interface IReportesService
    {
        Task<IEnumerable<EstudianteAnimo>> GetEstudiantesAnimoBajoAsync(
            int usuarioId, int dias, decimal umbral);
    }
}