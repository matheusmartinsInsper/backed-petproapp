using app.Domain.Agregate.Entities;
using app.Domain.DTO.User;

namespace app.Application.IRepository
{
    public interface IRepositoryNetWork
    {
        Task save(NetWorkCollaborators network);
        Task update(NetWorkCollaborators network);
        Task<NetWorkCollaborators> get(string id);
        Task<NetWorkCollaborators> getByUser(string idUser);
        Task<List<UserBaseDTO>> getUsers(string idUserPrimary);
    }
}
