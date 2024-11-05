namespace app.Domain.DTO.OrderService
{
    public class OrderServiceDbDTO
    {
        public string idorderservice { get; set; }
        public string idservice { get; set; }
        public string priority { get; set; }
        public string idusertutor { get; set; }
        public string iduserrecipient { get; set; }
        public string iduseraccepted { get; set; }
        public string iduserattendance { get; set; }
        public DateTime dateofsolicitation { get; set; }
        public DateTime dateappointed { get; set; }
        public string status    { get; set; }
        public string? comments { get; set; }
        public string idpet {  get; set; }
        public string attendencemodel { get; set; }
        public bool wasaccepted { get; set; }
        public bool tutorcancell { get; set; }
        public string idattendance { get;set; }
    }
}
