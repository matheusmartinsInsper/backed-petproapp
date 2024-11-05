using app.Domain.Agregate.Entities;

namespace app.Application.IRepository
{
    public interface IRepositoryProntuario
    {
        Task save(Prontuario prontuario);
        Task update(Prontuario prontuario);
        Task<Prontuario> get(string idprontuario);
        Task<Prontuario> getByIdPet(string idpet);
        Task<List<Prontuario>> getByIdOwner(string idowner);   
    }
}
