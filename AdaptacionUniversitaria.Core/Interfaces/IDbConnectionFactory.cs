using System.Data;

namespace AdaptacionUniversitaria.Core.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
