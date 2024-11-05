namespace app.Domain.DTO.Payment
{
    public class PaymentAttendanceDTO
    {
        public string typeofpayment {  get; set; }
        public string percentofdiscont { get; set; }
        public string price { get; set; }
        public string status { get; set; }
        public DateTime date {  get; set; }
    }
}
