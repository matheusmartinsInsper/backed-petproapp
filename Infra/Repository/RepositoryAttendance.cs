using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Attendance;
using app.Domain.DTO.Prontuario;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace app.Infra.Repository
{
    public class RepositoryAttendance : IRepositoryAttendance
    {
        private IFactoryDbContext _factoryDbContext;
        private IDbContext _context;
        private string _connectString = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=petprodb";
        public RepositoryAttendance(IFactoryDbContext factorycontext)
        {
            _factoryDbContext = factorycontext;
            _context = _factoryDbContext.psqlContext();
        }
        public async Task<Attendance> get(string id)
        {
            using (_context as IDisposable)
            {
                await _context.connect(_connectString);
                List<Attendance> atts = new List<Attendance>();
                string command = "SELECT idattendance,idorderservice,iduserattendance,idusertutor,iduserrecipient,idpet,hipotese,conclusao,payment,status,haveanamnese,idform FROM attendance WHERE " +
                    "idattendance = @idattendance";
             
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                  {"@idattendance",id}
                };
                List<JsonObject> result = await _context.read(command, parameters);
                List<AttendanceDbDTO> attendances = result.Select(jsonObject => JsonSerializer.Deserialize<AttendanceDbDTO>(jsonObject.ToJsonString()))
                                          .ToList();
                foreach(AttendanceDbDTO att in attendances)
                {
                    Attendance myatt = Attendance.restore(att);
                    atts.Add(myatt);
                }
                _context.close();
                return atts[0];
            }
        }

        public async Task save(Attendance attendance)
        {
            using (_context as IDisposable)
            {
                await _context.connect(_connectString);
                string command = "INSERT INTO \"attendance\" (idattendance,idorderservice,iduserattendance,idusertutor,iduserrecipient,idpet,hipotese,conclusao,payment,status,haveanamnese,idform) VALUES " +
               "(@idattendance,@idorderservice,@iduserattendance,@idusertutor,@idrecipient,@idpet,@hipotese,@conclusao,@payment,@status,@haveanamnese,@idform)";
                Dictionary<string, object> parameters = new Dictionary<string, object>()
             {
                {"@idattendance",attendance.idattendance},
                {"@idorderservice", attendance.idos },
                {"@iduserattendance",attendance.iduserattendance},
                {"@idusertutor",attendance.idusertutor},
                {"@idrecipient",attendance.iduserowner},
                {"@idpet",attendance.idpet},
                {"@hipotese",attendance.hipoteses},
                {"@conclusao",attendance.conclusao},
                {"@payment",attendance.waspaid},
                {"@status",attendance.status},
                {"@haveanamnese",attendance.haveanamnese},
                {"@idform",attendance.idform},
             };
                await _context.command(command, parameters);
                _context.close();
            }
        }

        public async Task update(Attendance attendance)
        {
            using (_context as IDisposable)
            {
                await _context.connect(_connectString);
                string command = "UPDATE attendance SET hipotese=@hipotese,conclusao=@conclusao,payment=@payment,status=@status,haveanamnese=@haveanamnese,idform=@idform where idattendance=@idattendance";
        ;
                Dictionary<string, object> parameters = new Dictionary<string, object>()
             {
                {"@idattendance",attendance.idattendance},
                {"@hipotese",attendance.hipoteses},
                {"@conclusao",attendance.conclusao},
                {"@payment",attendance.waspaid},
                {"@status",attendance.status},
                {"@haveanamnese",attendance.haveanamnese},
                {"@idform",attendance.idform},
             };
                await _context.command(command, parameters);
                _context.close();
            }
        }
    }
}
