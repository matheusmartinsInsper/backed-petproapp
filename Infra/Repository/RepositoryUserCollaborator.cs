using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.User;
using System.Text.Json.Nodes;
using System.Text.Json;
using app.Domain.Agregate.ObjectValues;
using app.Domain.DTO.Fone;

namespace app.Infra.Repository
{
    public class RepositoryUserCollaborator : IRepositoryUserCollaborator
    {

        private IFactoryDbContext _factoryDbContext;
        private IDbContext _context;
        private string _connectString = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=petprodb";
        public RepositoryUserCollaborator(IFactoryDbContext factorycontext)
        {
            _factoryDbContext = factorycontext;
            _context = _factoryDbContext.psqlContext();
        }
        public async Task<User> get(string id)
        {
            await _context.connect(_connectString);
            string commandBase = "SELECT name,email,password,\"user\".iduser,category,crmv,cpf,graduation,institution from \"user\" inner join collaborator on \"user\".iduser = collaborator.iduser " +
                "where \"user\".iduser = @iduser";
            string commandGetNumber = "SELECT idfone,iduser,countrycode,areacode,phone,createat FROM fone where iduser = @iduser";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@iduser",id }
            };
            List<JsonObject> result = await _context.read(commandBase, parameters);
            List<UserCollaboratorDb> dtos = result.Select(jsonObject => JsonSerializer.Deserialize<UserCollaboratorDb>(jsonObject.ToJsonString()))
                                           .ToList();
            List<JsonObject> fones = await _context.read(commandGetNumber, parameters);
            List<PhoneDTO> fonesdto = fones.Select(jsonObject => JsonSerializer.Deserialize<PhoneDTO>(jsonObject.ToJsonString()))
                                         .ToList();
            if (dtos.Count == 0)
            {
                _context.close();
                return null;
            }
            PhoneDTO fone = fonesdto.Count() == 0 ? null : fonesdto[0];
            User user = User.restore(dtos[0],fone);
            _context.close();
            return user;
        }

        public async Task<User> getByEmail(string email)
        {
            await _context.connect(_connectString);
            string commandBase = "SELECT name,email,password,\"user\".iduser,category,crmv,cpf,graduation,institution from \"user\" inner join collaborator on \"user\".iduser = collaborator.iduser " +
                "where \"user\".email = @email";
            string commandGetNumber = "SELECT idfone,iduser,countrycode,areacode,phone,createat FROM fone where iduser = @iduser";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@email",email }
            };
            List<JsonObject> result = await _context.read(commandBase, parameters);
            List<UserCollaboratorDb> dtos = result.Select(jsonObject => JsonSerializer.Deserialize<UserCollaboratorDb>(jsonObject.ToJsonString()))
                                           .ToList();
         
            if (dtos.Count == 0)
                throw new Exception("user not found");
            string iduser = dtos[0].iduser;
            parameters["@iduser"] = iduser;
            List<JsonObject> fones = await _context.read(commandGetNumber, parameters);
            List<PhoneDTO> fonesdto = fones.Select(jsonObject => JsonSerializer.Deserialize<PhoneDTO>(jsonObject.ToJsonString()))
                                         .ToList();
            PhoneDTO fone = fonesdto.Count() == 0 ? null : fonesdto[0];
            User user = User.restore(dtos[0],fone);
            _context.close();
            return user;
        }

        public async Task<List<User>> getcollaborators(string iduser)
        {
            List<User> users = new List<User>();
            await _context.connect(_connectString);
            string command = "select t.name,t.email,t.password,t.iduser,t.category,t.crmv,t.cpf,t.graduation,t.institution,t.dateborn from network " +
                             "inner join (select \"user\".iduser,crmv, cpf,institution,graduation,password,email,name,category,collaborator.dateborn from \"user\" inner join collaborator on \"user\".iduser = collaborator.iduser) " +
                             "as t on t.iduser = network.idcollaborator where network.iduserprimary = @iduser";
            string commandGetNumber = "SELECT idfone,iduser,countrycode,areacode,phone,createat FROM fone where iduser = @iduser";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@iduser",iduser }
            };
            List<JsonObject> result = await _context.read(command, parameters);
            List<UserCollaboratorDb> dtos = result.Select(jsonObject => JsonSerializer.Deserialize<UserCollaboratorDb>(jsonObject.ToJsonString()))
                                           .ToList();
            foreach(UserCollaboratorDb userdb in dtos)
            {
                string id = userdb.iduser;
                parameters["@iduser"] = id;
                List<JsonObject> fones = await _context.read(commandGetNumber, parameters);
                List<PhoneDTO> fonesdto = fones.Select(jsonObject => JsonSerializer.Deserialize<PhoneDTO>(jsonObject.ToJsonString()))
                                             .ToList();
                PhoneDTO fone = fonesdto.Count() == 0 ? null : fonesdto[0];
                User user = User.restore(userdb,fone);
                users.Add(user);
            }
            _context.close();
            return users;
        }

        public async Task save(User user)
        {
            await _context.connect(_connectString);
            string commandBase = "INSERT INTO \"user\" (name,email,password,iduser,category) VALUES (@name,@email,@password,@iduser,@category)";
            string commandSaveCollaborator = "INSERT INTO \"collaborator\" (iduser,crmv,cpf,graduation,institution,dateborn) " +
                "VALUES (@iduser,@crmv,@cpf,@graduation,@institution,@dateborn)";
            string commandAdress = "INSERT INTO \"adress\" (iduser,idadress,uf,city,street,number,cep) VALUES (@iduser,@idadress,@uf,@city,@street,@number,@cep)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@name",user.name },
                {"@email",user.email },
                {"@password",user.password },
                {"@iduser",user.id },
                {"@category",user.categoryCode },
            };
            Dictionary<string, object> parametersAdress = new Dictionary<string, object>()
            {
                {"@iduser",user.id },
                {"@idadress",user._idadress },
                {"@uf",user.uf },
                {"@city",user.city },
                {"@street",user.street },
                {"@number",user.number },
                {"@cep",user.cep },
            };
            Dictionary<string, object> parametersCollaborator = new Dictionary<string, object>()
            {
                {"@iduser",user.id },
                {"@crmv",user.crmv },
                {"@cpf",user.cpf },
                {"@graduation",user.graduation },
                {"@institution",user.institution },
                {"@dateborn",user.dateborn }
            };
            await _context.command(commandBase, parameters);
            await _context.command(commandSaveCollaborator, parametersCollaborator);
            await _context.command(commandAdress, parametersAdress);
            _context.close();
        }

        public async Task update(User user)
        {
            await _context.connect(_connectString);
            string commandinsertPhone = "insert into fone (iduser,idfone,phone,countrycode,areacode,createat) values (@iduser,@idfone,@phone,@countrycode,@areacode,@createat)";
            string commanddelete = "delete from fone where iduser = @iduser";
            Dictionary<string, object> paramns = new Dictionary<string, object>()
            {
                { "@iduser" ,user.id},
                { "@idfone" ,user.Fone.idfone},
                { "@phone" ,user.Fone.phone},
                { "@countrycode" ,user.Fone.countrycode},
                { "@areacode" ,user.Fone.areacode},
                { "@createat" ,user.Fone.createat}
            };
            await _context.command(commanddelete, paramns);
            await _context.command(commandinsertPhone, paramns);
            _context.close();
            return;
        }
    }
}
