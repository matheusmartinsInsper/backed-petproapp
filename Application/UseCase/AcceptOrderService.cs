using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Application.UseCase
{
    public class AcceptOrderService
    {
        private IRepositoryOrderService _repository;
        private IRepositoryNetWork _repositorynetwork;
        private IRepositoryUserCollaborator _repocollaborator;
        private IRepositoryUserClinic _repoclinic;
        public AcceptOrderService(IRepositoryOrderService repository, IRepositoryNetWork repositorynetwork,IRepositoryUserCollaborator repocollaborator,IRepositoryUserClinic repoclinic)
        {
            _repository = repository;
            _repositorynetwork = repositorynetwork;
            _repocollaborator = repocollaborator;
            _repoclinic = repoclinic;
        }
        public async Task execute(string iduser, string idos, string emailuserattendance)
        {
            OrderService order = await _repository.get(idos);
            User collaborator;
            if (order.iduserrecipinet != iduser)
            {
                NetWorkCollaborators collaborators = await _repositorynetwork.getByUser(order.iduserrecipinet);
                if (!collaborators.conttainsCollaborator(iduser))
                {
                    throw new Exception("Usuario sem permissão para aceitar essa ordem de serviço");
                }
                order.acceptOrder(iduser, iduser);
                await _repository.update(order);
                return;
            }
            User clinic = await _repoclinic.get(iduser);
            if (clinic.categoryCode == "Clinic")
            {
                NetWorkCollaborators mycollaborators = await _repositorynetwork.getByUser(iduser);
                if (emailuserattendance != clinic.email)
                {
                    collaborator = await _repocollaborator.getByEmail(emailuserattendance);
                    if (!mycollaborators.conttainsCollaborator(collaborator.id))
                    {
                        throw new Exception("Esse usuario nao pertence a sua rede e não pode atribuir atendimentos a ele");
                    }
                    order.acceptOrder(iduser, collaborator.id);
                    await _repository.update(order);
                    return;
                }
            }
            order.acceptOrder(iduser,iduser);
            await _repository.update(order);
            return;

        }
    }
}
