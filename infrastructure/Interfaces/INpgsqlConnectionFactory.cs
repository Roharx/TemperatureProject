using Npgsql;

namespace infrastructure.Interfaces;

public interface INpgsqlConnectionFactory
{
    NpgsqlConnection CreateConnection();
}