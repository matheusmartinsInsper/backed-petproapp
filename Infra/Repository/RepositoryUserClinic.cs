using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.User;
using System.Text.Json.Nodes;
using System.Text.Json;
using app.Domain.Agregate.ObjectValues;
using app.Domain.DTO.Fone;

namespace app.Infra.Repository
{
    public class RepositoryUserClinic : IRepositoryUserClinic
    {
        private IFactoryDbContext _factoryDbContext;
        private IDbContext _context;
        private string _connectString = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=petprodb";
        public RepositoryUserClinic(IFactoryDbContext factorycontext)
        {
            _factoryDbContext = factorycontext;
            _context = _factoryDbContext.psqlContext();
        }
        public async Task<User> get(string id)
        {
            await _context.connect(_connectString);
            string commandBase = "SELECT name,email,password,\"user\".iduser,category,plan,cnpj from \"user\" left join clinic on \"user\".iduser = clinic.iduser " +
                "where \"user\".iduser = @iduser";
            string commandGetNumber = "SELECT idfone,iduser,countrycode,areacode,phone,createat FROM fone where iduser = @iduser";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@iduser",id }
            };
            List<JsonObject> result = await _context.read(commandBase, parameters);
            List<JsonObject> fones = await _context.read(commandGetNumber, parameters);
            List<PhoneDTO> fonesdto = fones.Select(jsonObject => JsonSerializer.Deserialize<PhoneDTO>(jsonObject.ToJsonString()))
                                         .ToList();
            List<UserTutorDb> dtos = result.Select(jsonObject => JsonSerializer.Deserialize<UserTutorDb>(jsonObject.ToJsonString()))
                                           .ToList();
           
            PhoneDTO fone = fonesdto.Count()==0?null: fonesdto[0];
            if (dtos.Count == 0)
            {
                _context.close();
                return null;
            }
            _context.close();
            User user = User.restore(dtos[0],fone);
            return user;
        }

        public async Task<User> getByEmail(string email)
        {
            await _context.connect(_connectString);
            string commandBase = "SELECT name,email,password,\"user\".iduser,category,plan,cnpj from \"user\" inner join clinic on \"user\".iduser = clinic.iduser " +
                "where \"user\".email = @email";
            string commandGetNumber = "SELECT idfone,iduser,countrycode,areacode,phone,createat FROM fone where iduser = @iduser";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@email",email }
            };
            List<JsonObject> result = await _context.read(commandBase, parameters);
            List<UserTutorDb> dtos = result.Select(jsonObject => JsonSerializer.Deserialize<UserTutorDb>(jsonObject.ToJsonString()))
                                           .ToList();
            if (dtos.Count == 0)
                throw new Exception("user not found");
            string iduser = dtos[0].iduser;
            Dictionary<string, object> parametersiduser = new Dictionary<string, object>()
            {
                {"@iduser",iduser }
            };
            List<JsonObject> fones = await _context.read(commandGetNumber, parametersiduser);
            List<PhoneDTO> fonesdto = fones.Select(jsonObject => JsonSerializer.Deserialize<PhoneDTO>(jsonObject.ToJsonString()))
                                          .ToList();
            PhoneDTO fone = fonesdto.Count() == 0 ? null : fonesdto[0];
            User user = User.restore(dtos[0],fone);
            _context.close();
            return user;
        }

        public async Task<User> getUserBase(string id)
        {
            await _context.connect(_connectString);
            string commandBase = "SELECT name,email,password,iduser,category from \"user\" where \"user\".iduser = @iduser";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@iduser",id }
            };
            List<JsonObject> result = await _context.read(commandBase, parameters);
            List<UserTutorDb> dtos = result.Select(jsonObject => JsonSerializer.Deserialize<UserTutorDb>(jsonObject.ToJsonString()))
                                           .ToList();
            if (dtos.Count == 0)
                throw new Exception("user not found");
            _context.close();
            User user = User.restore(dtos[0]);
            return user;
        }

        public async Task save(User user)
        {
            await _context.connect(_connectString);
            string commandBase = "INSERT INTO \"user\" (name,email,password,iduser,category) VALUES (@name,@email,@password,@iduser,@category)";
            string commandSaveClinic = "INSERT INTO \"clinic\" (iduser,plan,cnpj) VALUES (@iduser,@plan,@cnpj)";
            string commandAdress = "INSERT INTO \"adress\" (iduser,idadress,uf,city,street,number,cep) VALUES (@iduser,@idadress,@uf,@city,@street,@number,@cep)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@name",user.name },
                {"@email",user.email },
                {"@password",user.password },
                {"@iduser",user.id },
                {"@category",user.categoryCode },
            };
            Dictionary<string, object> parametersClinic = new Dictionary<string, object>()
            {
                {"@iduser",user.id },
                {"@plan",user.plan },
                {"@cnpj",user.cnpj }
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
            await _context.command(commandBase, parameters);
            await _context.command(commandSaveClinic, parametersClinic);
            await _context.command(commandAdress, parametersAdress);
            _context.close();
        }

        public Task update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
