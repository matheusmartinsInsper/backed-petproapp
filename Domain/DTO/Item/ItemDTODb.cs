namespace app.Domain.DTO.Item
{
    public class ItemDTODb
    {
        public string itemid {  get; set; }
        public string iduser { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string unity { get; set; }
        public DateTime datecreate { get; set; }
    }
    public class ItemSizeDTODb
    {
        public string iditem { get; set; }
        public string iditemsize { get; set; }
        public string size { get; set; }
        public float price { get; set; }
        public bool avalaible { get; set; }
    }
}
