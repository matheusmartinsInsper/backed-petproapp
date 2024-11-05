using app.Domain.DTO.OrderService;
using app.Domain.DTO.Pet;
using app.Domain.DTO.User;

namespace app.Application.DTO
{
    public class InputOSWithoutUserTutor
    {
        public UserTutor user {  get; set; }
        public PetDTO pet { get; set; }
        public OrderServiceDTOWithoutUser order { get; set; }
    }
    public class OrderServiceDTOWithoutUser
    {
        public string idservice { get; set; }
        public string priority { get; set; }
        public List<string> idsubcategories { get; set; }
        public List<string> idvaccines { get; set; }
        public string? comments { get; set; }
        public DateTime dateappointed { get; set; }
        public string attendancemodel { get; set; }
    }
}
