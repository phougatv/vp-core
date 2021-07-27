namespace VP.Core.DataAccess.Sql.Executioner
{
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using VP.Core.DataAccess.Sql.Accessors;
    using VP.Core.DataAccess.Sql.Persistence.Command;

    /// <summary>
    /// SqlExecutioner class.
    /// </summary>
    class SqlDbContext : IDbContext
    {
        #region Private Readonly Fields
        private readonly ILogger<SqlDbContext> _logger;
        private readonly IConnectionStringAccessor _connectionStringAccessor;
        private readonly IExecutioner _executioner;
        private readonly Queue<SqlCommandDetail> _orderOfExecutions;
        #endregion Private Readonly Fields

        #region Public Ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger">The logger, <see cref="ILogger{SqlExecutioner}"/>.</param>
        /// <param name="connectionStringAccessor">The connectionStringAccessor, <see cref="IConnectionStringAccessor"/>.</param>
        /// <param name="sqlPeristent">The sqlPersistent, <see cref="IExecutioner"/></param>
        /// <param name="orderOfExecutions">The orderOfExecution, <see cref="Queue{SqlCommandDetail}"/>.</param>
        public SqlDbContext(
            ILogger<SqlDbContext> logger,
            IConnectionStringAccessor connectionStringAccessor,
            IExecutioner sqlPeristent,
            Queue<SqlCommandDetail> orderOfExecutions)
        {
            _logger = logger;
            _connectionStringAccessor = connectionStringAccessor;
            _executioner = sqlPeristent;
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

            var connectionString = _connectionStringAccessor.GetConnectionString(connectionStringKey);
            if (string.IsNullOrEmpty(connectionStringKey))
            {
                _logger.LogError($"{nameof(connectionStringKey)} returned null/empty. Current execution is terminated and returned to the caller.");
                return;
            }

            _executioner.ExecuteInSingleTransaction(connectionString, _orderOfExecutions);
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

            _orderOfExecutions.Enqueue(sqlCommandDetail);
        }
        #endregion Public Methods
    }
}
