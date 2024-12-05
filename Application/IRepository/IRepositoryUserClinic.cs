using app.Domain.Agregate.Entities;

namespace app.Application.IRepository
{
    public interface IRepositoryUserClinic
    {
        Task save(User user);
        Task update(User user);
        Task<User> get(string id);
        Task<User> getByEmail(string email);
        Task<User> getUserBase(string id);
        Task<User> getUserBaseByEmail(string email);    
    }
}
