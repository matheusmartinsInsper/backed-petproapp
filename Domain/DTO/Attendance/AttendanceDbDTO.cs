namespace app.Domain.DTO.Attendance
{
    public class AttendanceDbDTO
    {
        public string idorderservice { get; set; }
        public string hipotese { get; set; }
        public bool payment { get; set; }
        public string conclusao { get; set; }
        public string idpet { get; set; }
        public string idusertutor { get; set; }
        public string iduserrecipient { get; set; }
        public string iduserattendance { get; set; }
        public string status {  get; set; }
        public string idattendance { get; set; }
        public bool haveanamnese { get; set; }
        public string idform { get; set; }
    }
}
