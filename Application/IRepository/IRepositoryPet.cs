using app.Domain.Agregate.Entities;

namespace app.Application.IRepository
{
    public interface IRepositoryPet
    {
        Task save(Pet pet);
        Task update(Pet pet);
        Task delete(Pet pet);
        Task<Pet> get(string id);
        Task<List<Pet>> getByUser(string iduser);
    }
}
