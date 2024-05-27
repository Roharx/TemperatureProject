using System.Collections.Generic;
using System.Data;
using Dapper;
using Npgsql;
using infrastructure.Interfaces;

namespace infrastructure.Repositories
{
    public class DapperDatabase : IDatabase
    {
        private readonly INpgsqlConnectionFactory _connectionFactory;

        public DapperDatabase(INpgsqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        private NpgsqlConnection GetOpenConnection()
        {
            return _connectionFactory.CreateConnection();
        }

        public IEnumerable<T> Query<T>(string sql, object? param = null, IDbTransaction? transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using var connection = GetOpenConnection();
            return connection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public T QueryFirstOrDefault<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using var connection = GetOpenConnection();
            return connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public int Execute(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using var connection = GetOpenConnection();
            return connection.Execute(sql, param, transaction, commandTimeout, commandType);
        }

        public T ExecuteScalar<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using var connection = GetOpenConnection();
            return connection.ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType);
        }
    }
}