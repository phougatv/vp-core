namespace VP.Core.DataAccess.Sql.Persistence.Query
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    class SqlReader : IReader
    {
        private readonly ILogger<SqlReader> _logger;
        private readonly SqlConnection _connection;

        public SqlReader(
            ILogger<SqlReader> logger,
            IDbConnection connection)
        {
            _logger = logger;
            _connection = (SqlConnection)connection;
        }

        /// <summary> Read data from DB based on the values provided in sqlQueryDetail instance. </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlQueryDetail">The sql query detail, <see cref="SqlQueryDetail"/></param>
        /// <returns></returns>
        IDataReader IReader.GetDataReader(string connectionString, SqlQueryDetail sqlQueryDetail)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError($"Parameter: {nameof(connectionString)} cannot be null/empty. Current execution is terminated and returned to the caller.");
                return null;
            }
            if (null == sqlQueryDetail)
            {
                _logger.LogError($"Parameter: {nameof(sqlQueryDetail)} cannot be null/empty. Current execution is terminated and returned to the caller.");
                return null;
            }

            _connection.ConnectionString = connectionString;
            _connection.Open();

            SqlDataReader sqlDataReader = null;
            using var transaction = _connection.BeginTransaction();
            try
            {
                using var command = _connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandType = sqlQueryDetail.CommandType;
                command.CommandText = sqlQueryDetail.CommandText;
                sqlQueryDetail.PerformParamBinding(command.Parameters);
                sqlDataReader = command.ExecuteReader();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: IReader.GetDataReader, execution failed.");
                _logger.LogError($"Transaction rollback in progress...");
                transaction.Rollback();
                _logger.LogError($"Transaction successfully rolled back.");
                _logger.LogError($"Error message: {ex.Message}");

                return null;
            }
            finally
            {
                _connection.Close();
            }

            return sqlDataReader;
        }
    }
}
