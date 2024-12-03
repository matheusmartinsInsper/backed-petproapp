using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DomainService;
using app.Domain.DTO.OrderService;
using app.Domain.DTO.Prontuario;
using app.WebUI.DTO;

namespace app.Application.UseCase
{
    public class CreateOrderService
    {
        private IRepositoryOrderService _repository;
        private IRepositoryService _reposervice;
        private IRepositoryUserTutor _repouser;
        private IRepositoryPortfolioClient _repoport;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryPet _repopet;
        private IRepositoryProntuario _repoprontuario;
        public CreateOrderService(IRepositoryProntuario repoprontuario, IRepositoryPet repopet, IRepositoryUserClinic repoclinic, IRepositoryOrderService repository,IRepositoryService reposervice,IRepositoryUserTutor repouser,IRepositoryPortfolioClient repoport)
        {
            _repository = repository;
            _reposervice = reposervice;
            _repouser = repouser;
            _repoport = repoport;
            _repoclinic = repoclinic;
            _repopet = repopet;
            _repoprontuario = repoprontuario;
        }
        public async Task<OrderService> execute(OrderServiceInputController order,string idtutor)
        {
            Service service = await _reposervice.get(order.idservice);
            if (service.status == "Rascunho")
                throw new Exception("Serviço ainda não postado");
            User owner = await _repoclinic.getUserBase(service.idUser);
            User user = await _repouser.get(idtutor);
            List<Pet> pets = await _repopet.getByUser(user.id);
            if (pets.Find(pet => pet.IdPet == order.idpet) == null)
                throw new Exception("Pet não registrado ou não pertence a esse tutor");
            ClientPortfolio port = await _repoport.get(owner.id);
            if (!port.containsClient(idtutor))
            {
               ClientOnboarding onboarding = new ClientOnboarding(owner, user, port);
                await _repoport.save(onboarding.Integrate());
            }
            if (user.categoryCode != "Tutor")
                throw new Exception("Usuario sem permissão para criar uma solicitação");

            OrderServiceDTO os = new OrderServiceDTO()
            {
                iduserrecipient = service.idUser,
                idusertutor = idtutor,
                iduseraccepted = service.idUser,
                idservice = order.idservice,
                comments = order.comments,
                dateappointed = order.dateappointed,
                idvaccines = order.idvaccines,
                idsubcategories = order.idsubcategories,
                attendancemodel = order.attendancemodel,
                idpet = order.idpet,
                priority = order.priority,
            };
            OrderService orderservice = OrderService.create(os);
            await _repository.save(orderservice);
            Prontuario prontuarioOfPet = await _repoprontuario.getByIdPet(order.idpet);
            if (prontuarioOfPet == null)
            {
                ProntuarioDTO prontuariodto = new ProntuarioDTO()
                {
                    idowner = service.idUser,
                    idpet = order.idpet,
                    idtutor = user.id
                };
                Prontuario prontuario = Prontuario.create(prontuariodto);
                prontuario.addorderservice(orderservice.idorderservice);
                await _repoprontuario.save(prontuario);
                return orderservice;
            }
            prontuarioOfPet.addorderservice(orderservice.idorderservice);
            await _repoprontuario.update(prontuarioOfPet);
            return orderservice;
        }
    }
}
