using app.Domain.DTO.Service;

namespace app.Application.DTO
{
    public class OutputProntuarioDTO
    {
        public Tutor tutor { get; set; }
        public PetOS pet {  get; set; }
        public string idprontuario { get; set; }
        public DateTime datecreate { get; set; }
        public List<OutputOsFromProntuario> orders { get; set; }
        public List<string> idsattendance {  get; set; }
    }
    public class OutputOsFromProntuario
    {
        public bool waaccepted { get; set; }
        public bool tutorcancelled { get; set; }
        public string idorderservice { get; set; }
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
        public string attendancemodel { get; set; }
        public List<VaccineDbDTO> vaccines { get; set; }
        public List<ServiceSubCategoryDb> subcategories { get; set; }
    }
}
