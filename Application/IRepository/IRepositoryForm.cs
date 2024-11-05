using app.Domain.Agregate.Entities;

namespace app.Application.IRepository
{
    public interface IRepositoryForm
    {
        Task save(Form form);
        Task update(Form form);
        Task<Form> get(string id);
        Task<Form> getByIdAttendance(string id);
        Task<List<Form>> getByIdUser(string id);
    }
}
