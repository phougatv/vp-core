namespace VP.Core.DataAccess.Sql.Persistence.Query
{
    using System.Data;

    internal interface IReader
    {
        IDataReader GetDataReader(string connectionString, SqlQueryDetail sqlQueryDetail);
    }
}
