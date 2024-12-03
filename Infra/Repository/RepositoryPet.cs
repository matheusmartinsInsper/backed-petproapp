using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.User;
using System.Text.Json.Nodes;
using System.Text.Json;
using app.Domain.DTO.Pet;

namespace app.Infra.Repository
{
    public class RepositoryPet : IRepositoryPet
    {
        private IFactoryDbContext _factoryDbContext;
        private IDbContext _context;
        private string _connectString = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=petprodb";
        public RepositoryPet(IFactoryDbContext factorycontext)
        {
            _factoryDbContext = factorycontext;
            _context = _factoryDbContext.psqlContext();
        }

        public async Task delete(Pet pet)
        {
            await _context.connect(_connectString);
            string commandBase = "DELETE FROM pet WHERE idpet = @idpet";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idpet",pet.IdPet}
            };
            await _context.command(commandBase, parameters);
            _context.close();
        }

        public async Task<Pet> get(string id)
        {
            await _context.connect(_connectString);
            string commandBase = "SELECT idpet,idusertutor,dateborn,petname,weight,race,species,sex,castrated from pet " +
                "where idpet = @idpet";
            string commandGetContraIndication = "SELECT idpet,idcontraindication,description,categoria from contraindication where idpet=@idpet";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idpet",id}
            };
            List<JsonObject> result = await _context.read(commandBase, parameters);
            List<PetDbDTO> dtos = result.Select(jsonObject => JsonSerializer.Deserialize<PetDbDTO>(jsonObject.ToJsonString()))
                                           .ToList();
            List<JsonObject> contraindications = await _context.read(commandGetContraIndication, parameters);
            List<ContraindicationDTODb> contraindicationsdtos = contraindications.Select(jsonObject => JsonSerializer.Deserialize<ContraindicationDTODb>(jsonObject.ToJsonString()))
                                           .ToList();
            Pet pet = Pet.restore(dtos[0],contraindicationsdtos);
            _context.close();
            return pet;
        }
        public async Task<List<Pet>> getByUser(string iduser)
        {
            List<Pet> pets = new List<Pet>();   
            await _context.connect(_connectString);
            string commandBase = "SELECT idpet,idusertutor,dateborn,petname,weight,race,species,sex,castrated from pet " +
                "where idusertutor = @idusertutor";
            string commandGetContraIndication = "SELECT idpet,idcontraindication,description,categoria from contraindication where idpet=@idpet";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idusertutor",iduser}
            };
            List<JsonObject> result = await _context.read(commandBase, parameters);
            List<PetDbDTO> dtos = result.Select(jsonObject => JsonSerializer.Deserialize<PetDbDTO>(jsonObject.ToJsonString()))
                                           .ToList();
            foreach(PetDbDTO dto in dtos)
            {
                parameters["@idpet"] = dto.idpet;
                List<JsonObject> contraindications = await _context.read(commandGetContraIndication, parameters);
                List<ContraindicationDTODb> contraindicationsdtos = contraindications.Select(jsonObject => JsonSerializer.Deserialize<ContraindicationDTODb>(jsonObject.ToJsonString()))
                                               .ToList();
                Pet pet = Pet.restore(dto,contraindicationsdtos);
                pets.Add(pet);
            }
            _context.close();
            return pets;
        }

        public async Task save(Pet pet)
        {
            await _context.connect(_connectString);
            string commandBase = "INSERT INTO \"pet\" (idpet,idusertutor,dateborn,petname,weight,race,species,sex,castrated) " +
                "VALUES (@idpet,@idusertutor,@dateborn,@petname,@weight,@race,@species,@sex,@castrated)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idpet",pet.IdPet},
                {"@idusertutor",pet.IdUserTutor },
                {"@dateborn",pet.DateBorn },
                {"@petname",pet.PetName.ToString() },
                {"@weight",pet.Weight },
                {"@race",pet.Race },
                {"@species",pet.Species },
                {"@sex",pet.Sex },
                {"@castrated",pet.Castrated },
            };
            await _context.command(commandBase, parameters);
            _context.close();
        }

        public async Task update(Pet pet)
        {
            await _context.connect(_connectString);
            string deletContraindications = "delete from contraindication where idpet=@idpet";
            string insertContraindications = "insert into contraindication (idpet,idcontraindication,categoria,description) " +
                "values (@idpet,@idcontraindication,@categoria,@description)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idpet",pet.IdPet}
            };
            await _context.command(deletContraindications, parameters);
            foreach(Contraindication contraindication in pet.ContraIndications)
            {
                parameters["@idcontraindication"] = contraindication.idcontraindication;
                parameters["idpet"] = contraindication._idPet;
                parameters["@description"] = contraindication.Description;
                parameters["@categoria"] = contraindication.Type;
                await _context.command(insertContraindications, parameters);
            }
            _context.close();
        }
    }
}
