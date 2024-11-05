using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Application.UseCase
{
    public class GetUsersFromNet
    {
        private IRepositoryNetWork _reponet;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryUserCollaborator _repocollaborator;
        public GetUsersFromNet(IRepositoryUserClinic repoclinic, IRepositoryNetWork reponet, IRepositoryUserCollaborator repocollaborator)
        {
            _reponet = reponet;
            _repoclinic = repoclinic;
            _repocollaborator = repocollaborator;
        }
        public async Task<List<UsersNetWork>> execute(string iduser)
        {
            List<UsersNetWork> collaboratorDTOs = new List<UsersNetWork>();
            User user = await _repoclinic.get(iduser);
            if (user.categoryCode != "Clinic")
                throw new Exception("Usuario sem permissão para acessar esse recurso");
            NetWorkCollaborators collaborators = await _reponet.getByUser(iduser);
            foreach(string id in collaborators.idcollaborator)
            {
                if (id == collaborators.iduserprimary)
                {
                    User userprimary = await _repoclinic.get(id);
                    UsersNetWork usersNetWork = new UsersNetWork()
                    {
                        name = userprimary.name,
                        email = userprimary.email,
                        id = userprimary.id,
                    };
                    collaboratorDTOs.Add(usersNetWork);
                }
                else
                {
                    User usercollab = await _repocollaborator.get(id);
                    UsersNetWork usersNetWork = new UsersNetWork()
                    {
                        name = usercollab.name,
                        email = usercollab.email,
                        id = usercollab.id,
                    };
                    collaboratorDTOs.Add(usersNetWork);
                }
            }
            return collaboratorDTOs;
        }
        public class UsersNetWork
        {
            public string name { get; set; }
            public string email { get; set; }
            public string id {  get; set; }
        }
    }
}
