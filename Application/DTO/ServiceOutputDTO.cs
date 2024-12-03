using app.Domain.DTO.Service;

namespace app.Application.DTO
{
    public class ServiceOutputDTO
    {
        public string CodigoDoServiço { get; set; }
        public object NomeDoServiço { get; set; }
        public string Titulo { get; set; }
        public string Descrição { get; set; }
        public string IdDoServiço { get; set; }
        public string Status {  get; set; }
        public float? Preço { get; set; }
        public List<VaccineDbDTO> Vacinas { get; set; }
        public List<ServiceSubCategoryDb> Subcategorias { get; set; }
        public List<string> Atendimento { get; set; }
    }
}
