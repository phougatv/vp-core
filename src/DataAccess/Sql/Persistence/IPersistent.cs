namespace VP.Core.DataAccess.Sql.Persistence
{
    using System.Collections.Generic;

    interface IPersistent
    {
        internal bool CommitMultipleCommandsInSingleTransaction(
            string connectionString,
            Queue<SqlCommandDetail> orderOfExecutions);
    }
}
