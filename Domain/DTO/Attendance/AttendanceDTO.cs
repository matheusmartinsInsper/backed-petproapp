using app.Domain.Agregate.Entities;
using app.Domain.DTO.Payment;

namespace app.Domain.DTO.Attendance
{
    public class AttendanceDTO
    {
        public string IdOrderService { get; set; }
        public string Hipoteses { get; set; }
        public bool waspaid { get; set; }
        public string Conclusao { get; set; }
        public string idpet {  get; set; }
        public string idtutor { get; set; }
        public string idowner { get; set; }
        public string iduserattendance { get; set; }
    }
}
