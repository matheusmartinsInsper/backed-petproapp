using app.Domain.Agregate.Entities;

namespace app.Application.IRepository
{
    public interface IRepositoryOrderService
    {
        Task save(OrderService order);
        Task update(OrderService order);
        Task<OrderService> get(string id);
        Task delete (OrderService order);
        Task<List<OrderService>> getByUser(string iduser,bool wasaccept);
        Task<List<OrderService>> getByNetWork(string iduser,bool wasaccept);
        Task<List<OrderService>> getForGenderByNW(string iduser, bool wasaccept);
    }
}
