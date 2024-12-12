using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Stock;

namespace app.Application.UseCase
{
    public class MoveStock
    {
        private IRepositoryStock _repostock;
        private IRepositoryItem _repoitem;
        public MoveStock(IRepositoryStock repostock, IRepositoryItem repoitem)
        {
            _repostock = repostock;
            _repoitem = repoitem;
        }
        public async Task execute(TransactionDTO transaction,string iduser)
        {
            List<Stock> stocks = new List<Stock>();
            Item item = await _repoitem.getbyid(transaction.iditem);
            if (item == null)
                throw new Exception("Item não encontrado");
            stocks = await _repostock.getByIditem(transaction.iditem);
            Stock stock = stocks.Where((x)=>x.idstock==transaction.idstock&&x.lote==transaction.lote).FirstOrDefault();
            if (stock != null)
            {
                stock.moveStock(transaction);
                await _repostock.update(stock);
                return;
            }
            StockDTO stockDTO = new StockDTO()
            {
                iditem = transaction.iditem,
                iduser = iduser,
            };
            Stock stocknew = Stock.create(stockDTO);
            stocknew.moveStock(transaction);
            await _repostock.save(stocknew);
            return;
        }
    }
}
