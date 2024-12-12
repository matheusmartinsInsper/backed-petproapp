using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Service;
using app.Domain.DTO.Stock;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace app.Infra.Repository
{
    public class RepositoryStock : IRepositoryStock
    {
        private IFactoryDbContext _factoryDbContext;
        private IDbContext _context;
        private string _connectString = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=petprodb";
        public RepositoryStock(IFactoryDbContext factorycontext)
        {
            _factoryDbContext = factorycontext;
            _context = _factoryDbContext.psqlContext();
        }
        public async Task save(Stock stock)
        {
            // Conexão com o banco de dados
            await _context.connect(_connectString);

            // Comando para inserir na tabela stock
            string commandStock = @"INSERT INTO stock 
                            (idstock, iditem, iditemsize, iduser, quantity, lote, datecreate, dateupdate) 
                            VALUES 
                            (@idstock, @iditem, @iditemsize, @iduser, @quantity, @lote, @datecreate, @dateupdate)";

            // Parâmetros para o comando stock
            Dictionary<string, object> parametersStock = new Dictionary<string, object>()
    {
        { "@idstock", stock.idstock },
        { "@iditem", stock.iditem },
        { "@iditemsize", stock.iditemsize },
        { "@iduser", stock.iduser },
        { "@quantity", stock.quantity },
        { "@lote", stock.lote },
        { "@datecreate", stock.datecreate },
        { "@dateupdate", stock.dateupdate }
    };

            // Executa o comando para salvar o estoque
            await _context.command(commandStock, parametersStock);

            // Comando para inserir na tabela transaction
            string commandTransaction = "INSERT INTO \"transaction\" (idtransaction, idstock, iditem, iditemsize, type, lote, priceunity, quantity, datecreate) "+
                                  "VALUES (@idtransaction, @idstock, @iditem, @iditemsize, @type, @lote, @priceunity, @quantity, @datecreate)";

            // Loop pelas transações associadas ao estoque
            foreach (TransactionDTODb transaction in stock.transactions)
            {
                Dictionary<string, object> parametersTransaction = new Dictionary<string, object>()
        {
            { "@idtransaction", transaction.idtransaction },
            { "@idstock", stock.idstock },
            { "@iditem", transaction.iditem },
            { "@iditemsize", transaction.iditemsize },
            { "@type", transaction.type },
            { "@lote", transaction.lote },
            { "@priceunity", transaction.priceunity },
            { "@quantity", transaction.quantity },
            { "@datecreate", transaction.datecreate }
        };

                // Executa o comando para salvar a transação
                await _context.command(commandTransaction, parametersTransaction);
            }

            // Fecha a conexão
            _context.close();
        }


        public Task<Stock> getbyid(string idstock)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Stock>> getByIditem(string iditem)
        {
            List<Stock> Stocks = new List<Stock>();
            await _context.connect(_connectString);
            string commandBase = "SELECT idstock,iditem,iditemsize,iduser,quantity,lote,datecreate,dateupdate FROM \"stock\" where iditem = @iditem";
            string commandtransaction = "select transaction.idtransaction,transaction.iditem,transaction.lote,transaction.idstock,transaction.iditemsize,transaction.type,transaction.priceunity,transaction.quantity,transaction.datecreate " +
                                        "from transaction inner join stock on stock.idstock = transaction.idstock";
           
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@iditem",iditem }
            };

            List<JsonObject> result = await _context.read(commandBase, parameters);

            List<StockDTODb> stocks = result.Select(jsonObject => JsonSerializer.Deserialize<StockDTODb>(jsonObject.ToJsonString()))
                                          .ToList();


            foreach (StockDTODb stock in stocks)
            {
                parameters["@idstock"] = stock.idstock;
                List<JsonObject> resulttransaction = await _context.read(commandtransaction, parameters);
                List<TransactionDTODb> transactions = resulttransaction.Count() == 0 ? new List<TransactionDTODb>() : resulttransaction.Select(jsonObject => JsonSerializer.Deserialize<TransactionDTODb>(jsonObject.ToJsonString()))
                                          .ToList();
                Stock STOCK = Stock.restore(stock, transactions);
                Stocks.Add(STOCK);
            }
            _context.close();
            return Stocks;
        }

        public async Task<List<Stock>> getbyuser(string iduser)
        {
            List<Stock> Stocks = new List<Stock>();
            await _context.connect(_connectString);
            string commandBase = "SELECT idstock,iditem,iditemsize,iduser,quantity,lote,datecreate,dateupdate FROM \"stock\" where iduser = @iduser";
            string commandtransaction = "select transaction.idtransaction,transaction.iditem,transaction.lote,transaction.idstock,transaction.iditemsize,transaction.type,transaction.priceunity,transaction.quantity,transaction.datecreate " +
                                        "from transaction inner join stock on stock.idstock = transaction.idstock where transaction.idstock = @idstock";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@iduser",iduser }
            };

            List<JsonObject> result = await _context.read(commandBase, parameters);

            List<StockDTODb> stocks = result.Select(jsonObject => JsonSerializer.Deserialize<StockDTODb>(jsonObject.ToJsonString()))
                                          .ToList();


            foreach (StockDTODb stock in stocks)
            {
                parameters["@idstock"] = stock.idstock;
                List<JsonObject> resulttransaction = await _context.read(commandtransaction, parameters);
                List<TransactionDTODb> transactions = resulttransaction.Count() == 0 ? new List<TransactionDTODb>() : resulttransaction.Select(jsonObject => JsonSerializer.Deserialize<TransactionDTODb>(jsonObject.ToJsonString()))
                                          .ToList();
                Stock STOCK = Stock.restore(stock, transactions);
                Stocks.Add(STOCK);
            }
            _context.close();
            return Stocks;
        }

        public async Task update(Stock stock)
        {
            // Abre a conexão com o banco de dados
            await _context.connect(_connectString);

            // Comando para atualizar a tabela stock
            string commandStock = @"UPDATE stock 
                             SET quantity = @quantity, 
                                 lote = @lote, 
                                 dateupdate = @dateupdate 
                             WHERE idstock = @idstock";

            // Parâmetros para o comando de atualização
            Dictionary<string, object> parametersStock = new Dictionary<string, object>()
    {
        { "@idstock", stock.idstock },
        { "@quantity", stock.quantity },
        { "@lote", stock.lote },
        { "@dateupdate", stock.dateupdate }
    };

            // Executa o comando de atualização na tabela stock
            await _context.command(commandStock, parametersStock);

            // Obtém a última transação da lista de transações
            var lastTransaction = stock.transactions.LastOrDefault();

            if (lastTransaction != null)
            {
                // Comando para inserir na tabela transaction
                string commandTransaction = @"INSERT INTO transaction 
                                      (idtransaction, idstock, iditem, iditemsize, type, lote, priceunity, quantity, datecreate) 
                                      VALUES 
                                      (@idtransaction, @idstock, @iditem, @iditemsize, @type, @lote, @priceunity, @quantity, @datecreate)";

                // Parâmetros para o comando transaction
                Dictionary<string, object> parametersTransaction = new Dictionary<string, object>()
        {
            { "@idtransaction", lastTransaction.idtransaction },
            { "@idstock", stock.idstock },
            { "@iditem", lastTransaction.iditem },
            { "@iditemsize", lastTransaction.iditemsize },
            { "@type", lastTransaction.type },
            { "@lote", lastTransaction.lote },
            { "@priceunity", lastTransaction.priceunity },
            { "@quantity", lastTransaction.quantity },
            { "@datecreate", lastTransaction.datecreate }
        };

                // Executa o comando para inserir a transação
                await _context.command(commandTransaction, parametersTransaction);
            }

            // Fecha a conexão
            _context.close();
        }

    }
}
