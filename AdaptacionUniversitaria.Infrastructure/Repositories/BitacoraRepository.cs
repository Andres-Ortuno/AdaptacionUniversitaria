using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.Interfaces;
using AdaptacionUniversitaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AdaptacionUniversitaria.Infrastructure.Queries;

namespace AdaptacionUniversitaria.Infrastructure.Repositories
{
    public class BitacoraRepository : BaseRepository<Bitacora>, IBitacoraRepository
    {
        private readonly IDapperContext _dapper;
        public BitacoraRepository(AdaptacionUniversitariaContext context, IDapperContext dapper)
    : base(context)
        {
            _dapper = dapper;
        }

        private class BitacoraRow
        {
            public int Id { get; set; }
            public int EstudianteId { get; set; }
            public DateTime Fecha { get; set; }
            public int EstadoAnimo { get; set; }
            public string? Sintomas { get; set; }
            public string? Nota { get; set; }
            public DateTime CreadoEn { get; set; }
        }

        public async Task<IEnumerable<Bitacora>> GetHistorialDapperAsync(
            int estudianteId, DateOnly? desde, DateOnly? hasta)
        {
            var filas = await _dapper.QueryAsync<BitacoraRow>(
                BitacorasQueries.historialMySql,
                new
                {
                    EstudianteId = estudianteId,
                    Desde = desde?.ToDateTime(TimeOnly.MinValue),
                    Hasta = hasta?.ToDateTime(TimeOnly.MinValue)
                });

            return filas.Select(f => new Bitacora
            {
                Id = f.Id,
                EstudianteId = f.EstudianteId,
                Fecha = DateOnly.FromDateTime(f.Fecha),
                EstadoAnimo = f.EstadoAnimo,
                Sintomas = f.Sintomas,
                Nota = f.Nota,
                CreadoEn = f.CreadoEn
            });
        }

        public async Task<Bitacora?> GetByEstudianteYFechaAsync(int estudianteId, DateOnly fecha)
        {
            return await _entities.FirstOrDefaultAsync(b => b.EstudianteId == estudianteId && b.Fecha == fecha);
        }

        public async Task<IEnumerable<Bitacora>> GetHistorialAsync(int estudianteId, DateOnly? desde, DateOnly? hasta)
        {
            var query = _entities.AsNoTracking().Where(b => b.EstudianteId == estudianteId);

            if (desde.HasValue)
                query = query.Where(b => b.Fecha >= desde.Value);
            if (hasta.HasValue)
                query = query.Where(b => b.Fecha <= hasta.Value);

            // Del más reciente al más antiguo
            return await query.OrderByDescending(b => b.Fecha).ToListAsync();
        }
    }
}
