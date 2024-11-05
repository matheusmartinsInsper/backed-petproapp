using app.Application.IRepository;
using app.Domain.Agregate.Entities;
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
            _context.close();
            User user = User.restore(dtos[0]);
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
            User user = User.restore(dtos[0]);
            _context.close();
            return user;
        }
        public Task update(User user)
        {
            throw new NotImplementedException();
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
