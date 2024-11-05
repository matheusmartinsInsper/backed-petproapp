using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DomainService;

namespace app.Application.UseCase
{
    public class AcceptInvitation
    {
        private IRepositoryInvitation _repoinvite;
        private IRepositoryUserCollaborator _repouser;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryNetWork _reponet;
        public AcceptInvitation(IRepositoryInvitation repoinvite,IRepositoryUserCollaborator repouser,IRepositoryNetWork reponet, IRepositoryUserClinic repoclinic) 
        {
            _repoinvite = repoinvite;
            _repouser = repouser;
            _reponet = reponet;
            _repoclinic = repoclinic;
        }
        public async Task execute(string idinvitation, string iduser)
        {
            InviteCollaborator invite = await _repoinvite.get(idinvitation);
            if(invite.idusersender != null)
            {
                User user = await _repouser.get(iduser);
                User userclinic = await _repoclinic.get(invite.idusersender);
                NetWorkCollaborators net = await _reponet.getByUser(userclinic.id);
                CollaboratorOnboarding onboarding = new CollaboratorOnboarding(invite, net, user);
                onboarding.Integrate();
                await _repoinvite.update(onboarding.invite);
                await _reponet.update(onboarding.network);
                return;
            }
        }
    }
}
