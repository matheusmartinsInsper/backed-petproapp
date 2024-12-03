using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Fone;

namespace app.Application.UseCase
{
    public class AddPhone
    {
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryUserCollaborator _repocollaborator;
        private IRepositoryUserTutor _repotutor;
        public AddPhone(IRepositoryUserClinic repoclinic, IRepositoryUserCollaborator repocollaborator, IRepositoryUserTutor repotutor)
        {
            _repoclinic = repoclinic;
            _repotutor = repotutor;
            _repocollaborator = repocollaborator;
        }
        public async Task execute(string iduser, PhoneDTOInput phone)
        {
            Dictionary<string, object> repos = new Dictionary<string, object>();
            repos.Add("Tutor", _repotutor);
            repos.Add("Collaborator", _repocollaborator);
            repos.Add("Clinic", _repoclinic);
            User userbase = await _repoclinic.getUserBase(iduser);
            if (!repos.TryGetValue(userbase.categoryCode, out var repository)||userbase==null)
            {
                throw new Exception($"Categoria de usuário inválida: {userbase.categoryCode}");
            }
            dynamic repo = repository;
            User user = await repo.get(iduser);
            user.addFone(phone);
            await repo.update(user);
        }
    }
}
