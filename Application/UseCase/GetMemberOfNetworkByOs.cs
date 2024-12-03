using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.User;

namespace app.Application.UseCase
{
    public class GetMemberOfNetworkByOs
    {
        private IRepositoryNetWork _reponet;
        private IRepositoryOrderService _repoos;
        private IRepositoryUserClinic _repoclinic;
        public GetMemberOfNetworkByOs(IRepositoryNetWork reponet, IRepositoryOrderService repoos, IRepositoryUserClinic repoclinic)
        {
            _reponet = reponet;
            _repoos = repoos;
            _repoclinic = repoclinic;
        }
        public async Task<List<UserBaseDTO>> execute(string idOs)
        {
            OrderService os = await _repoos.get(idOs);
            List<UserBaseDTO> members = await _reponet.getUsers(os.iduserrecipinet);
            return members;
        }
    }
}
