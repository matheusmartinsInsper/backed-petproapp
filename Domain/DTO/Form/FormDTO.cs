namespace app.Domain.DTO.Form
{
    public class FormDTO
    {
        public string nameform { get; set; }
        public string color {  get; set; }
        public List<Fields> fields { get; set; }
    }
    public class Fields
    {
        public string label { get; set; }
        public string type { get; set; }
        public List<string> options { get; set; }
    }
}
