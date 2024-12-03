using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Pet;
using System.Text.Json.Nodes;
using System.Text.Json;
using app.Domain.DTO.NetWorkCollaborator;
using app.Domain.DTO.User;
using app.Domain.DTO.Fone;

namespace app.Infra.Repository
{
    public class RepositoryNetWork : IRepositoryNetWork
    {

        private IFactoryDbContext _factoryDbContext;
        private IDbContext _context;
        private string _connectString = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=petprodb";
        public RepositoryNetWork(IFactoryDbContext factorycontext)
        {
            _factoryDbContext = factorycontext;
            _context = _factoryDbContext.psqlContext();
        }
        public Task<NetWorkCollaborators> get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<NetWorkCollaborators> getByUser(string iduser)
        {
            await _context.connect(_connectString);
            NetWorkDbDTO net = new NetWorkDbDTO();
            List<string> collab = new List<string>();

            string commandBase = "SELECT iduserprimary,numbermaxuser from network " +
                "where iduserprimary = @iduserprimary";

            string commandgetcollaborators = "SELECT idcollaborator from network " +
                "where iduserprimary = @iduserprimary";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@iduserprimary",iduser}
            };

            List<JsonObject> resultbase = await _context.read(commandBase, parameters);
            List<JsonObject> result = await _context.read(commandgetcollaborators, parameters);

            List<NetWorkMetaData> network = resultbase.Select(jsonObject => JsonSerializer.Deserialize<NetWorkMetaData>(jsonObject.ToJsonString()))
                                           .ToList();

            List<NetWorkMetaDataCollaborators> idcollaboratos = result.Select(jsonObject => JsonSerializer.Deserialize<NetWorkMetaDataCollaborators>(jsonObject.ToJsonString()))
                                           .ToList();
            net.iduserprimary = network[0].iduserprimary;
            net.numbermaxuser = network[0].numbermaxuser;
            foreach(NetWorkMetaDataCollaborators ids in idcollaboratos)
            {
                collab.Add(ids.idcollaborator.ToString());
            }
            net.idcollaborator = collab;
            NetWorkCollaborators mynetwork = NetWorkCollaborators.restore(net);
            _context.close();
            return mynetwork;
        }

        public async Task<List<UserBaseDTO>> getUsers(string idUserPrimary)
        {
            await _context.connect(_connectString);
            string commandBase = "select name,email from \"user\" t inner join network on t.iduser = network.idcollaborator where network.iduserprimary=@iduserprimary";
            string commandGetNumber = "SELECT idfone,iduser,countrycode,areacode,phone,createat FROM fone where iduser = @iduser";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
               {"@iduserprimary",idUserPrimary}
            };
            List<JsonObject> resultbase = await _context.read(commandBase, parameters);
            
            List<UserBaseDTO> users = resultbase.Select(jsonObject => JsonSerializer.Deserialize<UserBaseDTO>(jsonObject.ToJsonString()))
                                           .ToList();
            _context.close();
            return users;
        }

        public async Task save(NetWorkCollaborators network)
        {
            await _context.connect(_connectString);
            string commandRegisterVaccines = "INSERT INTO \"network\" (iduserprimary,idcollaborator,numbermaxuser) VALUES (@iduserprimary,@idcollaborator,@numbermaxuser)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                   {"@iduserprimary",network.iduserprimary},
                   {"@numbermaxuser", network.numbermaxcollaborator }
                };
            foreach (string idcollaborator in network.idcollaborator)
            {
                parameters["@idcollaborator"] = idcollaborator;
                await _context.command(commandRegisterVaccines, parameters);
            }
            _context.close();
        }

        public async Task update(NetWorkCollaborators network)
        {
            await _context.connect(_connectString);
            string commandRegister = "INSERT INTO \"network\" (iduserprimary,idcollaborator,numbermaxuser) VALUES (@iduserprimary,@idcollaborator,@numbermaxuser)";
            string commandDeletNet = "DELETE FROM network WHERE iduserprimary = @iduserprimary";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@iduserprimary",network.iduserprimary},
                {"@numbermaxuser", network.numbermaxcollaborator }
            };
            Dictionary<string, object> parametersdelete = new Dictionary<string, object>()
            {
                {"@iduserprimary",network.iduserprimary}
            };
            
            await _context.command(commandDeletNet, parametersdelete);

            foreach (string idcollaborator in network.idcollaborator)
            {
                parameters["@idcollaborator"] = idcollaborator;
                await _context.command(commandRegister, parameters);
            }
            _context.close();
        }
    }
}
