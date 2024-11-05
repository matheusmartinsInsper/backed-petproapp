using app.Domain.Agregate.Entities;

namespace app.Application.IRepository
{
    public interface IRepositoryService
    {
        Task save(Service service);
        Task update(Service service);
        Task<Service> get(string id);
        Task<List<Service>> getbyuser(string id);
        Task<List<Service>> getbynetwork(string id);
    }
}
