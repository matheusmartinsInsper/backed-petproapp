using app.Domain.Agregate.ObjectValues;
using app.Domain.DTO.OrderService;
using app.Domain.DTO.Service;
using System.Linq;

namespace app.Domain.Agregate.Entities
{
    public class OrderService
    {
        private bool _waaccepted;
        private bool _waspaid;
        private string _idOrderService;
        public string idService;
        private List<string> _idsubcategories;
        private List<string> _idvaccines;
        public string idUserTutor;
        private string _idUserRecipient;
        private string _idUserAccepted;
        private string _iduserAttendance;
        private string _attendanceModel;
        private DateTime _dateOfSolicitation;
        private DateTime _dateAppointed;
        private TimeSpan _differenceTimeSinceRequeset;
        private bool _userTutorCancell;
        private string _priority;
        private string? _timeSinceRequest;
        private string _status;
        private string _idattendance;
        public string? comments;
        public string idPet;
        public bool waspaid { get { return _waspaid; } }
        public string status { get { return _status; } }
        public string idorderservice {  get { return _idOrderService; } }
        public string iduseraccepted {  get { return _idUserAccepted; } }
        public string iduserrecipinet { get { return _idUserRecipient; } }
        public string iduserattendance { get { return _iduserAttendance; } }
        public DateTime dateofsolicitation {  get { return _dateOfSolicitation; } }
        public DateTime dateappointed { get { return _dateAppointed; } }
        public List<string> idsubservices { get { return _idsubcategories; } }
        public List<string> idvaccines { get { return _idvaccines; } }
        public string attendance { get { return _attendanceModel; } }
        public bool wasaccepted { get { return _waaccepted; } }
        public bool tutorcancelled { get { return _userTutorCancell; } }
        public string priority { get { return _priority; } }
        public string idattedance { get { return _idattendance; } }
        
        private OrderService() { }
        public static OrderService create(OrderServiceDTO os)
        {
            OrderService order = new OrderService();
            order._idOrderService = Guid.NewGuid().ToString("N");
            order._status = "Pendente";
            order.comments = os.comments;
            order.idUserTutor = os.idusertutor;
            order._idUserRecipient = os.iduserrecipient;
            order._idUserAccepted = os.iduseraccepted;
            order._iduserAttendance = os.iduserrecipient;
            order.idPet = os.idpet;
            order._dateOfSolicitation = DateTime.Now;
            order._dateAppointed = os.dateappointed.Day<DateTime.Now.Day&&os.dateappointed.Month==DateTime.Now.Month
                                   ||os.dateappointed.Day==DateTime.Now.Day
                                   &&os.dateappointed.Hour<DateTime.Now.Hour
                                   ? throw new Exception("Date invalid"):os.dateappointed;
            order.idService = os.idservice;
            order._idsubcategories = os.idsubcategories;
            order._idvaccines = os.idvaccines;
            order._attendanceModel = os.attendancemodel;
            order._idattendance = "not attendance";
            order._userTutorCancell = false;
            order._waaccepted = false;
            order._waspaid = false;
            order._priority = order.typesofpriotity.Contains(os.priority)==true?os.priority:throw new Exception("Essa prioridade não é valida");
            return order;
        }
        public static OrderService restore(OrderServiceDbDTO os,List<VaccinesOsDB> vaccines, List<SubcategoryOSDb> subcategories)
        {
            OrderService order = new OrderService();
            order.idPet = os.idpet;
            order.comments = os.comments;
            order._status = os.status;
            order._idOrderService = os.idorderservice;
            order._idUserRecipient = os.iduserrecipient;
            order._iduserAttendance = os.iduserattendance;
            order.idUserTutor = os.idusertutor;
            order.idService=os.idservice;
            order._idUserAccepted = os.iduseraccepted;
            order._dateOfSolicitation = os.dateofsolicitation;
            order._dateAppointed = os.dateappointed;
            TimeSpan differencetime = DateTime.Now - order._dateOfSolicitation;
            order._differenceTimeSinceRequeset = differencetime;
            order._timeSinceRequest = $"Dias: {differencetime.Days} / Horas {differencetime.Hours}";
            order._idsubcategories = order.converteToSubcategories(subcategories);
            order._idvaccines = order.converteToVaccines(vaccines);
            order._attendanceModel = os.attendencemodel;
            order._waaccepted = os.wasaccepted;
            order._userTutorCancell = os.tutorcancell;
            order._priority = os.priority;
            order._idattendance = os.idattendance;
            return order;
        }
        public void acceptOrder(string iduser,string iduserattendance)
        {
            if (_status == "Cancelado" || _status == "Rejeitado" || _status == "Confirmado")
            {
                throw new Exception("This order cannot be accepted");
            }else if (DateTime.Now > _dateAppointed)
            {
                throw new Exception("This order is past the date of service");
            }
            _status = "Confirmado";
            _idattendance = "not attendance";
            _waaccepted = true;
            _iduserAttendance = iduserattendance;
            _idUserAccepted = iduser;
        }
        public void rejectOrder(string iduser)
        {
            if (_status != "Pendente")
            {
                throw new Exception("This order cannot be rejectd");
            }
            _status = "Rejeitado";
            _idattendance = "not attendance";
            _waaccepted = false;
            _idUserAccepted = iduser;
        }
        public void cancelledOrder(string iduser)
        {
            if (_status == "Confirmado"||_status == "Pendente")
            {
                _status = "Cancelado";
                _idattendance = "not attendance";
                if (iduser == idUserTutor)
                {
                    _userTutorCancell = true;
                    return;
                }
                return;
            }
            throw new Exception("This order cannot be cancelled or is already cancelled");
           
        }
        public void ConcluirOrder()
        {
            if (_status != "Andamento")
            {
                throw new Exception("This order cannot be finished");
            }
            _status = "Concluido";
        }
        public void StartOrder(string idattendance)
        {
            if (_status != "Confirmado")
            {
                throw new Exception("This order cannot be started");
            }
            _status = "Andamento";
            _idattendance = idattendance;
        }
        public void ConfirmPayment()
        {
            if(_waspaid==true)
                throw new Exception("Esse Serviço ja foi pago");
            _waspaid = true;
        }
        private List<string> converteToVaccines(List<VaccinesOsDB> vaccines)
        {
            List<string> result = new List<string>();
            foreach (VaccinesOsDB vaccine in vaccines)
            {
                result.Add(vaccine.idvaccine);
            }
            return result;
        }
        private List<string> converteToSubcategories(List<SubcategoryOSDb> subcategories)
        {
            List<string> result = new List<string>();
            foreach (SubcategoryOSDb subcategory in subcategories)
            {
                result.Add(subcategory.idsubcategory);
            }
            return result;
        }
        private List<string> typesofpriotity = new List<string>()
        {
            "Não urgente",
            "Pouco urgente",
            "Urgente",
            "Muito urgente",
            "Emergencia"
        };
    }
}
