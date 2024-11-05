using app.Domain.Agregate.Entities;

namespace app.Application.IRepository
{
    public interface IRepositoryFile
    {
        Task save(List<FileAttachment> files);
        Task update(Attendance attendance);
        Task<Attendance> get(string id);
    }
}
