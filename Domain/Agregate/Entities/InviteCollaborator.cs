using app.Domain.DTO.InviteCollaborator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace app.Domain.Agregate.Entities
{
    public class InviteCollaborator
    {
        private string _idInvite;
        private string _idUserSender;
        private string _emailrecipient;
        private string _status;
        private DateTime _createDateInvite;
        private TimeSpan _timeSinceInvitation;
        private TimeSpan _duration;
        public string emailrecipient { get { return _emailrecipient; } }
        public string idusersender { get { return _idUserSender; } }
        public string idinvite { get { return _idInvite; } }
        public string status { get { return _status; } }
        public DateTime datecreate { get { return _createDateInvite; } }
        private InviteCollaborator() { }
        public static InviteCollaborator create(InviteDTO invitedto)
        {
            InviteCollaborator invite = new InviteCollaborator();
            invite._idInvite = Guid.NewGuid().ToString("N");
            invite._emailrecipient = invitedto.EmailUserCollaborator;
            invite._idUserSender = invitedto.idUserSender;
            invite._status = "Pending";
            invite._createDateInvite = DateTime.Now;
            return invite;
        }
        public static InviteCollaborator restore(InviteDbDTO invitedb)
        {
            InviteCollaborator invite = new InviteCollaborator();
            invite._idInvite = invitedb.idinvitation;
            invite._idUserSender= invitedb.idusersender;
            invite._emailrecipient = invitedb.emailuserrecipient;
            invite._status= invitedb.status;
            invite._createDateInvite = invitedb.datecreate;
            return invite;
        }
        public void acceptInvite()
        {
            if(_status == "Cancelled"||_status == "Rejected"||_status == "Accepted")
            {
                throw new Exception("This invitation cannot be accepted");
            }
            _status = "Accepted";
            _duration = DateTime.Now - _createDateInvite;
            return;
        }
        public void rejectInvite()
        {
            if (_status == "Pending")
            {
                _status = "Rejected";
                _duration = DateTime.Now - _createDateInvite;
                return;
            }
            throw new Exception("This invitation cannot be rejected");
        }
        public void cancelInvite(string id)
        {
            if(id == _idUserSender && _status == "Pending")
            {
                _status = "Canceled";
                _duration = DateTime.Now - _createDateInvite;
                return;
            }
            throw new Exception("This invitation cannot be canceled");
        }
    }
}
