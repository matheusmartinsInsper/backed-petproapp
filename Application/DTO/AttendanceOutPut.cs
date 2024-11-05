using app.Domain.DTO.Service;

namespace app.Application.DTO
{
    public class AttendanceOutPut
    {
        public string idattendance {  get; set; }
        public string status { get; set; }
        public string hipotese { get; set; }
        public string conclusao { get; set; }
        public Serviceoutput service {  get; set; }
        public PetOS pet { get; set; }
        public Tutor tutor { get; set; }
    }
    public class Serviceoutput
    {
        public string priority { get; set; }
        public object category { get; set; }
        public string title { get; set; }
        public float? totalprice { get; set; }
        public string description { get; set; }
        public DateTime datesolicitation { get; set; }
        public DateTime dateapontted { get; set; }
        public string comments { get; set; }
        public string waspaid { get; set; }
        public string atendimento { get; set; }
        public List<VaccineDbDTO> vacinas { get; set; }
        public List<ServiceSubCategoryDb> subcategorias { get; set; }
       
    }
}
