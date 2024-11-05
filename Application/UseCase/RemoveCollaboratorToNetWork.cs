using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DomainService;

namespace app.Application.UseCase
{
    public class RemoveCollaboratorToNetWork
    {
        private IRepositoryUserCollaborator _repouser;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryNetWork _reponet;
        public RemoveCollaboratorToNetWork(IRepositoryUserCollaborator repouser, IRepositoryNetWork reponet, IRepositoryUserClinic repoclinic)
        {
            _repouser = repouser;
            _reponet = reponet;
            _repoclinic = repoclinic;
        }
        public async Task execute(string iduser,string emailuser)
        {
            User user = await _repouser.getByEmail(emailuser);
            User userclinic = await _repoclinic.get(iduser);
            NetWorkCollaborators net = await _reponet.getByUser(iduser);
            CollaboratorOnboarding onboarding = new CollaboratorOnboarding(net, user);
            if (userclinic.categoryCode != "Clinic")
                throw new Exception("Usuario sem permissao para acessar esse recurso");
            onboarding.Remove();
            await _reponet.update(onboarding.network);
        }
    }
}
