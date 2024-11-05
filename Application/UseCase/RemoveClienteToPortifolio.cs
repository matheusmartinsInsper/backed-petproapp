using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DomainService;

namespace app.Application.UseCase
{
    public class RemoveClienteToPortifolio
    {
        private IRepositoryUserTutor _repotutor;
        private IRepositoryPortfolioClient _repoport;
        private IRepositoryUserClinic _repoclinic;
        public RemoveClienteToPortifolio(IRepositoryUserClinic repoclinic, IRepositoryUserTutor repotutor, IRepositoryPortfolioClient repoport)
        {
            _repotutor = repotutor;
            _repoport = repoport;
            _repoclinic = repoclinic;
        }
        public async Task execute(string emailclient, string idowner)
        {
            ClientPortfolio portfolio = await _repoport.get(idowner);
            User owner = await _repoclinic.getUserBase(idowner);
            User client = await _repotutor.getByEmail(emailclient);
            ClientOnboarding onboarding = new ClientOnboarding(owner, client, portfolio);
            await _repoport.save(onboarding.Remove());
        }
    }
}
