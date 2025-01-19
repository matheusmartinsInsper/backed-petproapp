using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.OrderService;
using app.Domain.DTO.Prontuario;

namespace app.Application.UseCase
{
    public class CreateOsManuallyWithTutor
    {
        private IRepositoryUserTutor _repotutor;
        private IRepositoryUserCollaborator _repocolaborator;
        private IRepositoryOrderService _repoorder;
        private IRepositoryPet _repopet;
        private IRepositoryService _reposervice;
        private IRepositoryProntuario _repoprontuario;
        public CreateOsManuallyWithTutor(IRepositoryProntuario repoprontuario, IRepositoryService reposervice, IRepositoryPet repopet, IRepositoryUserCollaborator repocolaborator, IRepositoryOrderService repoorder, IRepositoryUserTutor repotutor)
        {
            _repocolaborator = repocolaborator;
            _repoorder = repoorder;
            _repotutor = repotutor;
            _repopet = repopet;
            _reposervice = reposervice;
            _repoprontuario = repoprontuario;
        }
        public async Task execute(InputOSWithUserTutor order, string idowner)
        {
            User tutor = await _repotutor.getByEmail(order.emailusertutor);
            if (tutor.categoryCode == "Tutor")
            {
                Service service = await _reposervice.get(order.order.idservice);
                if (service.idUser != idowner)
                    throw new Exception("Usuario sem permissão para acessar esse recuros");
                List<Pet> pets = await _repopet.getByUser(tutor.id);
                if(pets.Find(pet=>pet.IdPet==order.order.idpet)==null)
                    throw new Exception("Pet não registrado ou não pertence a esse tutor");
                OrderServiceDTO os = new OrderServiceDTO()
                {
                    iduserrecipient = service.idUser,
                    idusertutor = tutor.id,
                    iduseraccepted = service.idUser,
                    idservice = order.order.idservice,
                    comments = order.order.comments,
                    dateappointed = order.order.dateappointed,
                    idvaccines = order.order.idvaccines,
                    idsubcategories = order.order.idsubcategories,
                    attendancemodel = order.order.attendancemodel,
                    idpet = order.order.idpet,
                    priority = order.order.priority,
                };
                OrderService myorder = OrderService.create(os);
                myorder.acceptOrder(idowner, idowner);
                await _repoorder.save(myorder);
                List<Prontuario> prontuariosOfPet = await _repoprontuario.getByIdOwner(idowner);
                Prontuario prontuariopet = prontuariosOfPet.Find((x) => x.idpet == myorder.idPet);
                if (prontuariopet == null)
                {
                    ProntuarioDTO prontuariodto = new ProntuarioDTO()
                    {
                        idowner = myorder.iduserrecipinet,
                        idpet = myorder.idPet,
                        idtutor = myorder.idUserTutor
                    };
                    Prontuario prontuario = Prontuario.create(prontuariodto);
                    prontuario.addorderservice(myorder.idorderservice);
                    await _repoprontuario.save(prontuario);
                    return;
                }
                prontuariopet.addorderservice(myorder.idorderservice);
                await _repoprontuario.update(prontuariopet);
                return;
            }
            throw new Exception("Não é possivel agendamentar um atendimento para esse tipo de usuario");
        }
    }
}
