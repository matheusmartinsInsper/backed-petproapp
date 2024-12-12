using app.Domain.Agregate.Entities;

namespace app.Application.IRepository
{
    public interface IRepositoryItem
    {
        Task save(Item item);
        Task udpate(Item item);
        Task<Item> getbyid(string id);
        Task<List<Item>> getbyiduser(string id);
    }
}
