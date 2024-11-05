namespace app.Domain.DTO.InviteCollaborator
{
    public class InviteDbDTO
    {
        public string idinvitation {  get; set; }
        public string idusersender {  get; set; }
        public string emailuserrecipient { get; set; }
        public string status { get; set; }
        public DateTime datecreate { get; set; }
    }
}
