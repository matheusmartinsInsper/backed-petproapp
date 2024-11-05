using app.Domain.DTO.Service;

namespace app.WebUI.DTO
{
    public class ServiceInputController
    {

        public string CodigoDaCategoria { get; set; }
        public string Titulo { get; set; }
        public string Descrição { get; set; }
        public float? Preço { get; set; }
        public List<VaccineDTO>? CodigoDeVacinas { get; set; }
        public List<ServiceSubCategory>? SubCategories { get; set; }
        public List<string> Atendimento { get; set; }
    }
}
