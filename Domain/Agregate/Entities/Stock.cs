using app.Domain.DTO.Stock;
using System.Data;

namespace app.Domain.Agregate.Entities
{
    public class Stock
    {
        private string _idstock;
        private string _iditem;
        private string _iditemsize;
        private string _iduser;
        private int _quantity;
        private string _lote;
        private DateTime _datecreate;
        private DateTime _dateupdate;
        private List<TransactionDTODb> _transactions;
        public string idstock { get { return _idstock; } }
        public string iditem { get { return _iditem; } }
        public string iditemsize {  get { return _iditemsize; } }
        public string lote { get { return _lote; } }
        public string iduser { get { return _iduser; } }
        public int quantity { get { return _quantity; } }
        public DateTime datecreate { get { return _datecreate; } }
        public DateTime dateupdate { get { return _dateupdate; } }
        public List<TransactionDTODb> transactions { get { return _transactions; } }
        private class Transaction
        {

            private string _idstock;
            private string _iditem;
            private string _iditemsize;
            private string _idtransaction;
            private string _type;
            private string _lote;
            private float _priceunity;
            private int _quantity;
            private DateTime _datecreate;
            public string idstock { get { return _idstock; } }
            public string iditem { get { return _iditem; } }
            public string iditemsize { get { return _iditemsize; } }
            public string idtransaction { get { return _idtransaction; } }
            public string type { get { return _type; } }
            public string lote { get { return _lote; } }

            public float priceunity { get { return _priceunity; } }
            public DateTime datecreate { get { return _datecreate; } }
            public int quantity { get { return _quantity; } }
            private Transaction() { }
            public static Transaction create(TransactionDTO transactiondto)
            {
                Transaction transaction = new Transaction();
                transaction._idstock = transactiondto.idstock;
                transaction._iditem = transactiondto.iditem;
                transaction._iditemsize = transactiondto.iditemsize;
                transaction._idtransaction = Guid.NewGuid().ToString("N");
                transaction._lote = transactiondto.lote;
                transaction._quantity = transactiondto.quantity;
                transaction._priceunity = transactiondto.priceunity;
                transaction._datecreate = DateTime.Now;
                transaction._type = transactiondto.type=="Entrada"||transactiondto.type=="Saída"?transactiondto.type:
                                                                                                 throw new Exception("Movimentação não reconhecida");
                return transaction;
            }
        }

        private Stock() { }
        public static Stock create(StockDTO stockdto)
        {
            Stock stock = new Stock();
            stock._transactions = new List<TransactionDTODb>();
            stock._iditem = stockdto.iditem;
            stock._iditemsize = stockdto.iditemsize;
            stock._iduser = stockdto.iduser;
            stock._datecreate = DateTime.Now;
            stock._dateupdate = DateTime.Now;
            stock._quantity = 0;
            stock._lote = null;
            stock._idstock = Guid.NewGuid().ToString("N");
            return stock;
        }
        public static Stock restore(StockDTODb stockdto,List<TransactionDTODb> transactions)
        {
            Stock stock = new Stock();
            stock._transactions = new List<TransactionDTODb>();
            stock._idstock = stockdto.idstock;
            stock._iditem = stockdto.iditem;
            stock._iditemsize = stockdto.iditemsize;
            stock._iduser = stockdto.iduser;
            stock._datecreate = stockdto.datecreate;
            stock._dateupdate= stockdto.dateupdate;
            stock._lote = stockdto.lote;
            stock._quantity = stockdto.quantity;
            stock._transactions = transactions;
            return stock;
        }
        public void moveStock(TransactionDTO transactiondto)
        {
            Transaction transaction = Transaction.create(transactiondto);
            TransactionDTODb transactiondtodb = new TransactionDTODb()
            {
                lote = transaction.lote,
                iditem = transaction.iditem,
                idstock = transaction.idstock,
                idtransaction = transaction.idtransaction,
                quantity = transaction.quantity,
                datecreate = transaction.datecreate,
                priceunity = transaction.priceunity,
                iditemsize = transaction.iditemsize,
                type = transaction.type,
            };
            _transactions.Add(transactiondtodb);
            _lote = transaction.lote;
            _iditem = transaction.iditem;
            _iditemsize = transaction.iditemsize;
            if(transactiondto.lote == _lote && _lote != null)
            {
                if (transactiondto.type == "Entrada")
                {
                    _quantity = _quantity + transactiondtodb.quantity;
                    _dateupdate = transaction.datecreate;
                    return;
                }
                if (transactiondto.quantity > _quantity)
                    throw new Exception($"Quantidade em estoque inferior a baixa: quantidade em estoque {_quantity}");
                _quantity = _quantity - transactiondtodb.quantity;
                _dateupdate = transaction.datecreate;
                return;
            }
            throw new Exception("Lote não correspondente");
        }
    }
   
}
