namespace VP.Core.DataAccess.Sql.Persistence.Command
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    interface IExecutioner
    {
        bool ExecuteInSingleTransaction(string connectionString, Queue<SqlCommandDetail> orderOfExecutions);
        Task<bool> ExecuteInSingleTransactionAsync(string connectionString, Queue<SqlCommandDetail> orderOfExecutions);
    }
}
