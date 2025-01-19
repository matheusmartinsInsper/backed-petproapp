namespace app.Domain.DTO.Payment
{
    public class PaymentDTO
    {
        public float value { get; set; }
        public float discount { get; set; }
        public string idos { get; set; }
        public string paymentMethod { get; set; }
        public int plots { get; set; }
        public string status { get; set; }
    }
}
