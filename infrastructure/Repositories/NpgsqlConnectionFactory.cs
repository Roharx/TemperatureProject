using Npgsql;
using infrastructure.Interfaces;

namespace infrastructure.Repositories
{
    public class NpgsqlConnectionFactory : INpgsqlConnectionFactory
    {
        private readonly string _connectionString;

        public NpgsqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}