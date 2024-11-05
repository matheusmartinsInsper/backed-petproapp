using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Service;
using System.Text.Json.Nodes;
using System.Text.Json;
using app.Domain.DTO.OrderService;
using app.Application.UseCase;

namespace app.Infra.Repository
{
    public class RepositoryOrderService : IRepositoryOrderService
    {
        private IFactoryDbContext _factoryDbContext;
        private IDbContext _context;
        private string _connectString = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=petprodb";
        public RepositoryOrderService(IFactoryDbContext factorycontext)
        {
            _factoryDbContext = factorycontext;
            _context = _factoryDbContext.psqlContext();
        }

        public async Task delete(OrderService order)
        {
            await _context.connect(_connectString);
            string commandBase = "DELETE FROM orderservice WHERE orderserivce = @idos";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idos",order.idorderservice}
            };
            await _context.command(commandBase, parameters);
            _context.close();
        }

        public async Task<OrderService> get(string id)
        {
            await _context.connect(_connectString);
            string commandBase = "SELECT idservice,idorderservice,idusertutor,iduserrecipient,idpet,iduseraccepted,iduserattendance,status,dateofsolicitation,dateappointed,attendencemodel,comments,wasaccepted,priority,idattendance from " +
                                 "orderservice where idorderservice = @idorderservice";
            string commandGetSubcategory = "SELECT subcategoryoforder.idsubcategory, subcategoryoforder.idorderservice from subcategoryoforder INNER JOIN orderservice " +
                                           "on subcategoryoforder.idorderservice = orderservice.idorderservice WHERE orderservice.idorderservice = @idorderservice";
            string commandGetVaccines = "SELECT vaccineofos.idvaccine, vaccineofos.idorderservice from vaccineofos INNER JOIN orderservice on vaccineofos.idorderservice = orderservice.idorderservice " +
                                        "WHERE orderservice.idorderservice = @idorderservice";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idorderservice",id}
            };

            List<JsonObject> result = await _context.read(commandBase, parameters);
            List<JsonObject> resultsub = await _context.read(commandGetSubcategory, parameters);
            List<JsonObject> resultvaccine = await _context.read(commandGetVaccines, parameters);

            foreach (var jsonObject in result)
            {
                if (jsonObject.TryGetPropertyValue("comments", out var comments))
                {
                    if (comments.ToString() == "{}")
                    {
                        jsonObject["comments"] = null;
                    }
                }
            }
            foreach (var jsonObject in result)
            {
                if (jsonObject.TryGetPropertyValue("idattendance", out var comments))
                {
                    if (comments.ToString() == "{}")
                    {
                        jsonObject["idattendance"] = null;
                    }
                }
            }

            List<OrderServiceDbDTO> orders = result.Select(jsonObject => JsonSerializer.Deserialize<OrderServiceDbDTO>(jsonObject.ToJsonString()))
                                          .ToList();
            List<SubcategoryOSDb> subcategories = resultsub.Count() == 0 ? new List<SubcategoryOSDb>() : resultsub.Select(jsonObject => JsonSerializer.Deserialize<SubcategoryOSDb>(jsonObject.ToJsonString()))
                                          .ToList();
            List<VaccinesOsDB> vaccines = resultvaccine.Count() == 0 ? new List<VaccinesOsDB?>() : resultvaccine.Select(jsonObject => JsonSerializer.Deserialize<VaccinesOsDB>(jsonObject.ToJsonString()))
                                          .ToList();
            _context.close();
            return OrderService.restore(orders[0], vaccines, subcategories);
               
        }

        public async Task<List<OrderService>> getByNetWork(string iduser,bool wasaccept)
        {
            List<OrderService> orderofuser = new List<OrderService>();
            await _context.connect(_connectString);
            string commandGetOs = "select idorderservice from orderservice " +
                                   "where iduserrecipient in (select iduserprimary from network where idcollaborator = @iduser)";
            string commandBase = "SELECT idservice,idorderservice,idusertutor,iduserrecipient,idpet,iduseraccepted,iduserattendance,status,dateofsolicitation,dateappointed,attendencemodel,comments,wasaccepted,tutorcancell,priority,idattendance from " +
                                 "orderservice where iduserrecipient in (select iduserprimary from network where idcollaborator = @iduser) and wasaccepted=@wasaccepted ";
            string commandGetSubcategory = "SELECT subcategoryoforder.idsubcategory, subcategoryoforder.idorderservice from subcategoryoforder INNER JOIN orderservice " +
                                           $"on subcategoryoforder.idorderservice = orderservice.idorderservice WHERE orderservice.idorderservice in ({commandGetOs})";
            string commandGetVaccines = "SELECT vaccineofos.idvaccine, vaccineofos.idorderservice from vaccineofos INNER JOIN orderservice on vaccineofos.idorderservice = orderservice.idorderservice " +
                                        $"WHERE orderservice.idorderservice in ({commandGetOs})";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@iduser",iduser},
                {"@wasaccepted",wasaccept }
            };

            List<JsonObject> result = await _context.read(commandBase, parameters);
            List<JsonObject> resultsub = await _context.read(commandGetSubcategory, parameters);
            List<JsonObject> resultvaccine = await _context.read(commandGetVaccines, parameters);

            foreach (var jsonObject in result)
            {
                if (jsonObject.TryGetPropertyValue("comments", out var comments))
                {
                    if (comments.ToString() == "{}")
                    {
                        jsonObject["comments"] = null;
                    }
                }
            }
            foreach (var jsonObject in result)
            {
                if (jsonObject.TryGetPropertyValue("idattendance", out var comments))
                {
                    if (comments.ToString() == "{}")
                    {
                        jsonObject["idattendance"] = null;
                    }
                }
            }

            List<OrderServiceDbDTO> orders = result.Select(jsonObject => JsonSerializer.Deserialize<OrderServiceDbDTO>(jsonObject.ToJsonString()))
                                          .ToList();
            List<SubcategoryOSDb> subcategories = resultsub.Count() == 0 ? new List<SubcategoryOSDb>() : resultsub.Select(jsonObject => JsonSerializer.Deserialize<SubcategoryOSDb>(jsonObject.ToJsonString()))
                                          .ToList();
            List<VaccinesOsDB> vaccines = resultvaccine.Count() == 0 ? new List<VaccinesOsDB?>() : resultvaccine.Select(jsonObject => JsonSerializer.Deserialize<VaccinesOsDB>(jsonObject.ToJsonString()))
                                          .ToList();
            foreach (OrderServiceDbDTO order in orders)
            {
                OrderService servicerestore = OrderService.restore(order, vaccines.FindAll(x => x.idorderservice == order.idorderservice), subcategories.FindAll(x => x.idorderservice == order.idorderservice));
                orderofuser.Add(servicerestore);
            }
            _context.close();
            return orderofuser;
        }

        public async Task<List<OrderService>> getByUser(string iduser,bool wasaccept)
        {
            List<OrderService> orderofuser = new List<OrderService>();
            await _context.connect(_connectString);
            string commandBase = "SELECT idservice,idorderservice,idusertutor,iduserrecipient,idpet,iduseraccepted,iduserattendance,status,dateofsolicitation,dateappointed,attendencemodel,comments,wasaccepted,tutorcancell,priority,idattendance from " +
                                 "orderservice where iduserrecipient = @iduser and wasaccepted=@wasaccepted ";
            string commandGetSubcategory = "SELECT subcategoryoforder.idsubcategory, subcategoryoforder.idorderservice from subcategoryoforder INNER JOIN orderservice " +
                                           "on subcategoryoforder.idorderservice = orderservice.idorderservice WHERE orderservice.iduserrecipient = @iduser";
            string commandGetVaccines = "SELECT vaccineofos.idvaccine, vaccineofos.idorderservice from vaccineofos INNER JOIN orderservice on vaccineofos.idorderservice = orderservice.idorderservice " +
                                        "WHERE orderservice.iduserrecipient = @iduser";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@iduser",iduser},
                {"@wasaccepted",wasaccept }
            };

            List<JsonObject> result = await _context.read(commandBase, parameters);
            List<JsonObject> resultsub = await _context.read(commandGetSubcategory, parameters);
            List<JsonObject> resultvaccine = await _context.read(commandGetVaccines, parameters);

            foreach (var jsonObject in result)
            {
                if (jsonObject.TryGetPropertyValue("comments", out var comments))
                {
                    if (comments.ToString() == "{}")
                    {
                        jsonObject["comments"] = null;
                    }
                }
            }
            foreach (var jsonObject in result)
            {
                if (jsonObject.TryGetPropertyValue("idattendance", out var comments))
                {
                    if (comments.ToString() == "{}")
                    {
                        jsonObject["idattendance"] = null;
                    }
                }
            }

            List<OrderServiceDbDTO> orders = result.Select(jsonObject => JsonSerializer.Deserialize<OrderServiceDbDTO>(jsonObject.ToJsonString()))
                                          .ToList();
            List<SubcategoryOSDb> subcategories = resultsub.Count() == 0 ? new List<SubcategoryOSDb>() : resultsub.Select(jsonObject => JsonSerializer.Deserialize<SubcategoryOSDb>(jsonObject.ToJsonString()))
                                          .ToList();
            List<VaccinesOsDB> vaccines = resultvaccine.Count() == 0 ? new List<VaccinesOsDB?>() : resultvaccine.Select(jsonObject => JsonSerializer.Deserialize<VaccinesOsDB>(jsonObject.ToJsonString()))
                                          .ToList();
            foreach (OrderServiceDbDTO order in orders)
            {
                OrderService servicerestore = OrderService.restore(order, vaccines.FindAll(x => x.idorderservice == order.idorderservice), subcategories.FindAll(x => x.idorderservice == order.idorderservice));
                orderofuser.Add(servicerestore);
            }
            _context.close();
            return orderofuser;
        }

        public async Task<List<OrderService>> getForGenderByNW(string iduser, bool wasaccept)
        {
            throw new NotImplementedException();
        }

        public async Task save(OrderService order)
        {
            await _context.connect(_connectString);
            string commandBase = "INSERT INTO \"orderservice\" (idservice,idorderservice,idusertutor,iduserrecipient,idpet,iduseraccepted,iduserattendance,status,dateofsolicitation,dateappointed,attendencemodel,wasaccepted,tutorcancell,comments,priority,idattendance) " +
                                 "VALUES (@idservice,@idorderservice,@idusertutor,@iduserrecipient,@idpet,@iduseraccepted,@iduserattendance,@status,@datesolicitation,@dateappointed,@attendancemodel,@wasaccepted,@tutorcancell,@comments,@priority,@idattendance)";
            string commandinsertidvaccines = "INSERT INTO vaccineofos (idorderservice,idvaccine) VALUES (@idorderservice,@idvaccine)";
            string commandinsertsubcategorys = "INSERT INTO subcategoryoforder (idsubcategory,idorderservice) VALUES (@idsubcategory,@idorderservice)";
            Dictionary<string, object> parametersidos = new Dictionary<string, object>()
            {
                { "@idorderservice",order.idorderservice}
            };
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idservice",order.idService},
                {"@idorderservice",order.idorderservice },
                {"@idusertutor",order.idUserTutor },
                {"@iduserrecipient",order.iduserrecipinet },
                {"@iduserattendance",order.iduserattendance},
                {"@idpet",order.idPet },
                {"@iduseraccepted",order.iduseraccepted },
                {"@status",order.status },
                {"@datesolicitation",order.dateofsolicitation },
                {"@dateappointed",order.dateappointed },
                {"@attendancemodel", order.attendance },
                {"@wasaccepted",order.wasaccepted },
                {"@tutorcancell",order.tutorcancelled },
                {"@comments",order.comments },
                {"@priority",order.priority },
                {"@idattendance", order.idattedance }
            };
            await _context.command(commandBase, parameters);
            foreach(string id in order.idvaccines)
            {
                parametersidos["@idvaccine"] = id;
                await _context.command(commandinsertidvaccines, parametersidos);
            }
            foreach (string id in order.idsubservices)
            {
                parametersidos["@idsubcategory"] = id;
                await _context.command(commandinsertsubcategorys, parametersidos);
            }
            _context.close();
        }

        public async Task update(OrderService order)
        {
            await _context.connect(_connectString);
            string commandBase = "UPDATE \"orderservice\" SET idservice=@idservice,idorderservice=@idorderservice,idusertutor=@idusertutor,iduserrecipient=@iduserrecipient," +
                "idpet=@idpet,iduseraccepted=@iduseraccepted,iduserattendance=@iduserattendance,status=@status,dateofsolicitation=@datesolicitation,dateappointed=@dateappointed,attendencemodel=@attendancemodel,wasaccepted=@wasaccepted,tutorcancell=@tutorcancell,idattendance=@idattendance " +
                                 " WHERE idorderservice = @idorderservice";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idservice",order.idService},
                {"@idorderservice",order.idorderservice },
                {"@idusertutor",order.idUserTutor },
                {"@iduserrecipient",order.iduserrecipinet },
                {"@iduserattendance",order.iduserattendance},
                {"@idpet",order.idPet },
                {"@iduseraccepted",order.iduseraccepted },
                {"@status",order.status },
                {"@datesolicitation",order.dateofsolicitation },
                {"@dateappointed",order.dateappointed },
                {"@attendancemodel", order.attendance },
                {"@wasaccepted",order.wasaccepted },
                {"@tutorcancell",order.tutorcancelled },
                {"@idattendance", order.idattedance }
            };
            await _context.command(commandBase, parameters);
            _context.close();
        }
    }
}
