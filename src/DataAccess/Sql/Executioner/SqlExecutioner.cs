namespace VP.Core.DataAccess.Sql.Executioner
{
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using VP.Core.DataAccess.Sql.Accessors;
    using VP.Core.DataAccess.Sql.Persistence;

    class SqlExecutioner : IExecutioner
    {
        private readonly ILogger<SqlExecutioner> _logger;
        private readonly IConnectionStringAccessor _connectionStringAccessor;
        private readonly IPersistent _peristent;
        private readonly Queue<SqlCommandDetail> _orderOfExecutions;

        public SqlExecutioner(
            ILogger<SqlExecutioner> logger,
            IConnectionStringAccessor connectionStringAccessor,
            IPersistent sqlPeristent,
            Queue<SqlCommandDetail> orderOfExecutions)
        {
            _logger = logger;
            _connectionStringAccessor = connectionStringAccessor;
            _peristent = sqlPeristent;
            _orderOfExecutions = orderOfExecutions;
        }

        public void Commit(string connectionStringKey)
        {
            if (string.IsNullOrEmpty(connectionStringKey))
            {
                _logger.LogError($"Parameter: {nameof(connectionStringKey)} is null/empty. Current execution is terminated and returned to the caller.");
                return;
            }

            InternalCommit(connectionStringKey);
        }
        public void Execute(SqlCommandDetail sqlCommandDetail)
        {
            if (null == sqlCommandDetail)
            {
                _logger.LogError($"Parameter: {nameof(sqlCommandDetail)} is null. Current execution is terminated and returned to the caller.");
                return;
            }

            InternalExecute(sqlCommandDetail);
        }

        private void InternalCommit(string connectionStringKey)
        {
            var connectionString = _connectionStringAccessor.GetConnectionString(connectionStringKey);
            if (string.IsNullOrEmpty(connectionStringKey))
            {
                _logger.LogError($"{nameof(connectionString)} is null/empty. Current execution is terminated and returned to the caller.");
                return;
            }

            _peristent.CommitMultipleCommandsInSingleTransaction(connectionString, _orderOfExecutions);
        }
        private void InternalExecute(SqlCommandDetail sqlCommandDetail) => _orderOfExecutions.Enqueue(sqlCommandDetail);
    }
}
