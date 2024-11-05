using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.InviteCollaborator;

namespace app.Application.UseCase
{
    public class SendInviteToCollaborator
    {
        private IRepositoryInvitation _repoinvitation;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryUserCollaborator _repocollaborator;
        public SendInviteToCollaborator(IRepositoryInvitation repoinvitation,IRepositoryUserClinic repoclinic, IRepositoryUserCollaborator repocollaborator)
        {
            _repoclinic = repoclinic;
            _repoinvitation = repoinvitation;
            _repocollaborator = repocollaborator;
        }
        public async Task execute(InviteDTO invitedto)
        {
            User userclinic = await _repoclinic.get(invitedto.idUserSender);

            if (userclinic.categoryCode != "Clinic")
                throw new Exception("Usuario sem permissao para enviar convite a rede");

            User usercollaborator = await _repocollaborator.getByEmail(invitedto.EmailUserCollaborator);
            if (usercollaborator.categoryCode != "Collaborator" || usercollaborator.id == null)
                throw new Exception("Não é permitido enviar convite para esse tipo de usuario");

            InviteCollaborator invite = InviteCollaborator.create(invitedto);
            await _repoinvitation.save(invite);
            return;
        }
    }
}
