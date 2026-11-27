using AdaptacionUniversitaria.Core.Interfaces;
using Dapper;
using System.Data;
using System.Data.Common;

namespace AdaptacionUniversitaria.Infrastructure.Data
{
    public class DapperContext : IDapperContext
    {
        private readonly IDbConnectionFactory _connFactory;

        private static readonly AsyncLocal<(IDbConnection? Conn, IDbTransaction? Tx)>
            _ambient = new();

        public DapperContext(IDbConnectionFactory connFactory)
        {
            _connFactory = connFactory;
        }

        // UnitOfWork lo llama al iniciar la transacción
        public void SetAmbientConnection(IDbConnection conn, IDbTransaction? tx)
        {
            _ambient.Value = (conn, tx);
        }

        // UnitOfWork lo llama al finalizar / rollback
        public void ClearAmbientConnection()
        {
            _ambient.Value = (null, null);
        }

        /// <summary>
        /// Si hay UnitOfWork activo usa la misma conexión y transacción;
        /// si no, crea una conexión nueva desde IDbConnectionFactory.
        /// ownsConnection indica si este contexto debe cerrar la conexión.
        /// </summary>
        private (IDbConnection conn, IDbTransaction? tx, bool ownsConnection) GetConnAndTx()
        {
            var ambient = _ambient.Value;
            if (ambient.Conn != null)
                return (ambient.Conn, ambient.Tx, false);

            return (_connFactory.CreateConnection(), null, true);
        }

        private static async Task OpenIfNeededAsync(IDbConnection conn)
        {
            if (conn is DbConnection dbConn && dbConn.State == ConnectionState.Closed)
                await dbConn.OpenAsync();
        }

        private static async Task CloseIfOwnedAsync(IDbConnection conn, bool owns)
        {
            if (!owns) return;
            if (conn is DbConnection dbConn && dbConn.State != ConnectionState.Closed)
                await dbConn.CloseAsync();
            conn.Dispose();
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null,
            CommandType commandType = CommandType.Text)
        {
            var (conn, tx, owns) = GetConnAndTx();
            try
            {
                await OpenIfNeededAsync(conn);
                return await conn.QueryAsync<T>(new CommandDefinition(sql, param, tx, commandType: commandType));
            }
            finally
            {
                await CloseIfOwnedAsync(conn, owns);
            }
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null,
            CommandType commandType = CommandType.Text)
        {
            var (conn, tx, owns) = GetConnAndTx();
            try
            {
                await OpenIfNeededAsync(conn);
                return await conn.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, param, tx, commandType: commandType));
            }
            finally
            {
                await CloseIfOwnedAsync(conn, owns);
            }
        }

        public async Task<int> ExecuteAsync(string sql, object? param = null,
            CommandType commandType = CommandType.Text)
        {
            var (conn, tx, owns) = GetConnAndTx();
            try
            {
                await OpenIfNeededAsync(conn);
                return await conn.ExecuteAsync(new CommandDefinition(sql, param, tx, commandType: commandType));
            }
            finally
            {
                await CloseIfOwnedAsync(conn, owns);
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, object? param = null,
            CommandType commandType = CommandType.Text)
        {
            var (conn, tx, owns) = GetConnAndTx();
            try
            {
                await OpenIfNeededAsync(conn);
                var res = await conn.ExecuteScalarAsync(new CommandDefinition(sql, param, tx, commandType: commandType));
                if (res == null || res == DBNull.Value) return default!;
                return (T)Convert.ChangeType(res, typeof(T));
            }
            finally
            {
                await CloseIfOwnedAsync(conn, owns);
            }
        }
    }
}
