namespace app.Domain.DTO.Service
{
    public class ServiceDTO
    {
        public string codecategory { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string idUser { get; set; }
        public float? price { get; set; }
        public List<VaccineDTO>? vaccines { get; set; }
        public List<ServiceSubCategory> subcategories { get; set; }
        public List<string> typeofatendimento { get; set; }
    }
}
