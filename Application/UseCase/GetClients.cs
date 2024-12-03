using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Application.UseCase
{
    public class GetClients
    {
        private IRepositoryUserTutor _repotutor;
        private IRepositoryPet _repopet;
        private IRepositoryPortfolioClient _repoport;
        public GetClients(IRepositoryUserTutor repotutor, IRepositoryPet repopet, IRepositoryPortfolioClient repoport)
        {
            _repotutor = repotutor;
            _repopet = repopet;
            _repoport = repoport;
        }
        public async Task<List<OutPutClientDTO>> execute(string idowner)
        {
            List<OutPutClientDTO> myclients = new List<OutPutClientDTO>();
           
            ClientPortfolio port = await _repoport.get(idowner);
            List<Client> clients = port.clients.Where(x=>x.idclient!=idowner).ToList();
            foreach(Client client in clients)
            {
                OutPutClientDTO dto = new OutPutClientDTO();
                User tutor = await _repotutor.get(client.idclient);
                dto.dateadd = client.dateadd;
                dto.name = tutor.name;
                dto.email = tutor.email;
                dto.phone = tutor.Fone!=null?$"+{tutor.Fone.countrycode}{tutor.Fone.areacode}{tutor.Fone.phone}":null;
                dto.dateborn = tutor.dateborn;
                List<Pet> pets = await _repopet.getByUser(client.idclient);
                List<PetOutput> petsoutput = new List<PetOutput>();
                foreach (Pet pet in pets)
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
                    petsoutput.Add(petOutput);
                }
                dto.pets = petsoutput;
                myclients.Add(dto);
            }
            return myclients;
        }
    }
}
