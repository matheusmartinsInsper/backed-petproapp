using app.Application.DTO;
using app.Application.IAuth;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DomainService;
using app.Domain.DTO.OrderService;
using app.Domain.DTO.Pet;
using app.Domain.DTO.Prontuario;
using app.Domain.DTO.User;

namespace app.Application.UseCase
{
    public class CreateOSManually
    {
        private IRepositoryOrderService _repositoryos;
        private IRepositoryService _reposervice;
        private IRepositoryUserTutor _repouser;
        private IRepositoryUserClinic _repoclinic;
        private ITokenService _tokenservice;
        private IRepositoryPet _repopet;
        private IRepositoryPortfolioClient _repoport;
        private IRepositoryProntuario _repoprontuario;
        public CreateOSManually(IRepositoryUserClinic repoclinic, IRepositoryProntuario repoprontuario, IRepositoryPortfolioClient repoport,IRepositoryPet repopet, IRepositoryOrderService repository, IRepositoryService reposervice, IRepositoryUserTutor repouser, ITokenService tokenservice)
        {
            _repositoryos = repository;
            _reposervice = reposervice;
            _repouser = repouser;
            _tokenservice = tokenservice;
            _repopet = repopet;
            _repoport = repoport;
            _repoprontuario = repoprontuario;
            _repoclinic = repoclinic;
        }
        public async Task<OrderService> execute(InputOSWithoutUserTutor orderinput,string iduserowner)
        {
            User user = await _repoclinic.getUserBase(iduserowner);
            if (user.categoryCode == "Tutor")
                throw new Exception("Usuario sem acesso a esse recurso");
            User tutor = User.create(orderinput.user);
            PetDTOInputUseCase dto = new PetDTOInputUseCase()
            {
                idusertutor = tutor.id,
                petname = orderinput.pet.petname,
                castrated = orderinput.pet.castrated,
                dateborn = orderinput.pet.dateborn,
                sex = orderinput.pet.sex,
                weight = orderinput.pet.weight,
                race = orderinput.pet.race,
                species = orderinput.pet.species,
            };
            Pet pet = Pet.create(dto);
            Service service = await _reposervice.get(orderinput.order.idservice);
            OrderServiceDTO os = new OrderServiceDTO()
            {
                iduserrecipient = service.idUser,
                idusertutor = tutor.id,
                iduseraccepted = service.idUser,
                idservice = orderinput.order.idservice,
                comments = orderinput.order.comments,
                dateappointed = orderinput.order.dateappointed,
                idvaccines = orderinput.order.idvaccines,
                idsubcategories = orderinput.order.idsubcategories,
                attendancemodel = orderinput.order.attendancemodel,
                idpet = pet.IdPet,
                priority = orderinput.order.priority,
            };
            OrderService myorder = OrderService.create(os);
            ClientPortfolio port = await _repoport.get(iduserowner);
            try
            {
                string token = _tokenservice.GenerateToken(tutor);
                await _repouser.save(tutor);
                ClientOnboarding onboarding = new ClientOnboarding(user, tutor, port);
                await _repoport.save(onboarding.Integrate());
                await _repopet.save(pet);
                myorder.acceptOrder(iduserowner, iduserowner);
                await _repositoryos.save(myorder);
                Prontuario prontuarioOfPet = await _repoprontuario.getByIdPet(myorder.idPet);
                if (prontuarioOfPet == null)
                {
                    ProntuarioDTO prontuariodto = new ProntuarioDTO()
                    {
                        idowner = user.id,
                        idpet = pet.IdPet,
                        idtutor = tutor.id
                    };
                    Prontuario prontuario = Prontuario.create(prontuariodto);
                    prontuario.addorderservice(myorder.idorderservice);
                    await _repoprontuario.save(prontuario);
                    return myorder;
                }
                prontuarioOfPet.addorderservice(myorder.idorderservice);
                await _repoprontuario.update(prontuarioOfPet);
                return myorder;
            }
            catch (Exception ex)
            {
                port.remove(tutor.id);
                await _repoport.save(port);
                await _repouser.delete(tutor);
                await _repopet.delete(pet);
                await _repositoryos.delete(myorder);
                throw new Exception(ex.Message);
            }
           
        }
    }
}
