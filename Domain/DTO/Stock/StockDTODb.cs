namespace app.Domain.DTO.Stock
{
    public class StockDTODb
    {
        public string iditem { get; set; }
        public string iditemsize { get; set; }
        public string iduser { get; set; }
        public string idstock { get; set; }
        public int quantity { get; set; }
        public string lote { get; set; }
        public DateTime datecreate { get; set; }
        public DateTime dateupdate { get; set; }
    }
}
