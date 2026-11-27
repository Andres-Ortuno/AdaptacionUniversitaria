using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.Interfaces;
using AdaptacionUniversitaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AdaptacionUniversitaria.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : BaseEntity
    {
        protected readonly AdaptacionUniversitariaContext _context; // Base de datos
        protected readonly DbSet<T> _entities;                      // Tabla

        public BaseRepository(AdaptacionUniversitariaContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public Task Add(T entity)
        {
            _entities.Add(entity); // Insert (el commit lo hace UnitOfWork.SaveChangesAsync)
            return Task.CompletedTask;
        }

        public Task Update(T entity)
        {
            _entities.Update(entity);
            return Task.CompletedTask;
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            if (entity != null)
                _entities.Remove(entity);
        }
    }
}
