using AdaptacionUniversitaria.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace AdaptacionUniversitaria.Infrastructure.Repositories
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _mySqlConn;

        public DbConnectionFactory(IConfiguration config)
        {
            _mySqlConn = config.GetConnectionString("ConnectionMySql") ?? string.Empty;
        }

        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(_mySqlConn);
        }
    }
}
