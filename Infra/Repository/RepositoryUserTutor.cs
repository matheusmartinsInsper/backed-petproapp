using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Fone;
using app.Domain.DTO.User;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace app.Infra.Repository
{
    public class RepositoryUserTutor : IRepositoryUserTutor
    {
        private IFactoryDbContext _factoryDbContext;
        private IDbContext _context;
        private string _connectString = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=petprodb";
        public RepositoryUserTutor(IFactoryDbContext factorycontext) 
        {
            _factoryDbContext = factorycontext;
            _context = _factoryDbContext.psqlContext();
        }
        public async Task<User> get(string id)
        {
            await _context.connect(_connectString);
            string commandBase = "SELECT name,email,password,iduser,category,dateborn from \"user\" where iduser = @iduser";
            string commandGetNumber = "SELECT idfone,iduser,countrycode,areacode,phone,createat FROM fone where iduser = @iduser";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@iduser",id }
            };
            List<JsonObject> result = await _context.read(commandBase, parameters);
            foreach (var jsonObject in result)
            {
                if (jsonObject.TryGetPropertyValue("dateborn", out var comments))
                {
                    if (comments.ToString() == "{}")
                    {
                        jsonObject["dateborn"] = null;
                    }
                }
            }
            List<UserTutorDb> dtos = result.Select(jsonObject => JsonSerializer.Deserialize<UserTutorDb>(jsonObject.ToJsonString()))
                                           .ToList();
            List<JsonObject> fones = await _context.read(commandGetNumber, parameters);
            List<PhoneDTO> fonesdto = fones.Select(jsonObject => JsonSerializer.Deserialize<PhoneDTO>(jsonObject.ToJsonString()))
                                         .ToList();
            _context.close();
            PhoneDTO fone = fonesdto.Count() == 0 ? null : fonesdto[0];
            User user = User.restore(dtos[0],fone);
            return user;
        }

        public async Task save(User user)
        {
            await _context.connect(_connectString);
            string commandBase = "INSERT INTO \"user\" (name,email,password,iduser,category,dateborn) VALUES (@name,@email,@password,@iduser,@category,@dateborn)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@name",user.name },
                {"@email",user.email },
                {"@password",user.password },
                {"@iduser",user.id },
                {"@category",user.categoryCode },
                {"@dateborn",user.dateborn },
            };
            await _context.command(commandBase,parameters);
            _context.close();
        }
        public async Task<User> getByEmail(string email)
        {
            await _context.connect(_connectString);
            string commandBase = "SELECT name,email,password,iduser,category,dateborn from \"user\" where email = @email";
            string commandGetNumber = "SELECT idfone,iduser,countrycode,areacode,phone,createat FROM fone where iduser = @iduser";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@email",email }
            };
            List<JsonObject> result = await _context.read(commandBase, parameters);
            foreach (var jsonObject in result)
            {
                if (jsonObject.TryGetPropertyValue("dateborn", out var comments))
                {
                    if (comments.ToString() == "{}")
                    {
                        jsonObject["dateborn"] = null;
                    }
                }
            }
            List<UserTutorDb> dtos = result.Select(jsonObject => JsonSerializer.Deserialize<UserTutorDb>(jsonObject.ToJsonString()))
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

        public async Task delete(User user)
        {
            await _context.connect(_connectString);
            string commandBase = "DELETE FROM \"user\" WHERE iduser = @iduser";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@iduser",user.id}
            };
            await _context.command(commandBase, parameters);
            _context.close();
        }
    }
}
