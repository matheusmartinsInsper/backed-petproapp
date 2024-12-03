using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Application.UseCase
{
    public class GetServicesByUser
    {
        private IRepositoryService _reposervice;
        public GetServicesByUser(IRepositoryService reposervice)
        {
            _reposervice = reposervice;
        }
        public async Task<List<ServiceOutputDTO>> execute(string iduser, bool posted)
        {
            List<Service> services = await _reposervice.getbyuser(iduser);
            if (posted == true)
                services = services.Where((service) => service.status == "Postado").ToList();
            List<ServiceOutputDTO> outputs = new List<ServiceOutputDTO>();
            foreach (Service service in services)
            {
                ServiceOutputDTO outputDTO = new ServiceOutputDTO();
                outputDTO.NomeDoServiço = service.nameCategory;
                outputDTO.IdDoServiço = service.idService;
                outputDTO.CodigoDoServiço = service.codeCategory;
                outputDTO.Titulo = service.titleService;
                outputDTO.Preço = service.price;
                outputDTO.Descrição = service.description;
                outputDTO.Vacinas = service.vaccines;
                outputDTO.Subcategorias = service.subcategories;
                outputDTO.Atendimento = service.attendancemodels;
                outputDTO.Status = service.status;
                outputs.Add(outputDTO);
            }
            return outputs;
        }
    }
}
