using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Service;
using Sprache;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Input;

namespace app.Infra.Repository
{
    public class RepositoryPortfolioClient : IRepositoryPortfolioClient
    {
        private IFactoryDbContext _factoryDbContext;
        private IDbContext _context;
        private string _connectString = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=petprodb";

        public RepositoryPortfolioClient(IFactoryDbContext factorycontext)
        {
            _factoryDbContext = factorycontext;
            _context = _factoryDbContext.psqlContext();
        }
        public async Task<ClientPortfolio> get(string idowner)
        {
            await _context.connect(_connectString);
            string command = "select v.idclient,v.dateadd from \"user\" u left join portfolioclient v on u.iduser = v.idowner where u.iduser = @idowner";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idowner",idowner }
            };
            List<JsonObject> result = await _context.read(command, parameters);

            foreach (var jsonObject in result)
            {
                if (jsonObject.TryGetPropertyValue("idclient", out var comments))
                {
                    if (comments.ToString() == "{}")
                    {
                        jsonObject["idclient"] = idowner;
                    }
                }
            }
            foreach (var jsonObject in result)
            {
                if (jsonObject.TryGetPropertyValue("dateadd", out var comments))
                {
                    if (comments.ToString() == "{}")
                    {
                        jsonObject["dateadd"] = DateTime.Now;
                    }
                }
            }
            List<Client> clients = result.Select(jsonObject => JsonSerializer.Deserialize<Client>(jsonObject.ToJsonString()))
                                              .ToList();
            ClientPortfolio portfolio = ClientPortfolio.restore(clients, idowner);
            _context.close();
            return portfolio;
        }

        public async Task save(ClientPortfolio portfolio)
        {
            await _context.connect(_connectString);
            string commnad = "insert into \"portfolioclient\" (idclient,idowner,dateadd) values (@idclient,@idowner,@dateadd)";
            string delete = "delete from portfolioclient where idowner = @idowner";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idowner",portfolio.idowner }
            };
            await _context.command(delete, parameters);
            foreach(Client client in portfolio.clients)
            {
                parameters["@idclient"] = client.idclient;
                parameters["@dateadd"] = client.dateadd;
                await _context.command(commnad, parameters);
            }
            _context.close();
            return;
        }

        public Task update(ClientPortfolio portfolio)
        {
            throw new NotImplementedException();
        }
    }
}
