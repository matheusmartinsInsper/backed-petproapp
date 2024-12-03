using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Application.UseCase
{
    public class GetInvitationsOfUser
    {
        private IRepositoryInvitation _repoinvite;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryUserCollaborator _repocollaborator;
        public GetInvitationsOfUser(IRepositoryInvitation repoinvitation,IRepositoryUserClinic repoclinic,IRepositoryUserCollaborator repocollaborator)
        {
            _repoinvite = repoinvitation;
            _repoclinic = repoclinic;
            _repocollaborator = repocollaborator;
        }
        public async Task<List<InvitationOutput>> execute(string iduser)
        {
            List<InvitationOutput> invitesoutput = new List<InvitationOutput>();
            User collaborator = await _repoclinic.getUserBase(iduser);
            List<InviteCollaborator> invitations = await _repoinvite.getAll(collaborator.email);
            if (invitations == null)
                return null;
            foreach(InviteCollaborator invitation in invitations)
            {
                InvitationOutput inv = new InvitationOutput();
                User clinic = await _repoclinic.get(invitation.idusersender);
                inv.nameusersender = clinic.name;
                inv.status = invitation.status;
                inv.datecreate = invitation.datecreate;
                inv.idinvitation = invitation.idinvite;
                invitesoutput.Add(inv);
            }
            return invitesoutput;
        }
    }
}
