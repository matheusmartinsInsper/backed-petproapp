using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.NetWorkCollaborator;
using app.Domain.DTO.Service;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace app.Infra.Repository
{
    public class RepositoryService : IRepositoryService
    {
        private IFactoryDbContext _factoryDbContext;
        private IDbContext _context;
        private string _connectString = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=petprodb";

        public RepositoryService(IFactoryDbContext factorycontext)
        {
            _factoryDbContext = factorycontext;
            _context = _factoryDbContext.psqlContext();
        }
        public async Task<Service> get(string id)
        {
            using(_context as IDisposable)
            {
                await _context.connect(_connectString);
                string commandGetService = "select codecategory,namecategory,idservice,iduser,title,description,price from service " +
                                           "where iduser in (select iduserprimary from network where idcollaborator = @iduser)";
                string commandBase = "SELECT codecategory,namecategory,idservice,iduser,title,description,price FROM \"service\" where idservice=@idservice";
                string commandsubcategory = "select subcategory.title,subcategory.idservice,subcategory.price,subcategory.idsubcategory " +
                                            "from service inner join subcategory on service.idservice = subcategory.idservice where service.idservice=@idservice";
                string commandvaccine = "SELECT vaccine.idservice,vaccine.idvaccine,vaccine.price,vaccine.codevaccine,vaccine.nameofvaccine FROM " +
                                        "service INNER JOIN vaccine ON service.idservice = vaccine.idservice WHERE service.idservice=@idservice";
                string commandAttendanceModel = "select attendancemodel.idservice,attendancemodel.model from attendancemodel inner join service on service.idservice = attendancemodel.idservice " +
                                                "where service.idservice=@idservice";
                Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idservice",id }
            };

                List<JsonObject> result = await _context.read(commandBase, parameters);
                List<JsonObject> resultsub = await _context.read(commandsubcategory, parameters);
                List<JsonObject> resultvaccine = await _context.read(commandvaccine, parameters);
                List<JsonObject> resultAttendance = await _context.read(commandAttendanceModel, parameters);

                List<ServiceDb> services = result.Select(jsonObject => JsonSerializer.Deserialize<ServiceDb>(jsonObject.ToJsonString()))
                                              .ToList();
                List<ServiceSubCategoryDb> subcategories = resultsub.Count() == 0 ? new List<ServiceSubCategoryDb>() : resultsub.Select(jsonObject => JsonSerializer.Deserialize<ServiceSubCategoryDb>(jsonObject.ToJsonString()))
                                              .ToList();
                List<VaccineDbDTO> vaccines = resultvaccine.Count() == 0 ? new List<VaccineDbDTO?>() : resultvaccine.Select(jsonObject => JsonSerializer.Deserialize<VaccineDbDTO>(jsonObject.ToJsonString()))
                                              .ToList();
                List<AttendanceModel> attendance = resultAttendance.Select(jsonObject => JsonSerializer.Deserialize<AttendanceModel>(jsonObject.ToJsonString()))
                                              .ToList();

                if (services.Count == 0)
                    throw new Exception("Serviço não registrado");
                Service servicerestore = Service.restore(services[0], subcategories, vaccines, attendance);
                _context.close();
                return servicerestore;
            }
        }
        public async Task<List<Service>> getbyuser(string id)
        {
            List<Service> servicesofuser = new List<Service>();
            await _context.connect(_connectString);
            string commandBase = "SELECT codecategory,namecategory,idservice,iduser,title,description,price FROM \"service\" where iduser = @iduser";
            string commandsubcategory = "select subcategory.title,subcategory.idservice,subcategory.price,subcategory.idsubcategory " +
                                        "from service inner join subcategory on service.idservice = subcategory.idservice where service.iduser = @iduser";
            string commandvaccine = "SELECT vaccine.idservice,vaccine.idvaccine,vaccine.price,vaccine.codevaccine,vaccine.nameofvaccine FROM " +
                                    "vaccine INNER JOIN service ON service.idservice = vaccine.idservice WHERE service.iduser = @iduser";
            string commandAttendanceModel = "select attendancemodel.idservice,attendancemodel.model from attendancemodel inner join service on service.idservice = attendancemodel.idservice " +
                                            "where service.iduser = @iduser ";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@iduser",id }
            };

            List<JsonObject> result = await _context.read(commandBase, parameters);
            List<JsonObject> resultsub = await _context.read(commandsubcategory, parameters);
            List<JsonObject> resultvaccine = await _context.read(commandvaccine, parameters);
            List<JsonObject> resultAttendance = await _context.read(commandAttendanceModel, parameters);

            List<ServiceDb> services = result.Select(jsonObject => JsonSerializer.Deserialize<ServiceDb>(jsonObject.ToJsonString()))
                                          .ToList();
            List<ServiceSubCategoryDb> subcategories = resultsub.Count()==0? new List<ServiceSubCategoryDb>(): resultsub.Select(jsonObject => JsonSerializer.Deserialize<ServiceSubCategoryDb>(jsonObject.ToJsonString()))
                                          .ToList();
            List<VaccineDbDTO> vaccines = resultvaccine.Count()==0? new List<VaccineDbDTO?>(): resultvaccine.Select(jsonObject => JsonSerializer.Deserialize<VaccineDbDTO>(jsonObject.ToJsonString()))
                                          .ToList();
            List<AttendanceModel> attendance =  resultAttendance.Select(jsonObject => JsonSerializer.Deserialize<AttendanceModel>(jsonObject.ToJsonString()))
                                          .ToList();

            foreach (ServiceDb service in services)
            {
                Service servicerestore = Service.restore(service, subcategories.FindAll(x => x.idservice == service.idservice), vaccines.FindAll(x => x.idservice == service.idservice),attendance.FindAll(x=>x.idservice==service.idservice));
                servicesofuser.Add(servicerestore);
            }
            _context.close();
            return servicesofuser;
        }
        public async Task<List<Service>> getbynetwork(string id)
        {
            List<Service> servicesofuser = new List<Service>();
            await _context.connect(_connectString);
            string commandGetService = "select idservice from service " +
                                       "where iduser in (select iduserprimary from network where idcollaborator = @iduser)";
            string commandBase = "select codecategory,namecategory,idservice,iduser,title,description,price from service " +
                                 "where iduser in (select iduserprimary from network where idcollaborator = @iduser)";
            string commandsubcategory = "select subcategory.title,subcategory.idservice,subcategory.price,subcategory.idsubcategory " +
                                        $"FROM subcategory where idservice in ({commandGetService})";
            string commandvaccine = "SELECT vaccine.idservice,vaccine.idvaccine,vaccine.price,vaccine.codevaccine,vaccine.nameofvaccine FROM " +
                                    $"vaccine WHERE vaccine.idservice in ({commandGetService})";
            string commandAttendance = "SELECT attendancemodel.idservice, attendancemodel.model from attendancemodel where " +
                                       $"attendancemodel.idservice in ({commandGetService})";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@iduser",id }
            };

            List<JsonObject> result = await _context.read(commandBase, parameters);
            List<JsonObject> resultsub = await _context.read(commandsubcategory, parameters);
            List<JsonObject> resultvaccine = await _context.read(commandvaccine, parameters);
            List<JsonObject> resultattendance = await _context.read(commandAttendance,parameters);

            List<ServiceDb> services = result.Select(jsonObject => JsonSerializer.Deserialize<ServiceDb>(jsonObject.ToJsonString()))
                                          .ToList();
            List<ServiceSubCategoryDb> subcategories = resultsub.Count() == 0 ? new List<ServiceSubCategoryDb>() : resultsub.Select(jsonObject => JsonSerializer.Deserialize<ServiceSubCategoryDb>(jsonObject.ToJsonString()))
                                          .ToList();
            List<VaccineDbDTO> vaccines = resultvaccine.Count() == 0 ? new List<VaccineDbDTO?>() : resultvaccine.Select(jsonObject => JsonSerializer.Deserialize<VaccineDbDTO>(jsonObject.ToJsonString()))
                                          .ToList();
            List<AttendanceModel> attendances =  resultattendance.Select(jsonObject => JsonSerializer.Deserialize<AttendanceModel>(jsonObject.ToJsonString()))
                                          .ToList();

            foreach (ServiceDb service in services)
            {
                Service servicerestore = Service.restore(service, subcategories.FindAll(x => x.idservice == service.idservice), vaccines.FindAll(x => x.idservice == service.idservice),attendances.FindAll(x => x.idservice == service.idservice));
                servicesofuser.Add(servicerestore);
            }
            _context.close();
            return servicesofuser;
        }
        public async Task save(Service service)
        {
            await _context.connect(_connectString);
            string commandBase = "INSERT INTO \"service\" (codecategory,namecategory,idservice,iduser,title,description,price) VALUES (@codecategory,@namecategory,@idservice,@iduser,@title,@description,@price)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@codecategory",service.codeCategory},
                {"@namecategory",service.nameCategory },
                {"@idservice",service.idService },
                {"@iduser",service.idUser },
                {"@title",service.titleService },
                {"@description",service.description },
                {"@price",service.price.HasValue ? (float)service.price.Value : DBNull.Value},
            };
            await _context.command(commandBase, parameters);

            string commandSubcategorys = "INSERT INTO \"subcategory\" (idsubcategory,idservice,title,price) VALUES (@idsubcategory,@idservice,@title,@price)";
            Dictionary<string, object> parametersubcategory = new Dictionary<string, object>();
            foreach (ServiceSubCategoryDb subcategory in service.subcategories)
            {
                parametersubcategory["@idservice"] = service.idService;
                parametersubcategory["@title"] = subcategory.title;
                parametersubcategory["@price"] = subcategory.price;
                parametersubcategory["@idsubcategory"] = subcategory.idsubcategory;
                await _context.command(commandSubcategorys, parametersubcategory);
            }

            if (service.codeCategory == "C04")
            {
                string commandRegisterVaccines = "INSERT INTO \"vaccine\" (idservice,idvaccine,codevaccine,nameofvaccine,price) VALUES (@idservice,@idvaccine,@codevaccine,@nameofvaccine,@price)";
                Dictionary<string, object> parametersVaccine = new Dictionary<string, object>()
                {
                   {"@idservice",service.idService},
                   {"@codevaccine", "01" },
                   {"@nameofvaccine", "01" },
                   {"@price", "01" }
                };
                foreach (VaccineDbDTO codevaccine in service.vaccines)
                {
                    parametersVaccine["@codevaccine"] = codevaccine.codevaccine;
                    parametersVaccine["@nameofvaccine"] = codevaccine.nameofvaccine;
                    parametersVaccine["@price"] = codevaccine.price;
                    parametersVaccine["@idvaccine"] = codevaccine.idvaccine;
                    await _context.command(commandRegisterVaccines, parametersVaccine);
                }
            }
            string commandAttendance = "INSERT INTO attendancemodel (idservice,model) VALUES (@idservice,@model)";
            Dictionary<string, object> parametersAttendance = new Dictionary<string, object>()
            {
                {"@idservice",service.idService }
            };
            foreach (string attendance in service.attendancemodels)
            {
                parametersAttendance["@model"] = attendance;
                await _context.command(commandAttendance, parametersAttendance);
            }
            _context.close();
        }

        public async Task update(Service service)
        {
            await _context.connect(_connectString);
            string commandBase = "UPDATE  \"service\" SET codecategory=@codecategory,namecategory=@namecategory,iduser=@iduser,title=@title,description=@description,price=@price where idservice=@idservice)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@codecategory",service.codeCategory},
                {"@namecategory",service.nameCategory },
                {"@idservice",service.idService },
                {"@iduser",service.idUser },
                {"@title",service.titleService },
                {"@description",service.description },
                {"@price",service.price },
            };
            await _context.command(commandBase, parameters);
            if (service.codeCategory == "C04")
            {
                Dictionary<string, object> parametersVaccine = new Dictionary<string, object>()
                {
                   {"@idservice",service.idService},
                   {"@codevaccine", "01" },
                   {"@namevaccine", "01" },
                   {"@price", "01" }
                };
                string commandDeleteVaccines = "DELETE FROM \"vaccine\" WHERE idservice = @idservice";
                await _context.command(commandDeleteVaccines,parametersVaccine);
                string commandRegisterVaccines = "INSERT INTO \"vaccine\" (idservice,idvaccine,codevaccine,namevaccine,price) VALUES (@idservice,@idvaccine,@codevaccine,@namevaccine,@price)";
                foreach (VaccineDbDTO codevaccine in service.vaccines)
                {
                    parametersVaccine["@codevaccine"] = codevaccine.codevaccine;
                    parametersVaccine["@namevaccine"] = codevaccine.nameofvaccine;
                    parametersVaccine["@price"] = codevaccine.price;
                    parametersVaccine["@idvaccine"] = codevaccine.idvaccine;
                    await _context.command(commandRegisterVaccines, parametersVaccine);
                }
            }
            _context.close();  
        }
    }
}
