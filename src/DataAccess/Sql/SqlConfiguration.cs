namespace VP.Core.DataAccess.Sql
{
    using System.Collections.Generic;

    class SqlConfiguration
    {
        #region Internal Getters-Setters
        /// <summary>
        /// Connection-String map.
        /// </summary>
        internal IDictionary<string, string> ConnectionStrings { get; set; }
        #endregion Internal Getters-Setters

        #region Public Ctor
        /// <summary>
        /// Public Ctor with no parameters.
        /// </summary>
        public SqlConfiguration()
        {

        }
        #endregion Public Ctor
    }
}
