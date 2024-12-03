using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Prontuario;
using app.Domain.DTO.Service;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace app.Infra.Repository
{
    public class RepositoryProntuario : IRepositoryProntuario
    {
        private IFactoryDbContext _factoryDbContext;
        private IDbContext _context;
        private string _connectString = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=petprodb";

        public RepositoryProntuario(IFactoryDbContext factorycontext)
        {
            _factoryDbContext = factorycontext;
            _context = _factoryDbContext.psqlContext();
        }
        public async Task<Prontuario> get(string idprontuario)
        {
            await _context.connect(_connectString);
            List<Prontuario> prontuarioofpet = new List<Prontuario>();
            string commandBase = "SELECT idprontuario,idowner,idpet,idtutor,datecreate FROM \"prontuario\" WHERE idprontuario = @idprontuario";
            string commandgetidos = "SELECT idos,idprontuario FROM \"idosprontuario\" WHERE idprontuario = @idprontuario";
            string commandgetidatt = "SELECT idattendance,idprontuario FROM \"idattendanceprontuario\" WHERE idprontuario = @idprontuario";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idprontuario",idprontuario}
            };
            Dictionary<string, object> parameteridprontuario = new Dictionary<string, object>();
            List<JsonObject> result = await _context.read(commandBase, parameters);

            List<ProntuarioDbDTO> prontuarios = result.Select(jsonObject => JsonSerializer.Deserialize<ProntuarioDbDTO>(jsonObject.ToJsonString()))
                                          .ToList();
            if (prontuarios.Count == 0)
            {
                _context.close();
                return null;
            }
            foreach (ProntuarioDbDTO prontuario in prontuarios)
            {
                parameteridprontuario["@idprontuario"] = prontuario.idprontuario;
                List<JsonObject> os = await _context.read(commandgetidos, parameteridprontuario);
                List<IdOsDb> idos = os.Select(jsonObject => JsonSerializer.Deserialize<IdOsDb>(jsonObject.ToJsonString()))
                                              .ToList().FindAll(x => x.idprontuario == prontuario.idprontuario);

                List<JsonObject> attendance = await _context.read(commandgetidatt, parameteridprontuario);
                List<IdAttendanceDB> idattendance = attendance.Select(jsonObject => JsonSerializer.Deserialize<IdAttendanceDB>(jsonObject.ToJsonString()))
                                              .ToList().FindAll(x => x.idprontuario == prontuario.idprontuario);

                Prontuario prontuariorestore = Prontuario.restore(prontuario, idos, idattendance);
                prontuarioofpet.Add(prontuariorestore);
            }
            _context.close();
            return prontuarioofpet[0];
        }

        public async Task<List<Prontuario>> getByIdOwner(string idowner)
        {
            await _context.connect(_connectString);
            List<Prontuario> prontuarioofpet = new List<Prontuario>();
            string commandBase = "SELECT idprontuario,idowner,idpet,idtutor,datecreate FROM \"prontuario\" WHERE idowner = @idowner";
            string commandgetidos = "SELECT idos,idprontuario FROM idosprontuario WHERE idprontuario = @idprontuario";
            string commandgetidatt = "SELECT idattendance,idprontuario FROM idattendanceprontuario WHERE idprontuario = @idprontuario";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idowner",idowner}
            };
            Dictionary<string, object> parameteridprontuario = new Dictionary<string, object>();
            List<JsonObject> result = await _context.read(commandBase, parameters);

            List<ProntuarioDbDTO> prontuarios = result.Select(jsonObject => JsonSerializer.Deserialize<ProntuarioDbDTO>(jsonObject.ToJsonString()))
                                          .ToList();
            if (prontuarios.Count == 0)
            {
                _context.close();
                return null;
            }
            foreach (ProntuarioDbDTO prontuario in prontuarios)
            {
                parameteridprontuario["@idprontuario"] = prontuario.idprontuario;
                List<JsonObject> os = await _context.read(commandgetidos, parameteridprontuario);

                List<IdOsDb> idos = os.Select(jsonObject => JsonSerializer.Deserialize<IdOsDb>(jsonObject.ToJsonString()))
                                              .ToList().FindAll(x => x.idprontuario == prontuario.idprontuario);
                List<JsonObject> attendance = await _context.read(commandgetidatt, parameteridprontuario);

                List<IdAttendanceDB> idattendance = attendance.Select(jsonObject => JsonSerializer.Deserialize<IdAttendanceDB>(jsonObject.ToJsonString()))
                                              .ToList().FindAll(x => x.idprontuario == prontuario.idprontuario);
                Prontuario prontuariorestore = Prontuario.restore(prontuario, idos, idattendance);
                prontuarioofpet.Add(prontuariorestore);
            }
            _context.close();
            return prontuarioofpet;
        }

        public async Task<Prontuario> getByIdPet(string idpet)
        {
            await _context.connect(_connectString);
            List<Prontuario> prontuarioofpet = new List<Prontuario>();
            string commandBase = "SELECT idprontuario,idowner,idpet,idtutor,datecreate FROM \"prontuario\" WHERE idpet = @idpet";
            string commandgetidos = "SELECT idos,idprontuario FROM \"idosprontuario\" WHERE idprontuario = @idprontuario";
            string commandgetidatt = "SELECT idattendance,idprontuario FROM \"idattendanceprontuario\" WHERE idprontuario = @idprontuario";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idpet",idpet}
            };
            Dictionary<string, object> parameteridprontuario = new Dictionary<string, object>();
            List<JsonObject> result = await _context.read(commandBase, parameters);

            List<ProntuarioDbDTO> prontuarios = result.Select(jsonObject => JsonSerializer.Deserialize<ProntuarioDbDTO>(jsonObject.ToJsonString()))
                                          .ToList();
            if (prontuarios.Count == 0)
            {
                _context.close();
                return null;
            }
            foreach(ProntuarioDbDTO prontuario in prontuarios)
            {
                parameteridprontuario["@idprontuario"] = prontuario.idprontuario;
                List<JsonObject> os = await _context.read(commandgetidos, parameteridprontuario);
                List<IdOsDb> idos = os.Select(jsonObject => JsonSerializer.Deserialize<IdOsDb>(jsonObject.ToJsonString()))
                                              .ToList().FindAll(x => x.idprontuario == prontuario.idprontuario);

                List<JsonObject> attendance = await _context.read(commandgetidatt, parameteridprontuario);
                List<IdAttendanceDB> idattendance = attendance.Select(jsonObject => JsonSerializer.Deserialize<IdAttendanceDB>(jsonObject.ToJsonString()))
                                              .ToList().FindAll(x => x.idprontuario == prontuario.idprontuario);

                Prontuario prontuariorestore = Prontuario.restore(prontuario,idos,idattendance);
                prontuarioofpet.Add(prontuariorestore);
            }
            _context.close();
            return prontuarioofpet[0];
        }

        public async Task save(Prontuario prontuario)
        {
            await _context.connect(_connectString);
            string commandBase = "INSERT INTO \"prontuario\" (idprontuario,idowner,idpet,idtutor,datecreate) VALUES (@idprontuario,@idowner,@idpet,@idtutor,@datecreate)";
            string commandAttendance = "INSERT INTO idattendanceprontuario (idprontuario,idattendance) VALUES (@idprontuario,@idattendance)";
            string commandOs = "INSERT INTO idosprontuario (idprontuario,idos) VALUES (@idprontuario,@idos)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idprontuario",prontuario.idprontuario},
                {"@idowner",prontuario.idowner },
                {"@idpet",prontuario.idpet },
                {"@idtutor",prontuario.idtutor },
                {"@datecreate",prontuario.createDate }
            };
            Dictionary<string, object> parameteros = new Dictionary<string, object>()
            {
                {"@idprontuario",prontuario.idprontuario}
            };
            Dictionary<string, object> parameterattendance = new Dictionary<string, object>()
            {
                {"@idprontuario",prontuario.idprontuario}
            };
            await _context.command(commandBase, parameters);
            foreach(string idos in prontuario.idsorderservices)
            {
                parameteros["@idos"] = idos;
                await _context.command(commandOs, parameteros);
            }
            foreach (string idattendance in prontuario.idsattendance)
            {
                parameterattendance["@idattendance"] = idattendance;
                await _context.command(commandAttendance, parameterattendance);
            }
            _context.close();
        }

        public async Task update(Prontuario prontuario)
        {
            await _context.connect(_connectString);
            Dictionary<string, object> parameteros = new Dictionary<string, object>()
            {
                {"@idprontuario",prontuario.idprontuario}
            };
            Dictionary<string, object> parameterattendance = new Dictionary<string, object>()
            {
                {"@idprontuario",prontuario.idprontuario}
            };
            string commanddeleteidos = "delete from idosprontuario where idprontuario=@idprontuario";
            string commanddeleteidatt = "delete from idattendanceprontuario where idprontuario=@idprontuario";
            await _context.command(commanddeleteidos, parameteros);
            await _context.command(commanddeleteidatt, parameteros);
            string commandAttendance = "INSERT INTO idattendanceprontuario (idprontuario,idattendance) VALUES (@idprontuario,@idattendance)";
            string commandOs = "INSERT INTO idosprontuario (idprontuario,idos) VALUES (@idprontuario,@idos)";
            foreach (string idos in prontuario.idsorderservices)
            {
                parameteros["@idos"] = idos;
                await _context.command(commandOs, parameteros);
            }
            foreach (string idattendance in prontuario.idsattendance)
            {
                parameterattendance["@idattendance"] = idattendance;
                await _context.command(commandAttendance, parameterattendance);
            }
            _context.close();
        }
    }
}
