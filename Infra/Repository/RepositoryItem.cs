using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Item;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace app.Infra.Repository
{
    public class RepositoryItem : IRepositoryItem
    {
        private IFactoryDbContext _factoryDbContext;
        private IDbContext _context;
        private string _connectString = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=petprodb";
        public RepositoryItem(IFactoryDbContext factorycontext)
        {
            _factoryDbContext = factorycontext;
            _context = _factoryDbContext.psqlContext();
        }
        public async Task<Item> getbyid(string id)
        {
            await _context.connect(_connectString);

            // Consulta base para a tabela "item"
            string commandBase = "SELECT itemid, iduser, category, description, name, unity, datecreate,wasexclude FROM \"item\" WHERE itemid = @itemid and wasexclude=false";

            // Consulta para buscar os tamanhos dos itens na tabela "itemsize"
            string commandGetSizes = "SELECT itemsize.iditemsize, itemsize.iditem, itemsize.size, itemsize.price, itemsize.avalaible FROM \"itemsize\" inner join item on itemsize.iditem=item.itemid where item.itemid=@itemid";
            string commandgetsummaries = "SELECT summary from summaries where iditem = @iditem";
            string commandgetspecifications = "SELECT tag,name from specification where iditem = @iditem";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
             {
               {"@itemid", id}
             };
            Dictionary<string, object> parametersid = new Dictionary<string, object>()
             {
               {"@iditem", id}
             };

            // Realiza a leitura dos itens associados ao usuário
            List<JsonObject> result = await _context.read(commandBase, parameters);
            List<JsonObject> sizes = await _context.read(commandGetSizes, parameters);
            List<JsonObject> summaries = await _context.read(commandgetsummaries, parametersid);
            List<JsonObject> specifications = await _context.read(commandgetspecifications, parametersid);

            List<ItemDTODb> itemDtos = result.Select(jsonObject => JsonSerializer.Deserialize<ItemDTODb>(jsonObject.ToJsonString()))
                                             .ToList();
            List<ItemSizeDTODb> sizesdto = sizes.Select(jsonObject => JsonSerializer.Deserialize<ItemSizeDTODb>(jsonObject.ToJsonString()))
                                            .ToList();
            List<Summary> summariesdto = summaries.Select(jsonObject => JsonSerializer.Deserialize<Summary>(jsonObject.ToJsonString()))
                                            .ToList();
            List<Specification> specificationsdto = specifications.Select(jsonObject => JsonSerializer.Deserialize<Specification>(jsonObject.ToJsonString()))
                                            .ToList();

            if (itemDtos.Count == 0)
            {
                _context.close();
                return null;
            }
            Item item = Item.restore(itemDtos.FirstOrDefault(), sizesdto, summariesdto, specificationsdto);
            _context.close();

            return item;
        }

        public async Task<List<Item>> getbyiduser(string id)
        {

            await _context.connect(_connectString);

            List<Item> items = new List<Item>();

            // Consulta base para a tabela "item"
            string commandBase = "SELECT itemid, iduser, category, description, name, unity, datecreate, wasexclude FROM \"item\" WHERE iduser = @iduser and wasexclude=false";

            // Consulta para buscar os tamanhos dos itens na tabela "itemsize"
            string commandGetSizes = "SELECT iditemsize, iditem, size, price, avalaible FROM \"itemsize\" WHERE iditem = @iditem";
            string commandgetsummaries = "SELECT summary from summaries where iditem = @iditem";
            string commandgetspecifications = "SELECT tag,name from specification where iditem = @iditem";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
    {
        {"@iduser", id}
    };

            Dictionary<string, object> parameterIdItem = new Dictionary<string, object>();

            // Realiza a leitura dos itens associados ao usuário
            List<JsonObject> result = await _context.read(commandBase, parameters);

            List<ItemDTODb> itemDtos = result.Select(jsonObject => JsonSerializer.Deserialize<ItemDTODb>(jsonObject.ToJsonString()))
                                             .ToList();

            if (itemDtos.Count == 0)
            {
                _context.close();
                return null;
            }

            foreach (ItemDTODb itemDto in itemDtos)
            {
                parameterIdItem["@iditem"] = itemDto.itemid;

                // Busca os tamanhos do item
                List<JsonObject> sizesResult = await _context.read(commandGetSizes, parameterIdItem);
                List<JsonObject> summaries = await _context.read(commandgetsummaries, parameterIdItem);
                List<JsonObject> specifications = await _context.read(commandgetspecifications, parameterIdItem);

                List<ItemSizeDTODb> sizes = sizesResult.Select(jsonObject => JsonSerializer.Deserialize<ItemSizeDTODb>(jsonObject.ToJsonString()))
                                                       .ToList();
                List<Summary> summariesdto = summaries.Select(jsonObject => JsonSerializer.Deserialize<Summary>(jsonObject.ToJsonString()))
                                           .ToList();
                List<Specification> specificationsdto = specifications.Select(jsonObject => JsonSerializer.Deserialize<Specification>(jsonObject.ToJsonString()))
                                                .ToList();

                // Restaura o objeto Item usando os dados do banco e os tamanhos associados
                Item restoredItem = Item.restore(itemDto, sizes, summariesdto, specificationsdto);

                items.Add(restoredItem);
            }

            _context.close();

            return items;
        }

        public async Task save(Item item)
        {
            // Conexão com o banco de dados
            await _context.connect(_connectString);

            // Comando para inserir na tabela item
            string commandItem = "INSERT INTO item (itemid, iduser, category, description, name, unity, datecreate,wasexclude) VALUES (@itemid, @iduser, @category, @description, @name, @unity, @datecreate,@wasexclude)";

            // Parâmetros para o comando item
            Dictionary<string, object> parametersItem = new Dictionary<string, object>()
    {
        { "@itemid", item.itemid },
        { "@iduser", item.iduser },
        { "@category", item.category },
        { "@description", item.description },
        { "@name", item.name },
        { "@unity", item.unity },
        { "@datecreate", item.datecreate },
        {"@wasexclude", item.wasexclude }
    };

            // Executa o comando para salvar o item
            await _context.command(commandItem, parametersItem);

            // Comando para inserir na tabela itemsize
            string commandItemSize = "INSERT INTO itemsize (iditem, iditemsize, size, price, avalaible) VALUES (@iditem, @iditemsize, @size, @price, @avalaible)";
            string commandSpecifications = "INSERT INTO specification (iditem,tag,name) VALUES (@iditem,@tag,@name)";
            string commandSummary = "INSERT INTO summaries (iditem,summary) VALUES (@iditem,@summary)";
            // Loop pelos tamanhos associados ao item
            foreach (var size in item.sizes)
            {
                Dictionary<string, object> parametersItemSize = new Dictionary<string, object>()
                {
                    { "@iditem", size.iditem },
                    { "@iditemsize", size.iditemsize },
                    { "@size", size.size },
                    { "@price", size.price },
                    { "@avalaible", size.avalaible }
                };
                await _context.command(commandItemSize, parametersItemSize);
            }
            foreach (Specification specification in item.specificationsdto)
            {
                Dictionary<string, object> parametersspecifications = new Dictionary<string, object>()
                {
                    { "@iditem", item.itemid },
                    { "@tag", specification.tag },
                    { "@name", specification.name }
                };
                await _context.command(commandSpecifications, parametersspecifications);
            }
            foreach (Summary summary in item.summaries)
            {
                Dictionary<string, object> parameterssummaries = new Dictionary<string, object>()
                {
                    { "@iditem", item.itemid },
                    { "@summary", summary.summary }
                };
                await _context.command(commandSummary, parameterssummaries);
            }
            _context.close();
        }

        public async Task udpate(Item item)
        {
            await _context.connect(_connectString);
            string commandupdate = "UPDATE item SET category=@category,description=@description,name=@name,unity=@unity,wasexclude=@wasexclude WHERE itemid=@itemid";
            Dictionary<string, object> parametersItem = new Dictionary<string, object>()
    {
        { "@itemid", item.itemid },
        { "@iduser", item.iduser },
        { "@category", item.category },
        { "@description", item.description },
        { "@name", item.name },
        { "@unity", item.unity },
        { "@datecreate", item.datecreate },
        {"@wasexclude", item.wasexclude }
    };
            await _context.command(commandupdate, parametersItem);
            return;
        }
    }
}
