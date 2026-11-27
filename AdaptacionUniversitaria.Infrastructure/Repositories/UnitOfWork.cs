using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.Interfaces;
using AdaptacionUniversitaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace AdaptacionUniversitaria.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AdaptacionUniversitariaContext _context;
        private readonly IDapperContext _dapper;
        private IDbContextTransaction? _efTransaction;

        private IBaseRepository<Rol>? _rolRepository;
        private IUsuarioRepository? _usuarioRepository;
        private IBitacoraRepository? _bitacoraRepository;
        private ICitaRepository? _citaRepository;
        private IReportesRepository? _reportesRepository;

        public UnitOfWork(AdaptacionUniversitariaContext context, IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }

        public IBaseRepository<Rol> RolRepository =>
            _rolRepository ??= new BaseRepository<Rol>(_context);

        public IUsuarioRepository UsuarioRepository =>
            _usuarioRepository ??= new UsuarioRepository(_context);

        public IBitacoraRepository BitacoraRepository =>
            _bitacoraRepository ??= new BitacoraRepository(_context, _dapper);

        public ICitaRepository CitaRepository =>
            _citaRepository ??= new CitaRepository(_context, _dapper);
        public IReportesRepository ReportesRepository =>
            _reportesRepository ??= new ReportesRepository(_dapper);

        public void Dispose()
        {
            _efTransaction?.Dispose();
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #region Transacciones (EF + Dapper)
        public async Task BeginTransactionAsync()
        {
            if (_efTransaction == null)
            {
                _efTransaction = await _context.Database.BeginTransactionAsync();

                // Registrar la conexión/transacción en DapperContext
                var conn = _context.Database.GetDbConnection();
                var tx = _efTransaction.GetDbTransaction();
                _dapper.SetAmbientConnection(conn, tx);
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_efTransaction != null)
                {
                    await _efTransaction.CommitAsync();
                    _efTransaction.Dispose();
                    _efTransaction = null;
                }
            }
            finally
            {
                _dapper.ClearAmbientConnection();
            }
        }

        public async Task RollbackAsync()
        {
            if (_efTransaction != null)
            {
                await _efTransaction.RollbackAsync();
                _efTransaction.Dispose();
                _efTransaction = null;
            }
            _dapper.ClearAmbientConnection();
        }

        public IDbConnection? GetDbConnection()
        {
            return _context.Database.GetDbConnection();
        }

        public IDbTransaction? GetDbTransaction()
        {
            return _efTransaction?.GetDbTransaction();
        }
        #endregion
    }
}
