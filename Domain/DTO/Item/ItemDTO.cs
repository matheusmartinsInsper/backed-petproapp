namespace app.Domain.DTO.Item
{
    public class ItemDTO
    {
        public string category { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string unity { get; set; }
        public List<ItemSizeDTO> sizes { get; set; }
    }
    public class ItemSizeDTO
    {
        public string size { get; set; }
        public float price { get; set; }
        public bool avalaible { get; set; }
    }
  
}
