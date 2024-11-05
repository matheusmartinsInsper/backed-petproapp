namespace app.Application.IRepository
{
    public interface IFactoryDbContext
    {
        public IDbContext psqlContext();
    }
}
