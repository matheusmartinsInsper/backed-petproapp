using app.Domain.DTO.Payment;

namespace app.Domain.Agregate.Entities
{
    public class Payment
    {
        private DateTime _datecreate;
        private DateTime _dateupdate;
        private string _status;
        private int _plots;
        private string _paymentmethod;
        private string _idpayment;
        private string _idos;
        private string _iduser;
        private float _value;
        private float _discount;
        private Payment() { }
        public static Payment create(PaymentDTO payment,string iduser)
        {
            Payment pay = new Payment();
            pay._datecreate = DateTime.Now;
            pay._dateupdate = DateTime.Now;
            pay._value = payment.value;
            pay._plots = payment.plots;
            pay._discount = payment.discount;
            pay._paymentmethod = payment.paymentMethod;
            pay._status = pay.paymentstatus.Where((x) => x == payment.status).ToList().Count() != 0 ? payment.status : throw new Exception("Status invalido");
            pay._iduser = iduser;
            pay._idos = payment.idos;
            pay._idpayment = Guid.NewGuid().ToString("N");
            return pay;
        }
        public static Payment restore(PaymentDTO payment)
        {
            Payment pay = new Payment();
            return pay;
        }
        private List<string> paymentstatus = new List<string>()
        {
            "Confirmado",
            "Pendente",
            "Cancelado",
            "Deletado"
        };
    }
}
