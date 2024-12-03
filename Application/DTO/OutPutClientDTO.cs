using app.Domain.DTO.Fone;

namespace app.Application.DTO
{
    public class OutPutClientDTO
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public DateTime dateadd { get; set; }
        public DateTime dateborn { get; set; }
        public List<PetOutput> pets { get; set; }
    }

}
