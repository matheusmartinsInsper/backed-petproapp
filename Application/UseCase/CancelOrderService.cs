using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Application.UseCase
{
    public class CancelOrderService
    {
        private IRepositoryOrderService _repository;
        private IRepositoryNetWork _repositorynetwork;
        public CancelOrderService(IRepositoryOrderService repository, IRepositoryNetWork repositorynetwork)
        {
            _repository = repository;
            _repositorynetwork = repositorynetwork;
        }
        public async Task execute(string iduser, string idservice)
        {
            OrderService order = await _repository.get(idservice);
            if (order.iduserrecipinet != iduser&&order.idUserTutor != iduser)
            {
                NetWorkCollaborators collaborators = await _repositorynetwork.getByUser(order.iduserrecipinet);
                if (!collaborators.conttainsCollaborator(iduser))
                {
                    throw new Exception("this user cannot cancel this order");
                }
            }
            order.cancelledOrder(iduser);
            await _repository.update(order);
            return;
        }
    }
}
