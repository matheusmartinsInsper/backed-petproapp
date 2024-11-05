using app.Domain.Agregate.Entities;

namespace app.Domain.DTO.Form
{
    public class FormDbDTO
    {
        public string idform { get; set; }
        public string nameform { get; set; }
        public string color {  get; set; }
        public List<attribute> attributes { get; set; }
    }
}
