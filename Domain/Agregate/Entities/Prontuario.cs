using app.Domain.DTO.Prontuario;

namespace app.Domain.Agregate.Entities
{
    public class Prontuario
    {
        private string _idprontuario;
        private List<string> _idsorderservices;
        private List<string> _idsattendance;
        private string _idtutor;
        private string _idpet;
        private string _idowner;
        private DateTime _createDate;
        public string idprontuario { get { return _idprontuario; } }
        public List<string> idsorderservices { get { return _idsorderservices; } }
        public List<string> idsattendance { get { return _idsattendance; } }
        public string idowner { get { return _idowner; } }
        public string idtutor { get { return _idtutor; } }
        public string idpet { get { return _idpet; } }
        public DateTime createDate { get { return _createDate; } }
        private Prontuario() { }
        public static Prontuario create(ProntuarioDTO prontuariodto)
        {
            Prontuario prontuario = new Prontuario();
            prontuario._idprontuario = Guid.NewGuid().ToString("N");
            prontuario._createDate = DateTime.Now;
            prontuario._idowner = prontuariodto.idowner;
            prontuario._idtutor = prontuariodto.idtutor;
            prontuario._idpet = prontuariodto.idpet;
            prontuario._idsattendance = new List<string>();
            prontuario._idsorderservices = new List<string>();
            return prontuario;
        }
        public static Prontuario restore(ProntuarioDbDTO prontuariodto,List<IdOsDb> idos, List<IdAttendanceDB> idattendance)
        {
            Prontuario prontuario = new Prontuario();
            prontuario._idprontuario = prontuariodto.idprontuario;
            prontuario._createDate = prontuariodto.datecreate;
            prontuario._idowner = prontuariodto.idowner;
            prontuario._idtutor = prontuariodto.idtutor;
            prontuario._idpet = prontuariodto.idpet;
            prontuario._idsorderservices = prontuario.convertToStringOs(idos);
            prontuario._idsattendance = prontuario.convertToStringAttedance(idattendance);
            return prontuario;
        }
        private List<string> convertToStringOs(List<IdOsDb> idos)
        {
            List<string> result = new List<string>();
            foreach (IdOsDb idosDb in idos)
            { 
                result.Add(idosDb.idos);
            }
            return result;
        }
        private List<string> convertToStringAttedance(List<IdAttendanceDB> idsatt)
        {
            List<string> result = new List<string>();
            foreach (IdAttendanceDB idatt in idsatt)
            {
                result.Add(idatt.idattendance);
            }
            return result;
        }
        public void addorderservice(string idos)
        {
            if (_idsorderservices.Contains(idos))
                throw new Exception("Essa solicitação já está adicionada ao prontuario");
            _idsorderservices.Add(idos);
        }
        public void addattendance(string idattendance)
        {
            if (_idsattendance.Contains(idattendance))
                throw new Exception("Esse atendimento já está adicionada ao prontuario");
            _idsattendance.Add(idattendance);
        }
    }
}
