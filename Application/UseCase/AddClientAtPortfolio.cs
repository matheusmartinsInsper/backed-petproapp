using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DomainService;

namespace app.Application.UseCase
{
    public class AddClientAtPortfolio
    {
        private IRepositoryUserTutor _repotutor;
        private IRepositoryPortfolioClient _repoport;
        private IRepositoryUserCollaborator _repocollaborator;
        private IRepositoryUserClinic _repoclinic;
        public AddClientAtPortfolio(IRepositoryUserClinic repoclinic, IRepositoryUserTutor repotutor,IRepositoryPortfolioClient repoport,IRepositoryUserCollaborator repocollaborator) 
        { 
            _repotutor = repotutor;
            _repoport = repoport;
            _repocollaborator = repocollaborator;
            _repoclinic = repoclinic;
        }
        public async Task execute(string emailclient,string idowner)
        {
            ClientPortfolio portfolio = await _repoport.get(idowner);
            User owner = await _repoclinic.getUserBase(idowner);
            User client = await _repotutor.getByEmail(emailclient);
            ClientOnboarding onboarding = new ClientOnboarding(owner, client,portfolio);
            await _repoport.save(onboarding.Integrate());
        }
    }
}
