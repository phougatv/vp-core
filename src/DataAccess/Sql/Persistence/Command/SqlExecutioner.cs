namespace VP.Core.DataAccess.Sql.Persistence.Command
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// SqlPersistent class.
    /// Communicates with SQL Server and creates/deletes/updates data based on the stored procedure.
    /// </summary>
    class SqlExecutioner : IExecutioner
    {
        private readonly ILogger<SqlExecutioner> _logger;
        private readonly SqlConnection _connection;

        public SqlExecutioner(ILogger<SqlExecutioner> logger, IDbConnection connection)
        {
            _logger = logger;
            _connection = (SqlConnection)connection;
        }

        /// <summary> Commits all the commands in a single transaction. </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="orderOfExecutions">The order of execution of commands, <see cref="Queue{SqlCommandDetail}"/>.</param>
        /// <returns></returns>
        bool IExecutioner.ExecuteInSingleTransaction(string connectionString, Queue<SqlCommandDetail> orderOfExecutions)
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
                    using var command = _connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandType = detail.CommandType;
                    command.CommandText = detail.CommandText;
                    detail.PerformParamBinding(command.Parameters);
                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (Exception ex)    //TODO: use specific exceptions.
            {
                _logger.LogError($"Method: IExecutioner.ExecuteInSingleTransaction, execution failed.");
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

        async Task<bool> IExecutioner.ExecuteInSingleTransactionAsync(string connectionString, Queue<SqlCommandDetail> orderOfExecutions)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError($"Parameter: {nameof(connectionString)} cannot be null/empty. Current execution is terminated and returned to the caller.");
                return await Task.FromResult(false);
            }
            if (null == orderOfExecutions || !orderOfExecutions.Any())
            {
                _logger.LogError($"Parameter: {nameof(orderOfExecutions)} cannot be null/empty. Current execution is terminated and returned to the caller.");
                return await Task.FromResult(false);
            }

            _connection.ConnectionString = connectionString;
            _connection.Open();
            using var transaction = (SqlTransaction) await _connection.BeginTransactionAsync();
            try
            {
                foreach(var detail in orderOfExecutions)
                {
                    using var command = _connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandType = detail.CommandType;
                    command.CommandText = detail.CommandText;
                    detail.PerformParamBinding(command.Parameters);
                    await command.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: IExecutioner.ExecuteInSingleTransaction, execution failed.");
                _logger.LogError($"Transaction rollback in progress...");
                transaction.Rollback();
                _logger.LogError($"Transaction successfully rolled back.");
                _logger.LogError($"Error message: {ex.Message}");

                return await Task.FromResult(false);
            }
            finally
            {
                _connection.Close();
            }

            return await Task.FromResult(true);
        }
    }
}
