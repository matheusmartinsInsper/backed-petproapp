using app.Domain.Agregate.Entities;

namespace app.Application.IRepository
{
    public interface IRepositoryAttendance
    {

        Task save(Attendance attendance);
        Task update(Attendance attendance);
        Task<Attendance> get(string id);
    }
}
