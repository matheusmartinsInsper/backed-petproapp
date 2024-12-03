using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Fone;
using app.Domain.DTO.Pet;

namespace app.Application.UseCase
{
    public class AddContraindication
    {
        private IRepositoryPet _repopet;
        private IRepositoryUserTutor _repotutor;
        public AddContraindication(IRepositoryPet repopet, IRepositoryUserTutor repotutor)
        {
            _repopet = repopet;
            _repotutor = repotutor;
        }
        public async Task execute(string idpet,string iduser, ContraindicationDTO contraindication)
        {
           User tutor = await _repotutor.get(iduser);
           Pet pet = await _repopet.get(idpet);
           pet.addContraindication(contraindication);
           await _repopet.update(pet);
        }
    }
}
