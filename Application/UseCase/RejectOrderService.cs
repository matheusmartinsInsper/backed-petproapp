using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.OrderService;

namespace app.Application.UseCase
{
    public class RejectOrderService
    {
        private IRepositoryOrderService _repository;
        private IRepositoryNetWork _repositorynetwork;
        public RejectOrderService(IRepositoryOrderService repository, IRepositoryNetWork repositorynetwork)
        {
            _repository = repository;
            _repositorynetwork = repositorynetwork;
        }
        public async Task execute(string iduser,string idservice)
        {
            OrderService order = await _repository.get(idservice);
            if (order.iduserrecipinet != iduser)
            {
                NetWorkCollaborators collaborators = await _repositorynetwork.getByUser(order.iduserrecipinet);
                if (!collaborators.conttainsCollaborator(iduser))
                {
                    throw new Exception("this user cannot reject this order");
                }
            }
            order.rejectOrder(iduser);
            await _repository.update(order);
            return;
        }
    }
}
