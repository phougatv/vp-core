namespace VP.Core.DataAccess.Sql.Executioner
{
    public interface IExecutioner
    {
        void Commit(string connectionStringKey);
        void Execute(SqlCommandDetail detail);
    }
}
