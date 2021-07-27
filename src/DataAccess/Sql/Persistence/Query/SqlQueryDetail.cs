namespace VP.Core.DataAccess.Sql.Persistence.Query
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using VP.Core.DataAccess.Sql.Enums;

    class SqlQueryDetail
    {
        #region Public Getters-Setters
        /// <summary>
        /// The Query - Read.
        /// </summary>
        public Query Query { get; set; }

        /// <summary>
        /// The command text that queries the DB state.
        /// 
        /// NOTE: Queries/stored-procedures that only read data.
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        /// The command type. <see cref="System.Data.CommandType"/>
        /// </summary>
        public CommandType CommandType { get; set; }

        /// <summary>
        /// The sql parameter collection delegate.
        /// </summary>
        public Action<SqlParameterCollection> PerformParamBinding { get; set; }
        #endregion Public Getters-Setters
    }
}
