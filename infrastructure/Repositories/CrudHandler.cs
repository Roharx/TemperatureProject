using Dapper;
using exceptions;
using infrastructure.Interfaces;
using Npgsql;

namespace infrastructure.Repositories
{
    public class CrudHandler : ICrudHandler
    {
        private readonly IDatabase _database;

        public CrudHandler(IDatabase database)
        {
            _database = database;
        }

        private T ExecuteDbOperation<T>(Func<IDatabase, T> operation)
        {
            try
            {
                return operation(_database);
            }
            catch (NpgsqlException ex)
            {
                throw new Exceptions.DatabaseConnectionException("Failed to execute a database operation", ex);
            }
            catch (Exception ex)
            {
                throw new Exceptions.QueryExecutionException("An error occurred during query execution", ex);
            }
        }

        private static string BuildWhereClause(Dictionary<string, object> parameters)
        {
            return string.Join(" AND ", parameters.Keys.Select(key => $"{key} = @{key}"));
        }

        public IEnumerable<T> GetAllItems<T>(string tableName)
        {
            var sql = $"SELECT * FROM {tableName}";
            return ExecuteDbOperation(db => db.Query<T>(sql));
        }

        public IEnumerable<T> GetItemsByParameters<T>(string tableName, Dictionary<string, object> parameters)
        {
            var whereClause = BuildWhereClause(parameters);
            var sql = $"SELECT * FROM {tableName} WHERE {whereClause}";
            return ExecuteDbOperation(db => db.Query<T>(sql, parameters));
        }

        public T? GetSingleItemByParameters<T>(string tableName, Dictionary<string, object> parameters)
        {
            var whereClause = BuildWhereClause(parameters);
            var sql = $"SELECT * FROM {tableName} WHERE {whereClause}";
            return ExecuteDbOperation(db => db.QueryFirstOrDefault<T>(sql, parameters));
        }

        public IEnumerable<T> GetSelectedParametersForItems<T>(string tableName, string columns,
            Dictionary<string, object> parameters)
        {
            var whereClause = BuildWhereClause(parameters);
            var sql = $"SELECT {columns} FROM {tableName} WHERE {whereClause}";
            return ExecuteDbOperation(db => db.Query<T>(sql, parameters));
        }

        public int CreateItem(string tableName, Dictionary<string, object> parameters)
        {
            var columns = string.Join(", ", parameters.Keys);
            var values = string.Join(", ", parameters.Keys.Select(key => $"@{key}"));
            var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values}) RETURNING id";

            return ExecuteDbOperation(db => db.ExecuteScalar<int>(sql, parameters));
        }

        public bool CreateItemWithoutReturn(string tableName, Dictionary<string, object> parameters)
        {
            var columns = string.Join(", ", parameters.Keys);
            var values = string.Join(", ", parameters.Keys.Select(key => $"@{key}"));
            var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

            ExecuteDbOperation(db => db.Execute(sql, parameters));
            return true;
        }

        public bool UpdateItem(string tableName, Dictionary<string, object> conditionColumns,
            Dictionary<string, object> modifications)
        {
            var conditionClauses = string.Join(" AND ", conditionColumns.Select(cond => $"{cond.Key} = @{cond.Key}"));
            var updateSet = string.Join(", ", modifications.Select(mod => $"{mod.Key} = @{mod.Key}"));

            var sql = $"UPDATE {tableName} SET {updateSet} WHERE {conditionClauses}";

            var parameters = conditionColumns.Union(modifications).ToDictionary(pair => pair.Key, pair => pair.Value);
            ExecuteDbOperation(db => db.Execute(sql, parameters));
            return true;
        }

        public bool DeleteItem(string tableName, int itemId)
        {
            var sql = $"DELETE FROM {tableName} WHERE id=@id";
            ExecuteDbOperation(db => db.Execute(sql, new { id = itemId }));
            return true;
        }

        public bool DeleteItemWithMultipleParams(string tableName, Dictionary<string, object> conditionColumns)
        {
            var conditionClauses = string.Join(" AND ", conditionColumns.Select(cond => $"{cond.Key} = @{cond.Key}"));
            var sql = $"DELETE FROM {tableName} WHERE {conditionClauses}";

            ExecuteDbOperation(db => db.Execute(sql, conditionColumns));
            return true;
        }
    }
}