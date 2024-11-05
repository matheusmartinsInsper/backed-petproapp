using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Form;
using app.Domain.DTO.Pet;
using app.WebUI.DTO;

namespace app.Application.UseCase
{
    public class CreateForm
    {
        private IRepositoryForm _repoform;
        public CreateForm(IRepositoryForm repoform)
        {
            _repoform = repoform;
        }
        public async Task execute(FormDTO formdto,string iduser)
        {
            Form form = Form.create(formdto,iduser);
            await _repoform.save(form);
        }
    }
}
