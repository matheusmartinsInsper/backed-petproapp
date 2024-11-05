using app.Application.DTO;

namespace app.WebUI.DTO
{
    public class InputConcludeAttendance
    {
        public string hipotese { get; set; }
        public string conclusao { get; set; }
        public string idorderservice { get; set; }
        public string idattendance { get; set; }
    }
}
