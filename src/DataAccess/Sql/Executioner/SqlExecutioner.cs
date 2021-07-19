namespace VP.Core.DataAccess.Sql.Executioner
{
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using VP.Core.DataAccess.Sql.Accessors;
    using VP.Core.DataAccess.Sql.Persistence;

    /// <summary>
    /// SqlExecutioner class.
    /// </summary>
    class SqlExecutioner : IExecutioner
    {
        #region Private Readonly Fields
        private readonly ILogger<SqlExecutioner> _logger;
        private readonly IConnectionStringAccessor _connectionStringAccessor;
        private readonly IPersistent _peristent;
        private readonly Queue<SqlCommandDetail> _orderOfExecutions;
        #endregion Private Readonly Fields

        #region Public Ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger">The logger, <see cref="ILogger{SqlExecutioner}"/>.</param>
        /// <param name="connectionStringAccessor">The connectionStringAccessor, <see cref="IConnectionStringAccessor"/>.</param>
        /// <param name="sqlPeristent">The sqlPersistent, <see cref="IPersistent"/></param>
        /// <param name="orderOfExecutions">The orderOfExecution, <see cref="Queue{SqlCommandDetail}"/>.</param>
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
        #endregion Public Ctor

        #region Public Methods
        /// <summary> Commits the commands. </summary>
        /// <param name="connectionStringKey">The connection string key.</param>
        public void Commit(string connectionStringKey)
        {
            if (string.IsNullOrEmpty(connectionStringKey))
            {
                _logger.LogError($"Parameter: {nameof(connectionStringKey)} is null/empty. Current execution is terminated and returned to the caller.");
                return;
            }

            InternalCommit(connectionStringKey);
        }

        /// <summary> Enqueues the sql command detail object. </summary>
        /// <param name="sqlCommandDetail">The sql command detail, <see cref="SqlCommandDetail"/>.</param>
        public void Execute(SqlCommandDetail sqlCommandDetail)
        {
            if (null == sqlCommandDetail)
            {
                _logger.LogError($"Parameter: {nameof(sqlCommandDetail)} is null. Current execution is terminated and returned to the caller.");
                return;
            }

            InternalExecute(sqlCommandDetail);
        }
        #endregion Public Methods

        #region Private Methods
        /// <summary> Gets the connection-string based on the connection-string-key and performs the commit. </summary>
        /// <param name="connectionStringKey">The connection string key.</param>
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

        /// <summary> Enqueues the sql command detail object. </summary>
        /// <param name="sqlCommandDetail">The sql command detail, <see cref="SqlCommandDetail"/>.</param>
        private void InternalExecute(SqlCommandDetail sqlCommandDetail) => _orderOfExecutions.Enqueue(sqlCommandDetail);
        #endregion Private Methods
    }
}
