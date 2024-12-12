using app.Domain.DTO.Item;

namespace app.Application.DTO
{
    public class StockOutputcs
    {
        public string idstock { get; set; }
        public string iditem { get; set; }
        public string iditemsize { get; set; }
        public DateTime createdate { get; set; }
        public DateTime updatedate { get; set; }
        public string lote { get; set; }
        public int quantity { get; set; }
        public string nameitem { get; set; }
        public string categoryitem { get; set; }
        public string unity { get; set; }
        public string description { get; set; }
        public ItemSizeDTODb itemsize { get; set; }
    }
}
