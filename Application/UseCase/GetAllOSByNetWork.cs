using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DomainService;

namespace app.Application.UseCase
{
    public class GetAllOSByNetWork
    {

        private IRepositoryOrderService _repoorder;
        private IRepositoryService _reposervice;
        private IRepositoryPet _repopet;
        private IRepositoryUserTutor _repotutor;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryUserCollaborator _repocollaborator;
        public GetAllOSByNetWork(IRepositoryUserCollaborator repocollaborator, IRepositoryUserClinic repoclinic, IRepositoryOrderService repoorder, IRepositoryService reposervice, IRepositoryPet repopet, IRepositoryUserTutor repotutor)
        {
            _repoorder = repoorder;
            _reposervice = reposervice;
            _repopet = repopet;
            _repotutor = repotutor;
            _repoclinic = repoclinic;
            _repocollaborator = repocollaborator;
        }
        public async Task<List<OrderServiceOutputDTO>> execute(string iduser)
        {
            List<OrderService> ordersaccepted = await _repoorder.getByNetWork(iduser, true);
            List<OrderService> ordersnotaccepted = await _repoorder.getByNetWork(iduser, false);
            List<OrderService> allOrders = new List<OrderService>();
            allOrders.AddRange(ordersaccepted);
            allOrders.AddRange(ordersnotaccepted);
            List<OrderServiceOutputDTO> outputs = new List<OrderServiceOutputDTO>();
            foreach (OrderService order in allOrders)
            {
                OrderServiceOutputDTO outputDTO = new OrderServiceOutputDTO
                {
                    pet = new PetOS(), // Inicialize os objetos aninhados
                    tutor = new Tutor()
                };
                Pet pet = await _repopet.get(order.idPet);
                User tutor = await _repotutor.get(order.idUserTutor);
                User user = await _repoclinic.get(order.iduserrecipinet);
                if (user.id != order.iduserattendance)
                {
                    User collaborator = await _repocollaborator.get(order.iduserattendance);
                    outputDTO.nameprofissional = collaborator.name;
                }
                else
                {
                    outputDTO.nameprofissional = user.name;
                }
                Service service = await _reposervice.get(order.idService);
                outputDTO.priority = order.priority;
                outputDTO.nameuserowner = user.name;
                outputDTO.vaccines = service.selectvaccines(order.idvaccines);
                outputDTO.subcategories = service.subcategoriesOfOs(order.idsubservices);
                outputDTO.waaccepted = order.wasaccepted;
                outputDTO.tutorcancelled = order.tutorcancelled;
                outputDTO.attendancemodel = order.attendance;
                outputDTO.idservice = order.idService;
                outputDTO.comments = order.comments;
                outputDTO.categoryname = service.nameCategory;
                outputDTO.title = service.titleService;
                outputDTO.dateappointed = order.dateappointed;
                outputDTO.datesolicitation = order.dateofsolicitation;
                outputDTO.status = order.status;
                outputDTO.idorderservice = order.idorderservice;
                outputDTO.pet.petname = pet.PetName;
                outputDTO.pet.age = pet.Age;
                outputDTO.pet.castrated = pet.Castrated;
                outputDTO.pet.race = pet.Race;
                outputDTO.pet.species = pet.Species;
                outputDTO.pet.weight = pet.Weight;
                outputDTO.tutor.email = tutor.email;
                outputDTO.tutor.name = tutor.name;
                SumPriceOS sum = new SumPriceOS(service, outputDTO.subcategories, outputDTO.vaccines);
                outputDTO.price = sum.sum();
                outputs.Add(outputDTO);
            }
            return outputs;
        }
    }
}
