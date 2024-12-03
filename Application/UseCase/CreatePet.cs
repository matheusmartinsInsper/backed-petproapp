using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Pet;

namespace app.Application.UseCase
{
    public class CreatePet
    {
        private IRepositoryPet _repopet;
        private IRepositoryUserTutor _repotutor;
        public CreatePet(IRepositoryPet repopet,IRepositoryUserTutor repotutor) 
        { 
            _repopet = repopet;
            _repotutor = repotutor;
        }
        public async Task execute(PetDTO petdto,string idusertutor)
        {
            User user = await _repotutor.get(idusertutor);
            if (user.id != null)
            {
                PetDTOInputUseCase dto = new PetDTOInputUseCase()
                {
                    idusertutor = idusertutor,
                    petname = petdto.petname,
                    castrated = petdto.castrated,
                    dateborn = petdto.dateborn,
                    sex = petdto.sex,
                    weight = petdto.weight,
                    race = petdto.race,
                    species = petdto.species,
                    ContraindicationDTO = petdto.ContraindicationDTO
                };
                if (dto.ContraindicationDTO != null)
                {
                    Pet petwithcontraindication = Pet.create(dto,dto.ContraindicationDTO);
                    await _repopet.save(petwithcontraindication);
                    return;
                }
                Pet pet = Pet.create(dto);
                await _repopet.save(pet);
                return;
            }
            throw new Exception("UserTutor not exist");
        }
    }
}
