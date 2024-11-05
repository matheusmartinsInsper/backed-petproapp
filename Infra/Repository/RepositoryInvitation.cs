using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.InviteCollaborator;
using app.Domain.DTO.Pet;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace app.Infra.Repository
{
    public class RepositoryInvitation : IRepositoryInvitation
    {
        private IFactoryDbContext _factoryDbContext;
        private IDbContext _context;
        private string _connectString = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=petprodb";
        public RepositoryInvitation(IFactoryDbContext factorycontext)
        {
            _factoryDbContext = factorycontext;
            _context = _factoryDbContext.psqlContext();
        }
        public async Task<InviteCollaborator> get(string id)
        {
            await _context.connect(_connectString);
            string command = "SELECT idinvitation,idusersender,emailuserrecipient,status,datecreate FROM invitation " +
                "WHERE idinvitation = @idinvitation";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
             {
                {"@idinvitation",id}
             };
            List<JsonObject> result = await _context.read(command, parameters);
            List<InviteDbDTO> dtos = result.Select(jsonObject => JsonSerializer.Deserialize<InviteDbDTO>(jsonObject.ToJsonString()))
                                           .ToList();
            if (dtos.Count() == 0)
                throw new Exception("this invite not exist");
            InviteCollaborator invite = InviteCollaborator.restore(dtos[0]);
            _context.close();
            return invite;
        }

        public async Task<List<InviteCollaborator>> getAll(string email)
        {
            await _context.connect(_connectString);
            List<InviteCollaborator> invites = new List<InviteCollaborator>();
            string command = "SELECT idinvitation,idusersender,emailuserrecipient,status,datecreate FROM invitation " +
                "WHERE emailuserrecipient = @emailuserrecipient and status = 'Pending'";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
             {
                {"@emailuserrecipient",email}
             };
            List<JsonObject> result = await _context.read(command, parameters);
            List<InviteDbDTO> dtos = result.Select(jsonObject => JsonSerializer.Deserialize<InviteDbDTO>(jsonObject.ToJsonString()))
                                           .ToList();
            foreach (InviteDbDTO dto in dtos)
            {
                InviteCollaborator invite = InviteCollaborator.restore(dto);
                invites.Add(invite);
            }
            _context.close();
            return invites;
        }

        public Task<InviteCollaborator> getByUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task save(InviteCollaborator invitation)
        {
            await _context.connect(_connectString);
            string command = "INSERT INTO \"invitation\" (idinvitation,idusersender,emailuserrecipient,status,datecreate) VALUES " +
                "(@idinvitation,@idusersender,@emailuserrecipient,@status,@datecreate)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
             {
                {"@idinvitation",invitation.idinvite},
                {"@idusersender", invitation.idusersender },
                {"@emailuserrecipient",invitation.emailrecipient},
                {"@status",invitation.status},
                {"@datecreate",invitation.datecreate},
             };
            await _context.command(command, parameters);
            _context.close();
        }

        public async Task update(InviteCollaborator invitation)
        {
            await _context.connect(_connectString);
            string command = "UPDATE  \"invitation\" SET idinvitation = @idinvitation,idusersender = @idusersender,emailuserrecipient = @emailuserrecipient,status = @status,datecreate = @datecreate " +
                "WHERE idinvitation = @idinvitation";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
             {
                {"@idinvitation",invitation.idinvite},
                {"@idusersender", invitation.idusersender },
                {"@emailuserrecipient",invitation.emailrecipient},
                {"@status",invitation.status},
                {"@datecreate",invitation.datecreate},
             };
            await _context.command(command, parameters);
            _context.close();
        }
    }
}
