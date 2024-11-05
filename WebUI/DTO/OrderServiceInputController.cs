namespace app.WebUI.DTO
{
    public class OrderServiceInputController
    {

        public string idservice { get; set; }
        public string priority {  get; set; }
        public List<string> idsubcategories { get; set; }
        public List<string> idvaccines { get; set; }
        public string? comments { get; set; }
        public string idpet { get; set; }
        public DateTime dateappointed { get; set; }
        public string attendancemodel { get; set; }
    }
}
