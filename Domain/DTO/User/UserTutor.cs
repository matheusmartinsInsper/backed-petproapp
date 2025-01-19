using app.Domain.DTO.Adress;
using app.Domain.DTO.Fone;

namespace app.Domain.DTO.User
{
    public class UserTutor
    {
        public string email { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public PhoneDTOInput? phone { get; set; }
        public AdressDTO? adress { get; set; }
    }
}
