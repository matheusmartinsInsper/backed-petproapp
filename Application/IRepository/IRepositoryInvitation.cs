using app.Domain.Agregate.Entities;

namespace app.Application.IRepository
{
    public interface IRepositoryInvitation
    {
        Task save(InviteCollaborator invitation);
        Task update(InviteCollaborator invitation);
        Task<InviteCollaborator> get(string id);
        Task<InviteCollaborator> getByUser(string userId);
        Task<List<InviteCollaborator>> getAll(string email);
    }
}
