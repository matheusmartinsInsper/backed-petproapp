using app.Domain.Agregate.Entities;
using app.Domain.DTO.User;

namespace app.Domain.DomainService
{
    public class CollaboratorOnboarding
    {
        private InviteCollaborator InviteCollaborator;
        private NetWorkCollaborators NetWorkCollaborators;
        private User UserCollaborator;
        public InviteCollaborator invite { get {  return InviteCollaborator; } }
        public NetWorkCollaborators network { get { return NetWorkCollaborators; } }
        public User user { get { return UserCollaborator; } }
        public CollaboratorOnboarding(InviteCollaborator inviteCollaborator, NetWorkCollaborators netWorkCollaborators, User usercollaborator)
        {
            InviteCollaborator = inviteCollaborator;
            NetWorkCollaborators = netWorkCollaborators;
            UserCollaborator = usercollaborator;
        }
        public CollaboratorOnboarding(NetWorkCollaborators netWorkCollaborators, User usercollaborator)
        {
            NetWorkCollaborators = netWorkCollaborators;
            UserCollaborator = usercollaborator;
        }
        public void Integrate()
        {
            if (UserCollaborator.id != null)
            {
                if(UserCollaborator.email != InviteCollaborator.emailrecipient)
                    throw new Exception("Esse convite nao pertence a esse usuario");
                InviteCollaborator.acceptInvite();
                NetWorkCollaborators.addCollaborator(UserCollaborator.id, UserCollaborator.categoryCode);
                return;
            }
            else if(UserCollaborator.categoryCode != "Collaborator")
            {
                throw new Exception("This user can't be added to the network");
            }
            throw new Exception("This user not exist");
        }
        public void Remove()
        {
            if(UserCollaborator.id != null)
            {
                NetWorkCollaborators.removeCollaborator(UserCollaborator.id);
                return;
            }
            throw new Exception("This user not exist");
        }
    }
}
