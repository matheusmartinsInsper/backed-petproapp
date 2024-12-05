using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Application.UseCase
{
    public class RemoveContraindication
    {
        private IRepositoryPet _repopet;
        private IRepositoryUserTutor _repotutor;
        public RemoveContraindication(IRepositoryPet repopet, IRepositoryUserTutor repotutor)
        {
            _repopet = repopet;
            _repotutor = repotutor;
        }

        public async Task execute(string iduser,string idcontraindication,string idpet)
        {
            Pet pet = await _repopet.get(idpet);
            if (iduser != pet.IdUserTutor)
                throw new Exception("Usuario sem permissão para alterar dados do pet");
            pet.removeContraindication(idcontraindication);
            await _repopet.update(pet);
            return;
        }
    }
}
