using app.Domain.DTO.Adress;

namespace app.Domain.DTO.User
{
    public class UserCollaborator
    {
        public string email { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string cpf { get; set; }
        public string crmv { get; set; }
        public string? graduation { get; set; }
        public string? institution { get; set; }
        public AdressDTO adress { get; set; }
        public DateTime dateborn { get; set; }
    }
}
