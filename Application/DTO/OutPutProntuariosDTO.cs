namespace app.Application.DTO
{
    public class OutPutProntuariosDTO
    {
        public Tutor tutor { get; set; }
        public PetOS pet { get; set; }
        public string idprontuario { get; set; }
        public DateTime datecreate { get; set; }
        public List<string> idsorder { get; set; }
        public List<string> idsattendance { get; set; }
    }
}
