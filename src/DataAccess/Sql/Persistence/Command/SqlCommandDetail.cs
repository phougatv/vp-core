namespace VP.Core.DataAccess.Sql.Persistence.Command
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using VP.Core.DataAccess.Sql.Enums;

    public class SqlCommandDetail
    {
        #region Public Getters-Setters
        /// <summary>
        /// The Command - Create, Delete, and Update.
        /// </summary>
        public Command Command { get; set; }

        /// <summary>
        /// The command text that modifies the DB state.
        /// 
        /// NOTE: Do not pass queries/stored-procedures that read data.
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
