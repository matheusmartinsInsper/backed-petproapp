using app.Application.IRepository;

namespace app.Infra.Repository.FactoryContext
{
    public class FactoryDbContext : IFactoryDbContext
    {
        public IDbContext psqlContext()
        {
            return new PGSQLContext();
        }
    }
}
