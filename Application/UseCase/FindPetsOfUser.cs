using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Application.UseCase
{
    public class FindPetsOfUser
    {
        public IRepositoryPet _repopet;
        public FindPetsOfUser(IRepositoryPet repopet) 
        {
            _repopet = repopet;
        }
        public async Task<List<PetOutput>> execute(string idusertutor)
        {
            List<PetOutput> petsoutput = new List<PetOutput>();
            List<Pet> pets = await _repopet.getByUser(idusertutor);
            foreach(Pet pet in pets)
            {
                PetOutput petOutput = new PetOutput();
                petOutput.datebor = pet.DateBorn;
                petOutput.weight = pet.Weight;
                petOutput.age = pet.Age;
                petOutput.idpet = pet.IdPet;
                petOutput.castrated = pet.Castrated;
                petOutput.petname = pet.PetName;
                petOutput.species = pet.Species;
                petOutput.race = pet.Race;
                petOutput.sex = pet.Sex;
                petOutput.contraindication = pet.contraindications;
                petsoutput.Add(petOutput);
            }
            return petsoutput;
        }
    }
}
