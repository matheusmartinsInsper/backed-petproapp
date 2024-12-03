using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DomainService;

namespace app.Application.UseCase
{
    public class SaveAnamnese
    {
        private IRepositoryAttendance _repoattendance;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryForm _repoform;
        public SaveAnamnese(IRepositoryAttendance repoattendance, IRepositoryForm repoform, IRepositoryUserClinic repoclinic)
        {
            _repoattendance = repoattendance;
            _repoform = repoform;
            _repoclinic = repoclinic;
        }
        public async Task execute(string iduser, List<valueInstanceFormCreate> values)
        {
            if(values.Count == 0) return;
            Attendance att = await _repoattendance.get(values[0].idattendance);
            if (att != null & att.iduserattendance == iduser || att.iduserowner == iduser)
            {
                if (att.status == "Concluido")
                    throw new Exception("Atendimento já concluido");
                Form form = await _repoform.get(values[0].formid);
                form.createInstanceForm(values);
                await _repoform.saveAnamnese(form,att.idattendance);
                att.addAnamnese(form.idform);
                await _repoattendance.update(att);
                return;
            }
        }
    }
}
