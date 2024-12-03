using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DomainService;

namespace app.Application.UseCase
{
    public class RemoveAnamnese
    {
        private IRepositoryAttendance _repoattendance;
        private IRepositoryForm _repoform;
        private IRepositoryUserClinic _repoclinic;
        public RemoveAnamnese(IRepositoryUserClinic repoclinic, IRepositoryForm repoform, IRepositoryAttendance repoattendance)
        {
            _repoattendance = repoattendance;
            _repoform = repoform;
            _repoclinic = repoclinic;
        }
        public async Task execute(string idattendance, string idowner)
        {
            Attendance att = await _repoattendance.get(idattendance);
            if(att.iduserattendance == idowner || att.iduserowner == idowner)
            {
                if (att.status == "Concluido")
                    throw new Exception("Não é permitido remover anamnese de um atendimento concluido");
                await _repoform.deleteAnamnese(idattendance);
                att.removeAnamnese();
                await _repoattendance.update(att);
                return;
            }
            throw new Exception("Usuario sem permissão");
        }
    }
}
