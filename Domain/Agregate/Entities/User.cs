using app.Domain.Agregate.ObjectValues;
using app.Domain.DTO.User;

namespace app.Domain.Agregate.Entities
{
    public class User
    {
        private Email Email { get; set; }
        public string email;
        public string name;
        public string password;
        public string cpf;
        public string cnpj;
        public string? cep;
        public string? street;
        public string? city;
        public int? number;
        public string? uf;
        public string? _idadress;
        public string crmv;
        public string graduation;
        public string institution;
        private DateTime _dateborn;
        private string _plan;
        private string _id;
        private string _categoryCode;
        public string id { get { return _id; } }
        public string categoryCode { get { return _categoryCode; } }
        public string plan { get { return _plan; } }    
        public DateTime dateborn { get { return _dateborn; } }
        private User() { }
        public static User create(UserTutor user)
        {
            User _user = new User();
            _user.Email = new Email(user.email);
            _user.email = _user.Email.Value;
            _user.name = user.name == null ? "erro" : user.name;
            _user.password = user.password;
            _user._id = Guid.NewGuid().ToString("N");
            _user._dateborn = DateTime.Today;
            _user._categoryCode = "Tutor";
            return _user;
        }
        public static User create(UserClinic user)
        {
            User _user = new User();
            _user.Email = new Email(user.email);
            _user.email = _user.Email.Value;
            _user.name = user.name == null ? "erro" : user.name;
            _user.password = user.password;
            _user.cnpj = user.cnpj;
            _user._categoryCode = "Clinic";
            _user._plan = "Basic";
            _user._id = Guid.NewGuid().ToString("N");
            _user._idadress = Guid.NewGuid().ToString("N");
            _user.uf = user.adress.uf;
            _user.city = user.adress.city;
            _user.street = user.adress.street;
            _user.number = user.adress.number;
            _user.cep = user.adress.cep;
            return _user;
        }
        public static User create(UserCollaborator user)
        {
            User _user = new User();
            _user.Email = new Email(user.email);
            _user.email = _user.Email.Value;
            _user.name = user.name == null ? "erro" : user.name;
            _user.password = user.password;
            _user.crmv = user.crmv;
            _user.graduation = user.graduation;
            _user.institution = user.institution;
            _user.cpf = user.cpf;
            _user._id = Guid.NewGuid().ToString("N");
            _user._idadress = Guid.NewGuid().ToString("N");
            _user.uf = user.adress.uf;
            _user.city = user.adress.city;
            _user.street = user.adress.street;
            _user.number = user.adress.number;
            _user.cep = user.adress.cep;
            _user._dateborn = user.dateborn;
            _user._categoryCode = "Collaborator";
            return _user;
        }
        public static User restore(UserTutorDb user)
        {
            User _user = new User();
            _user.email = user.email;
            _user.name = user.name;
            _user.password = user.password;
            _user._id = user.iduser;
            _user._categoryCode = user.category;
            _user._dateborn = user.dateborn;
            return _user;
        }
        public static User restore(UserCollaboratorDb user)
        {
            User _user = new User();
            _user.email = user.email;
            _user.name = user.name;
            _user.password = user.password;
            _user._id = user.iduser;
            _user._categoryCode = user.category;
            _user.crmv = user.crmv;
            _user.cpf= user.cpf;
            _user.graduation = user.graduation;
            _user.institution = user.institution;
            _user._dateborn = user.dateborn;
            return _user;
        }
        public static User restore(UserBaseDTO user)
        {
            User _user = new User();
            _user.email = user.email;
            _user.name = user.name;
            _user.password = user.password;
            _user._id = user.iduser;
            _user._categoryCode = user.category;
            return _user;
        }
        public void setPassWord(string pass)
        {
            if (pass == null || pass == password)
            {
                throw new Exception("invalid password");
            }
            password = pass;
        }
    }
}
