using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Item;
using app.Domain.DTO.Stock;
using System.Transactions;

namespace app.Application.UseCase
{
    public class GetStockByUser
    {

        private IRepositoryStock _repostock;
        private IRepositoryItem _repoitem;
        public GetStockByUser(IRepositoryStock repostock, IRepositoryItem repoitem)
        {
            _repostock = repostock;
            _repoitem = repoitem;
        }
        public async Task<List<StockOutputcs>> execute(string iduser)
        {
            List<Stock> stocks = await _repostock.getbyuser(iduser);
            List<StockOutputcs> stocksdto = new List<StockOutputcs>();
            foreach (Stock stock in stocks)
            {
                StockOutputcs stockdto = new StockOutputcs();
                stockdto.transactions = new List<TransactionDTODb>();
                Item item = await _repoitem.getbyid(stock.iditem);
                ItemSize size = item.sizes.Where((x)=>x.iditemsize==stock.iditemsize).FirstOrDefault(); 
                ItemSizeDTODb sizedto = new ItemSizeDTODb();
                sizedto.iditemsize = size.iditemsize;
                sizedto.price = size.price;
                sizedto.size = size.size;
                sizedto.avalaible = size.avalaible;
                stockdto.quantity = stock.quantity;
                stockdto.lote = stock.lote;
                stockdto.idstock = stock.idstock;
                stockdto.createdate = stock.datecreate;
                stockdto.updatedate = stock.dateupdate;
                stockdto.iditem = stock.iditem;
                stockdto.iditemsize = stock.iditemsize;
                stockdto.nameitem = item.name;
                stockdto.description = item.description;
                stockdto.categoryitem = item.category;
                stockdto.unity = item.unity;
                stockdto.itemsize = sizedto;
                stockdto.transactions = stock.transactions.Select(transaction => new TransactionDTODb
                {
                    iditem = transaction.iditem,
                    lote = transaction.lote,
                    quantity = transaction.quantity,
                    iditemsize = transaction.iditemsize,
                    idtransaction = transaction.idtransaction,
                    idstock = transaction.idstock,
                    priceunity = transaction.priceunity,
                    datecreate = transaction.datecreate,
                    type = transaction.type
                }).ToList();
                stocksdto.Add(stockdto);
               
            }
            return stocksdto;
        }
    }
}
