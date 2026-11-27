using System;
using System.Collections.Generic;
using System.Text;
using AdaptacionUniversitaria.Core.CustomEntities;
using AdaptacionUniversitaria.Core.CustomEntities.Reporteria;
using AdaptacionUniversitaria.Core.Interfaces;
using AdaptacionUniversitaria.Infrastructure.Queries;

namespace AdaptacionUniversitaria.Infrastructure.Repositories
{
    public class ReportesRepository : IReportesRepository
    {
        private readonly IDapperContext _dapper;

        public ReportesRepository(IDapperContext dapper)
        {
            _dapper = dapper;
        }

        public async Task<IEnumerable<CantidadCitasEstado>> GetCantidadCitasEstadoAsync()
        {
            try
            {
                return await _dapper.QueryAsync<CantidadCitasEstado>(
                    ReportesQueries.cantidadCitasEstado);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<EstudianteAnimo>> GetEstudiantesAnimoBajoAsync(
            int consejeroId, DateTime desde, decimal umbral)
        {
            return await _dapper.QueryAsync<EstudianteAnimo>(
                ReportesQueries.estudiantesAnimoBajoMySql,
                new { ConsejeroId = consejeroId, Desde = desde, Umbral = umbral });
        }

        public async Task<IEnumerable<PromedioAnimoEstudiante>> GetPromedioAnimoEstudiantesAsync()
        {
            try
            {
                return await _dapper.QueryAsync<PromedioAnimoEstudiante>(
                    ReportesQueries.promedioAnimoEstudianteMySql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}