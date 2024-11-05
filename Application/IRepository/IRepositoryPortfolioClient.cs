using app.Domain.Agregate.Entities;

namespace app.Application.IRepository
{
    public interface IRepositoryPortfolioClient
    {
        public Task save(ClientPortfolio portfolio);
        public Task<ClientPortfolio> get(string idowner);
        public Task update(ClientPortfolio portfolio);
    }
}
