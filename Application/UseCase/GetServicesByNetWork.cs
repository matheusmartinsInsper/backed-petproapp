using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Application.UseCase
{
    public class GetServicesByNetWork
    {
        private IRepositoryService _reposervice;
        private IRepositoryNetWork _reponet;
        public GetServicesByNetWork(IRepositoryService reposervice,IRepositoryNetWork reponet)
        {
            _reposervice = reposervice;
            _reponet = reponet;
        }
        public async Task<List<ServiceOutputDTO>> execute(string iduser, bool posted)
        {
            List<Service> services = await _reposervice.getbynetwork(iduser);
            if(posted == true)
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
