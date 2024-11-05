namespace app.Application.DTO
{
    public class InputAttendanceConclude
    {
       public List<IFormFile> files {  get; set; }
       public string hipotese { get; set; }
       public string conclusao { get; set; }
       public string iduserattendance { get; set; }
       public string idorderservice { get; set; }
       public string idattendance { get; set; }
       //public AnamneseValueDTO anamnese { get; set; }
    }
}
