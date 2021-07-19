namespace VP.Core.DataAccess.Sql.Persistence
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    /// <summary>
    /// SqlPersistent class.
    /// Communicates with SQL Server and creates/deletes/updates data based on the stored procedure.
    /// </summary>
    class SqlPersistent : IPersistent
    {
        private readonly ILogger<SqlPersistent> _logger;
        private readonly SqlConnection _connection;

        public SqlPersistent(
            ILogger<SqlPersistent> logger,
            IDbConnection connection)
        {
            _logger = logger;
            _connection = (SqlConnection)connection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="orderOfExecutions"></param>
        /// <returns></returns>
        bool IPersistent.CommitMultipleCommandsInSingleTransaction(
            string connectionString,
            Queue<SqlCommandDetail> orderOfExecutions)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError($"Parameter: {nameof(connectionString)} cannot be null/empty. Current execution is terminated and returned to the caller.");
                return false;
            }
            if (null == orderOfExecutions || !orderOfExecutions.Any())
            {
                _logger.LogError($"Parameter: {nameof(orderOfExecutions)} cannot be null/empty. Current execution is terminated and returned to the caller.");
                return false;
            }

            _connection.ConnectionString = connectionString;
            _connection.Open();
            using var transaction = _connection.BeginTransaction();
            try
            {
                foreach (var detail in orderOfExecutions)
                {
                    var command = _connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandType = detail.CommandType;
                    command.CommandText = detail.CommandText;
                    detail.PerformParamBinding(command.Parameters);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)    //TODO: use specific exceptions.
            {
                _logger.LogError($"Method: PersistMultipleCommandsInSingleTransaction, execution failed.");
                _logger.LogError($"Transaction rollback in progress...");
                transaction.Rollback();
                _logger.LogError($"Transaction successfully rolled back.");
                _logger.LogError($"Error message: {ex.Message}");

                return false;
            }
            finally
            {
                _connection.Close();
            }

            return true;
        }
    }
}
