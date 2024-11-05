using app.Domain.Agregate.Entities;
using System.Runtime.CompilerServices;

namespace app.Application.IRepository
{
    public interface IRepositoryUserTutor
    {
        Task save(User user);
        Task update(User user);
        Task delete(User user);
        Task<User> get(string id);
        Task<User> getByEmail(string email);
    }
}
