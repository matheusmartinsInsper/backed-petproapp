using app.Domain.Agregate.Entities;

namespace app.Application.IRepository
{
    public interface IRepositoryStock
    {
        Task save(Stock stock);
        Task<List<Stock>> getByIditem(string iditem);
        Task<Stock> getbyid(string idstock);
        Task<List<Stock>> getbyuser(string iduser); 
        Task update(Stock stock);
    }
}
