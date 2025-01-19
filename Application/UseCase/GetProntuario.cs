using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DomainService;

namespace app.Application.UseCase
{
    public class GetProntuario
    {
        private IRepositoryOrderService _repository;
        private IRepositoryService _reposervice;
        private IRepositoryUserTutor _repouser;
        private IRepositoryPortfolioClient _repoport;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryPet _repopet;
        private IRepositoryProntuario _repoprontuario;
        private IRepositoryAttendance _repoattendance;
        public GetProntuario(IRepositoryAttendance repoattendance, IRepositoryProntuario repoprontuario, IRepositoryPet repopet, IRepositoryUserClinic repoclinic, IRepositoryOrderService repository, IRepositoryService reposervice, IRepositoryUserTutor repouser, IRepositoryPortfolioClient repoport)
        {
            _repository = repository;
            _reposervice = reposervice;
            _repouser = repouser;
            _repoport = repoport;
            _repoclinic = repoclinic;
            _repopet = repopet;
            _repoprontuario = repoprontuario;
            _repoattendance = repoattendance;
        }
        public async Task<OutputProntuarioDTO> execute(string idprontuario)
        {
            Prontuario prontuario = await _repoprontuario.get(idprontuario);
                OutputProntuarioDTO dto = new OutputProntuarioDTO()
                {
                    pet = new PetOS(),
                    tutor = new Tutor()
                };
                List<OutputOsFromProntuario> orders = new List<OutputOsFromProntuario>();
                List<AttendanceOutPutOfProntuario> attendances = new List<AttendanceOutPutOfProntuario>();
                Pet pet = await _repopet.get(prontuario.idpet);
                User tutor = await _repouser.get(prontuario.idtutor);
                foreach (string idsos in prontuario.idsorderservices)
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
                foreach (string idattendance in prontuario.idsattendance)
                {
                    Attendance att = await _repoattendance.get(idattendance);
                OrderService os = await _repository.get(att.idos);
                Service service = await _reposervice.get(os.idService);
                AttendanceOutPutOfProntuario outputDTO = new AttendanceOutPutOfProntuario
                {
                    service = new Serviceoutput()
                };
                outputDTO.status = att.status;
                outputDTO.idattendance = att.idattendance;
                outputDTO.haveanamnese = att.haveanamnese;
                outputDTO.idform = att.idform;
                outputDTO.idos = att.idos;
                outputDTO.hipotese = att.hipoteses;
                outputDTO.conclusao = att.conclusao;
                outputDTO.service.comments = os.comments;
                outputDTO.service.title = service.titleService;
                outputDTO.service.category = service.nameCategory;
                outputDTO.service.priority = os.priority;
                outputDTO.service.vacinas = service.selectvaccines(os.idvaccines);
                outputDTO.service.subcategorias = service.subcategoriesOfOs(os.idsubservices);
                outputDTO.service.atendimento = os.attendance;
                outputDTO.service.waspaid = att.waspaid ? "Sim" : "Não";
                outputDTO.service.description = service.description;
                outputDTO.service.dateapontted = os.dateappointed;
                outputDTO.service.datesolicitation = os.dateofsolicitation;
                SumPriceOS sum = new SumPriceOS(service, outputDTO.service.subcategorias, outputDTO.service.vacinas);
                outputDTO.service.totalprice = sum.sum();
                attendances.Add(outputDTO);
            }
                dto.idprontuario = prontuario.idprontuario;
                dto.attendances = attendances;
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
                dto.pet.contraindications = pet.contraindications;
                dto.tutor.email = tutor.email;
                dto.tutor.name = tutor.name;
                dto.tutor.phone = tutor.Fone != null ? $"+{tutor.Fone.countrycode}{tutor.Fone.areacode}{tutor.Fone.phone}" : null;
            return dto;
        }
    }
}
