using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DomainService;
using System.ComponentModel;

namespace app.Application.UseCase
{
    public class GetProntuarioByUser
    {
        private IRepositoryOrderService _repository;
        private IRepositoryService _reposervice;
        private IRepositoryUserTutor _repouser;
        private IRepositoryPortfolioClient _repoport;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryPet _repopet;
        private IRepositoryProntuario _repoprontuario;
        public GetProntuarioByUser(IRepositoryProntuario repoprontuario, IRepositoryPet repopet, IRepositoryUserClinic repoclinic, IRepositoryOrderService repository, IRepositoryService reposervice, IRepositoryUserTutor repouser, IRepositoryPortfolioClient repoport)
        {
            _repository = repository;
            _reposervice = reposervice;
            _repouser = repouser;
            _repoport = repoport;
            _repoclinic = repoclinic;
            _repopet = repopet;
            _repoprontuario = repoprontuario;
        }
        public async Task<List<OutputProntuarioDTO>> execute(string idowner)
        {
            List<OutputProntuarioDTO> output = new List<OutputProntuarioDTO>();
            List<Prontuario> prontuarios = await _repoprontuario.getByIdOwner(idowner);
            foreach(Prontuario prontuario in prontuarios)
            {
                OutputProntuarioDTO dto = new OutputProntuarioDTO()
                {
                    pet = new PetOS(),
                    tutor = new Tutor()
                };
                List<OutputOsFromProntuario> orders = new List<OutputOsFromProntuario>();
                Pet pet = await _repopet.get(prontuario.idpet);
                User tutor = await _repouser.get(prontuario.idtutor);
                foreach(string idsos in prontuario.idsorderservices)
                {
                    OrderService order = await _repository.get(idsos);
                    Service service = await _reposervice.get(order.idService);
                    User recipiente = await _repoclinic.getUserBase(order.iduserrecipinet);
                    User profissional = await _repoclinic.getUserBase(order.iduserattendance);
                    OutputOsFromProntuario osofprontuario = new OutputOsFromProntuario();
                    osofprontuario.nameuserowner = recipiente.name;
                    osofprontuario.nameprofissional = profissional.name;
                    osofprontuario.priority = order.priority;
                    osofprontuario.vaccines = service.selectvaccines(order.idvaccines);
                    osofprontuario.subcategories = service.subcategoriesOfOs(order.idsubservices);
                    osofprontuario.waaccepted = order.wasaccepted;
                    osofprontuario.tutorcancelled = order.tutorcancelled;
                    osofprontuario.attendancemodel = order.attendance;
                    osofprontuario.idservice = order.idService;
                    osofprontuario.comments = order.comments;
                    osofprontuario.categoryname = service.nameCategory;
                    osofprontuario.title = service.titleService;
                    osofprontuario.dateappointed = order.dateappointed;
                    osofprontuario.datesolicitation = order.dateofsolicitation;
                    osofprontuario.status = order.status;
                    osofprontuario.idorderservice = order.idorderservice;
                    SumPriceOS sum = new SumPriceOS(service, osofprontuario.subcategories, osofprontuario.vaccines);
                    osofprontuario.price = sum.sum();
                    orders.Add(osofprontuario);
                }
                dto.idprontuario = prontuario.idprontuario;
                dto.idsattendance = prontuario.idsattendance;
                dto.datecreate = prontuario.createDate;
                dto.orders = orders;
                dto.pet.petname = pet.PetName;
                dto.pet.age = pet.Age;
                dto.pet.castrated = pet.Castrated;
                dto.pet.race = pet.Race;
                dto.pet.species = pet.Species;
                dto.pet.weight = pet.Weight;
                dto.pet.castrated = pet.Castrated;
                dto.pet.sex = pet.Sex;
                dto.tutor.email = tutor.email;
                dto.tutor.name = tutor.name;
                output.Add(dto);
            }
            return output;
        }
    }
}
