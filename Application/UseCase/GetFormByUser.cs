using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Form;

namespace app.Application.UseCase
{
    public class GetFormByUser
    {
        private IRepositoryForm _repoform;
        public GetFormByUser(IRepositoryForm repoform)
        {
            _repoform = repoform;
        }
        public async Task<List<FormDbDTO>> execute(string iduser)
        {
            List<Form> forms =  await _repoform.getByIdUser(iduser);
            List<FormDbDTO> formsoutput = new List<FormDbDTO>();
            foreach(Form form in forms)
            {
                FormDbDTO db = new FormDbDTO();
                db.idform = form.idform;
                db.nameform = form.nameform;
                db.attributes = form.attributes;
                db.color = form.color;
                formsoutput.Add(db);
            }
            return formsoutput;
        }
    }
}
