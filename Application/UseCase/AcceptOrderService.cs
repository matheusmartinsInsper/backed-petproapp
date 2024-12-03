using app.Application.GatewayService.WppAPI;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Application.UseCase
{
    public class AcceptOrderService
    {
        private IRepositoryOrderService _repository;
        private IRepositoryService _reposervice;
        private IRepositoryNetWork _repositorynetwork;
        private IRepositoryUserCollaborator _repocollaborator;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryUserTutor _repouserTutor;
        private IMessage _message;
        public AcceptOrderService(IMessage message, IRepositoryUserTutor repotutor, IRepositoryService reposervice, IRepositoryOrderService repository, IRepositoryNetWork repositorynetwork,IRepositoryUserCollaborator repocollaborator,IRepositoryUserClinic repoclinic)
        {
            _repository = repository;
            _repositorynetwork = repositorynetwork;
            _repocollaborator = repocollaborator;
            _repoclinic = repoclinic;
            _reposervice = reposervice;
            _repouserTutor = repotutor;
            _message = message;
        }
        public async Task execute(string iduser, string idos, string emailuserattendance)
        {
            OrderService order = await _repository.get(idos);
            Service service = await _reposervice.get(order.idService);
            User tutor = await _repouserTutor.get(order.idUserTutor);
            User collaborator;
            if (order.iduserrecipinet != iduser)
            {
                NetWorkCollaborators collaborators = await _repositorynetwork.getByUser(order.iduserrecipinet);
                User CollaboratorOwner = await _repocollaborator.get(iduser);
                if (!collaborators.conttainsCollaborator(iduser))
                {
                    throw new Exception("Usuario sem permissão para aceitar essa ordem de serviço");
                }
                order.acceptOrder(iduser, iduser);
                await _repository.update(order);
                string message = $"Olá Tutor, seu serviço para o dia {order.dateappointed} foi confirmado pela {CollaboratorOwner.name}";
                await _message.sendMessage(message, "5574999090074");
                return;
            }
            User clinic = await _repoclinic.getUserBase(iduser);
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
                    string message = $"Olá tutor {tutor.name}, seu serviço de {service.nameCategory} para o dia " +
                        $"{order.dateappointed.ToString("dd/MM/yyyy")} as {order.dateappointed.ToString("HH:mm")} foi confirmado pela {collaborator.name}";
                    await _message.sendMessage(message, "5574999090074");
                    return;
                }
            }
            order.acceptOrder(iduser,iduser);
            await _repository.update(order);
            string message2 = $"Olá Tutor {tutor.name}, seu serviço de {service.nameCategory} para o dia " +
                        $"{order.dateappointed.ToString("dd/MM/yyyy")} as {order.dateappointed.ToString("HH:mm")} foi confirmado pela {clinic.name}";
            await _message.sendMessage(message2, "5574999090074");
            return;

        }
    }
}
