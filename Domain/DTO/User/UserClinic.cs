using app.Domain.DTO.Adress;

namespace app.Domain.DTO.User
{
    public class UserClinic
    {
        public string email { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string cnpj {  get; set; }
        public AdressDTO adress { get; set; }
    }
}
