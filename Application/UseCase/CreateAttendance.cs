using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DomainService;
using app.Domain.DTO.Attendance;
using app.WebUI.DTO;

namespace app.Application.UseCase
{
    public class CreateAttendance
    {
        private IRepositoryAttendance _repoattendance;
        private IRepositoryFile _repofile;
        private IRepositoryOrderService _repoos;
        private IRepositoryPet _repopet;
        private IRepositoryUserTutor _repotutor;
        private IRepositoryProntuario _repoprontuario;
        public CreateAttendance(IRepositoryProntuario repoprontuario, IRepositoryUserTutor repotutor, IRepositoryPet repopet, IRepositoryOrderService repoos, IRepositoryAttendance repoattendance,IRepositoryFile repofile)
        {
            _repoattendance = repoattendance;
            _repofile = repofile;
            _repoos = repoos;
            _repopet = repopet;
            _repotutor = repotutor;
            _repoprontuario = repoprontuario;
        }
        public async Task<OuputAttendanceCreate> execute(string idos,string iduserattendance)
        {
            OrderService os = await _repoos.get(idos);
            if (os.status == "Confirmado")
            {
                if (iduserattendance == os.iduserattendance || iduserattendance == os.iduserrecipinet)
                {
                    Pet pet = await _repopet.get(os.idPet);
                    User tutor = await _repotutor.get(pet.IdUserTutor);
                    Prontuario prontuario = await _repoprontuario.getByIdPet(pet.IdPet);
                    AttendanceDTO attendancedto = new AttendanceDTO()
                    {
                        iduserattendance = iduserattendance,
                        idpet = pet.IdPet,
                        idowner = os.iduserrecipinet,
                        idtutor = tutor.id,
                        Hipoteses = "",
                        Conclusao = "",
                        waspaid = os.waspaid,
                        IdOrderService = os.idorderservice
                    };
                    Attendance attendance = Attendance.create(attendancedto);
                    if(prontuario != null)
                    {
                        prontuario.addattendance(attendance.idattendance);
                        await _repoprontuario.update(prontuario);
                    }
                    await _repoattendance.save(attendance);
                    os.StartOrder(attendance.idattendance);
                    await _repoos.update(os);
                    OuputAttendanceCreate output = new OuputAttendanceCreate()
                    {
                        idattendance = attendance.idattendance,
                        idorderservice = os.idorderservice
                    };
                    return output;
                }
                throw new Exception("Usuario sem permissão para iniciar esse atendimento");
            }
            throw new Exception("Atendimento ja iniciado para essa solicitação");
        }

    }
    public class OuputAttendanceCreate
    {
        public string idattendance { get; set; }
        public string idorderservice { get; set; }
    }
}
