using VP.Core.DataAccess.Sql.Persistence.Command;

namespace VP.Core.DataAccess.Sql.Executioner
{
    public interface IDbContext
    {
        void Commit(string connectionStringKey);
        void Execute(SqlCommandDetail detail);
    }
}
