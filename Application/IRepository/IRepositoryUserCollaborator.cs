using app.Domain.Agregate.Entities;

namespace app.Application.IRepository
{
    public interface IRepositoryUserCollaborator
    {
        Task save(User user);
        Task update(User user);
        Task<User> get(string id);
        Task<User> getByEmail(string email);
        Task<List<User>> getcollaborators(string iduser);
    }
}
