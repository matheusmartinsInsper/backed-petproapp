namespace app.WebUI.DTO
{
    public class PetInputController
    {
        public DateTime DatadeNascimento { get; set; }
        public string Nome {  get; set; }
        public float Peso { get; set; }
        public string Raça { get; set; }
        public string Especie { get; set; }
        public string Sexo { get; set; }
        public bool castrado { get; set; }
    }
}
