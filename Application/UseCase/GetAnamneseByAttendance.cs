using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Form;

namespace app.Application.UseCase
{
    public class GetAnamneseByAttendance
    {
        private IRepositoryForm _repoform;
        public GetAnamneseByAttendance(IRepositoryForm repoform)
        {
            _repoform = repoform;
        }
        public async Task<FormDbDTO> execute(string iduser, string idattendance, string idform)
        {
            Form form = await _repoform.get(idform);
            FormDbDTO db = new FormDbDTO();
            db.idform = form.idform;
            db.nameform = form.nameform;
            db.color = form.color;
            db.attributes = form.MapValuesToAttributes(idattendance);
            return db;
        }
    }
}
