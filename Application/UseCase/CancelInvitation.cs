using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Application.UseCase
{
    public class CancelInvitation
    {
        private IRepositoryInvitation _repoinvite;
        private IRepositoryUserClinic _repoclinic;
        public CancelInvitation(IRepositoryUserClinic repoclinic, IRepositoryInvitation repoinvite)
        {
            _repoinvite = repoinvite;
            _repoclinic = repoclinic;
        }
        public async Task execute(string idinvitation, string iduser)
        {
            InviteCollaborator invite = await _repoinvite.get(idinvitation);
            if (invite.idusersender != null)
            {
                User user = await _repoclinic.get(iduser);
                invite.cancelInvite(invite.idusersender);
                await _repoinvite.update(invite);
                return;
            }
            throw new Exception("Esse convite nao existe");
        }
    }
}
