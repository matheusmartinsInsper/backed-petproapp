using app.Domain.DTO.Service;

namespace app.Application.DTO
{
    public class OrderServiceOutputDTO
    {
        public bool waaccepted { get; set; }
        public bool tutorcancelled { get; set; }
        public string idorderservice { get; set; }
        public string idattendance { get; set; }
        public string idservice { get; set; }
        public string nameuserowner { get; set; }
        public string nameprofissional { get; set; }
        public DateTime datesolicitation { get; set; }
        public DateTime dateappointed { get; set; }
        public string status { get; set; }
        public object categoryname { get; set; }
        public string title { get; set; }
        public float? price { get; set; }
        public string? comments { get; set; }
        public string priority { get; set; }
        public PetOS pet { get; set; }
        public Tutor tutor { get; set; }
        public string attendancemodel { get; set; }
        public List<VaccineDbDTO> vaccines { get; set; }
        public List<ServiceSubCategoryDb> subcategories { get; set; }
    }
    public class PetOS
    {
        public string age { get; set; }
        public string petname { get; set; }
        public float weight { get; set; }
        public string race { get; set; }
        public string species { get; set; }
        public string sex { get; set; }
        public bool castrated {  get; set; }
    }
    public class Tutor
    {
        public string name { get; set; }
        public string email { get; set; }
        public string number { get; set; }
    }
}