using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Application.UseCase
{
    public class GetServiceByID
    {
        private IRepositoryService _reposervice;
        private IRepositoryNetWork _reponet;
        public GetServiceByID(IRepositoryService reposervice,IRepositoryNetWork reponet)
        {
            _reposervice = reposervice;
            _reponet = reponet;
        }
        public async Task<ServiceOutputDTO> execute(string iduser, string idservice)
        {
            Service service = await _reposervice.get(idservice);
            if(service.idUser != iduser)
            {
                NetWorkCollaborators collaborators = await _reponet.getByUser(service.idUser);
                if (!collaborators.conttainsCollaborator(iduser))
                    throw new Exception("Usuario sem permissao para visualizar esse serviço");
            }
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
            return outputDTO;
        }
    }
}
