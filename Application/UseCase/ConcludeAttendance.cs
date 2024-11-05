using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Attendance;

namespace app.Application.UseCase
{
    public class ConcludeAttendance
    {
        private IRepositoryAttendance _repoattendance;
        private IRepositoryFile _repofile;
        private IRepositoryOrderService _repoos;
        private IRepositoryPet _repopet;
        private IRepositoryUserTutor _repotutor;
        private IRepositoryForm _repoform;
        public ConcludeAttendance(IRepositoryForm repoform, IRepositoryUserTutor repotutor, IRepositoryPet repopet, IRepositoryOrderService repoos, IRepositoryAttendance repoattendance, IRepositoryFile repofile)
        {
            _repoattendance = repoattendance;
            _repofile = repofile;
            _repoos = repoos;
            _repopet = repopet;
            _repotutor = repotutor;
            _repoform = repoform;
        }
        public async Task execute(InputAttendanceConclude attendance)
        {
            OrderService os = await _repoos.get(attendance.idorderservice);
            if(os.iduserattendance==attendance.iduserattendance||os.iduserrecipinet==attendance.iduserattendance)
            {
                Attendance attendaceofos = await _repoattendance.get(os.idattedance);
                if (attendaceofos == null)
                    throw new Exception("Atendimento não criado");
                attendaceofos.conclusao = attendance.conclusao;
                attendaceofos.hipoteses = attendance.hipotese;
                attendaceofos.Conclude();
                os.ConcluirOrder();

                List<FileAttachment> files = new List<FileAttachment>();
                foreach (IFormFile file in attendance.files)
                {
                   string fileName = file.FileName;
                   using (var memoryStream = new MemoryStream())
                   {
                      await file.CopyToAsync(memoryStream);
                      byte[] fileBytes = memoryStream.ToArray();
                      string base64File = Convert.ToBase64String(fileBytes);
                      FileAttachment attachment = new FileAttachment
                      {
                         FileName = fileName,
                         FileBase64 = base64File
                      };
                      attendaceofos.addFile(attachment);
                   }
                 }
                await _repoattendance.update(attendaceofos);
                await _repofile.save(attendaceofos.anexos);
                await _repoos.update(os);
                return;
            }
            throw new Exception("Usuario sem permissão para concluir esse atendimento");
        }
    }
}
