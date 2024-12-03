using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DomainService;

namespace app.Application.UseCase
{
    public class GetAttendanceById
    {
        private IRepositoryAttendance _repoattendance;
        private IRepositoryFile _repofile;
        private IRepositoryOrderService _repoos;
        private IRepositoryPet _repopet;
        private IRepositoryUserTutor _repotutor;
        private IRepositoryService _reposervice;
        public GetAttendanceById(IRepositoryService reposervice, IRepositoryUserTutor repotutor, IRepositoryPet repopet, IRepositoryOrderService repoos, IRepositoryAttendance repoattendance, IRepositoryFile repofile)
        {
            _repoattendance = repoattendance;
            _repofile = repofile;
            _repoos = repoos;
            _repopet = repopet;
            _repotutor = repotutor;
            _reposervice = reposervice;
        }
        public async Task<AttendanceOutPut> execute(string idattendance, string iduserattendance)
        {
            Attendance att = await _repoattendance.get(idattendance);
            Pet pet = await _repopet.get(att.idpet);
            User tutor = await _repotutor.get(att.idusertutor);
            OrderService os = await _repoos.get(att.idos);
            Service service = await _reposervice.get(os.idService);
            AttendanceOutPut outputDTO = new AttendanceOutPut
            {
                pet = new PetOS(),
                tutor = new Tutor(),
                service = new Serviceoutput()
            };
            outputDTO.status = att.status;
            outputDTO.idattendance = att.idattendance;
            outputDTO.haveanamnese = att.haveanamnese;
            outputDTO.idform = att.idform;
            outputDTO.idos = att.idos;
            outputDTO.hipotese = att.hipoteses;
            outputDTO.conclusao = att.conclusao;
            outputDTO.pet.petname = pet.PetName;
            outputDTO.pet.age = pet.Age;
            outputDTO.pet.castrated = pet.Castrated;
            outputDTO.pet.race = pet.Race;
            outputDTO.pet.species = pet.Species;
            outputDTO.pet.weight = pet.Weight;
            outputDTO.pet.castrated = pet.Castrated;
            outputDTO.pet.sex = pet.Sex;
            outputDTO.pet.contraindications = pet.contraindications;
            outputDTO.tutor.email = tutor.email;
            outputDTO.tutor.name = tutor.name;
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
            return outputDTO;
        }
    }
}
