using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.Enums;
using AdaptacionUniversitaria.Core.Interfaces;
using AdaptacionUniversitaria.Infrastructure.Data;
using AdaptacionUniversitaria.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace AdaptacionUniversitaria.Infrastructure.Repositories
{
    public class CitaRepository : BaseRepository<Cita>, ICitaRepository
    {
        private readonly IDapperContext _dapper;

        public CitaRepository(AdaptacionUniversitariaContext context, IDapperContext dapper)
            : base(context)
        {
            _dapper = dapper;
        }

        public async Task<Cita?> GetByIdConUsuariosAsync(int id)
        {
            return await _entities
                .Include(c => c.Estudiante)
                .Include(c => c.Consejero)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistePendienteAsync(int estudianteId)
        {
            return await _entities.AnyAsync(c =>
                c.EstudianteId == estudianteId && c.Estado == EstadoCita.Pendiente);
        }

        public async Task<IEnumerable<Cita>> GetByEstudianteAsync(int estudianteId, EstadoCita? estado)
        {
            var query = _entities.AsNoTracking()
                .Include(c => c.Estudiante)
                .Include(c => c.Consejero)
                .Where(c => c.EstudianteId == estudianteId);

            if (estado.HasValue)
                query = query.Where(c => c.Estado == estado.Value);

            return await query.OrderByDescending(c => c.FechaHora).ToListAsync();
        }

        public async Task<IEnumerable<Cita>> GetByConsejeroAsync(int consejeroId, EstadoCita? estado)
        {
            var query = _entities.AsNoTracking()
                .Include(c => c.Estudiante)
                .Include(c => c.Consejero)
                .Where(c => c.ConsejeroId == consejeroId);

            if (estado.HasValue)
                query = query.Where(c => c.Estado == estado.Value);

            return await query.OrderByDescending(c => c.FechaHora).ToListAsync();
        }

        public async Task<bool> ExisteCitaAprobadaAsync(int consejeroId, int estudianteId)
        {
            var total = await _dapper.ExecuteScalarAsync<int>(
                CitasQueries.existeCitaAprobadaMySql,
                new { ConsejeroId = consejeroId, EstudianteId = estudianteId });
            return total > 0;
        }
    }
}
