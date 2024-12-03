using app.Domain.Agregate.Entities;

namespace app.Application.IRepository
{
    public interface IRepositoryForm
    {
        Task save(Form form);
        Task update(Form form);
        Task saveAnamnese(Form form,string idattendance);
        Task deleteAnamnese(string idattendance);    

        Task<Form> get(string id);
        Task<Form> getByIdAttendance(string id);
        Task<List<Form>> getByIdUser(string id);
    }
}
