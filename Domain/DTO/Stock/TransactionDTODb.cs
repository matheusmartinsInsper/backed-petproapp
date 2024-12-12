namespace app.Domain.DTO.Stock
{
    public class TransactionDTODb
    {
        public string idstock { get; set; }
        public string iditem { get; set; }
        public string iditemsize { get; set; }
        public string idtransaction { get; set; }
        public string type { get; set; }
        public string lote { get; set; }
        public float priceunity { get; set; }
        public int quantity { get; set; }
        public DateTime datecreate { get; set; }
    }
}
