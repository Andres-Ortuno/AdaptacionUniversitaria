using AdaptacionUniversitaria.Core.Entities;
using System.Data;

namespace AdaptacionUniversitaria.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Rol> RolRepository { get; }
        IUsuarioRepository UsuarioRepository { get; }
        IBitacoraRepository BitacoraRepository { get; }
        ICitaRepository CitaRepository { get; }
        IReportesRepository ReportesRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();

        // Transacciones (EF + Dapper comparten conexión)
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        IDbConnection? GetDbConnection();
        IDbTransaction? GetDbTransaction();
    }
}
