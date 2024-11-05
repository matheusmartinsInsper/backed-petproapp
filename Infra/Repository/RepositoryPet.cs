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
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idpet",id}
            };
            List<JsonObject> result = await _context.read(commandBase, parameters);
            List<PetDbDTO> dtos = result.Select(jsonObject => JsonSerializer.Deserialize<PetDbDTO>(jsonObject.ToJsonString()))
                                           .ToList();
            Pet pet = Pet.restore(dtos[0]);
            _context.close();
            return pet;
        }
        public async Task<List<Pet>> getByUser(string iduser)
        {
            List<Pet> pets = new List<Pet>();   
            await _context.connect(_connectString);
            string commandBase = "SELECT idpet,idusertutor,dateborn,petname,weight,race,species,sex,castrated from pet " +
                "where idusertutor = @idusertutor";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@idusertutor",iduser}
            };
            List<JsonObject> result = await _context.read(commandBase, parameters);
            List<PetDbDTO> dtos = result.Select(jsonObject => JsonSerializer.Deserialize<PetDbDTO>(jsonObject.ToJsonString()))
                                           .ToList();
            foreach(PetDbDTO dto in dtos)
            {
                Pet pet = Pet.restore(dto);
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

        public Task update(Pet pet)
        {
            throw new NotImplementedException();
        }
    }
}
