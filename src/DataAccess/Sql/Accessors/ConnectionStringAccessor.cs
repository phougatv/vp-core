namespace VP.Core.DataAccess.Sql.Accessors
{
    using Microsoft.Extensions.Logging;

    /// <summary> ConnectionStringAccessor class. </summary>
    class ConnectionStringAccessor : IConnectionStringAccessor
    {
        #region Private Readonly Fields
        private readonly ILogger<ConnectionStringAccessor> _logger;
        private readonly SqlConfiguration _sqlConfiguration;
        #endregion Private Readonly Fields

        #region Public Ctor
        /// <summary> Public Ctor. </summary>
        /// <param name="logger">The logger, <see cref="ILogger{ConnectionStringAccessor}"/></param>
        /// <param name="sqlConfiguration">The sqlConfiguration, <see cref="SqlConfiguration"/></param>
        public ConnectionStringAccessor(
            ILogger<ConnectionStringAccessor> logger,
            SqlConfiguration sqlConfiguration)
        {
            _logger = logger;
            _sqlConfiguration = sqlConfiguration;
        }
        #endregion Public Ctor

        /// <summary>
        /// Gets the connection-string based on the key.
        /// CAUTION: If key is not found, null is returned.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetConnectionString(string key)
        {
            if (_sqlConfiguration.ConnectionStrings.TryGetValue(key, out var connectionString))
                return connectionString;

            _logger.LogError($"Parameter: {nameof(key)} does not exists.");
            return null;
        }
    }
}
