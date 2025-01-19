using app.Domain.DTO.Item;

namespace app.Application.DTO
{
    public class ItemDTOOutput
    {
        public string itemid { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string unity { get; set; }
        public DateTime datecreate { get; set; }
        public List<Summary> summaries { get; set; }
        public List<Specification> specifications { get; set; }
        public List<ItemSizeDTODb> sizes { get; set; }
    }
   
}
