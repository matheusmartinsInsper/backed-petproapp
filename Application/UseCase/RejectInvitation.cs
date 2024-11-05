using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DomainService;

namespace app.Application.UseCase
{
    public class RejectInvitation
    {
        private IRepositoryInvitation _repoinvite;
        private IRepositoryUserCollaborator _repouser;
        public RejectInvitation(IRepositoryInvitation repoinvite, IRepositoryUserCollaborator repouser)
        {
            _repoinvite = repoinvite;
            _repouser = repouser;
        }
        public async Task execute(string idinvitation, string iduser)
        {
            InviteCollaborator invite = await _repoinvite.get(idinvitation);
            if (invite.idusersender != null)
            {
                User user = await _repouser.get(iduser);
                if (user.email != invite.emailrecipient)
                    throw new Exception("Usuario sem permissao para rejeitar convite");
                invite.rejectInvite();
                await _repoinvite.update(invite);
                return;
            }
        }
    }
}
