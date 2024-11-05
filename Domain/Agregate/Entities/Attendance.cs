using app.Domain.DTO.Attendance;
using app.Domain.DTO.Payment;
using System.ComponentModel;

namespace app.Domain.Agregate.Entities
{
    public class Attendance
    {
        private string _idAttendance;
        private string _idanamnese;
        private string _idprescription;
        private string _idOrderService;
        private string _idUserAttendance;
        private string _iduserowner;
        private string _idusertutor;
        private string _idpet;
        private bool _waspaid;
        private string _status;
        public List<FileAttachment> anexos = new List<FileAttachment>();
        public string hipoteses { get; set; }
        public string conclusao { get; set; }
        public string idattendance { get { return _idAttendance; } }
        public string idanamnese { get { return _idanamnese; } }
        public string idos { get { return _idOrderService; } }
        public string iduserattendance { get { return _idUserAttendance; } }
        public string iduserowner { get { return _iduserowner; } }
        public string idusertutor { get { return _idusertutor; } }
        public string idpet { get { return _idpet; } }
        public string status { get { return _status; } }
        public bool waspaid { get { return _waspaid; } }

        private Attendance() { }  
        public static Attendance create(AttendanceDTO attendanceDTO)
        {
            Attendance attendance = new Attendance();
            attendance._idAttendance = Guid.NewGuid().ToString("N");
            attendance._idOrderService = attendanceDTO.IdOrderService;
            attendance._idUserAttendance = attendanceDTO.iduserattendance;
            attendance._iduserowner = attendanceDTO.idowner;
            attendance._idpet = attendanceDTO.idpet;
            attendance._idusertutor = attendanceDTO.idtutor;
            attendance.hipoteses = attendanceDTO.Hipoteses;
            attendance._waspaid = attendanceDTO.waspaid;
            attendance.conclusao = attendanceDTO.Conclusao;
            attendance._status = "Andamento";
            return attendance;
        }
        public static Attendance restore(AttendanceDbDTO att)
        {
            Attendance attendance = new Attendance();
            attendance._idOrderService = att.idorderservice;
            attendance._idAttendance = att.idattendance;
            attendance._iduserowner = att.iduserrecipient;
            attendance._idUserAttendance = att.iduserattendance;
            attendance._idAttendance = att.idattendance;
            attendance._idusertutor = att.idusertutor;
            attendance._waspaid = att.payment;
            attendance._status = att.status;
            attendance._idpet = att.idpet;
            return attendance;
        }
        public void Conclude()
        {
            if (_status != "Andamento")
                throw new Exception("Não é possivel concluir esse atendimento");
            _status = "Concluido";
        }
        public void addFile(FileAttachment file)
        {
            file.id = Guid.NewGuid().ToString("N");
            file.category = "Exames";
            file.idattendance = _idAttendance;
            anexos.Add(file);
        }
    }
    public class FileAttachment
    {
        public string id { get; set; }
        public string idattendance { get; set; }
        public string category { get; set; }
        public string FileName { get; set; }
        public string FileBase64 { get; set; }
    }
}
