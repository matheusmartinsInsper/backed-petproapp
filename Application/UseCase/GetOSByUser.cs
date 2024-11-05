using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DomainService;
using app.Domain.DTO.Pet;
using app.Domain.DTO.User;

namespace app.Application.UseCase
{
    public class GetOSByUser
    {

        private IRepositoryOrderService _repoorder;
        private IRepositoryService _reposervice;
        private IRepositoryPet _repopet;
        private IRepositoryUserTutor _repotutor;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryUserCollaborator _repocollaborator;
        public GetOSByUser(IRepositoryUserCollaborator repocollaborator, IRepositoryUserClinic repoclinic, IRepositoryOrderService repoorder, IRepositoryService reposervice,IRepositoryPet repopet,IRepositoryUserTutor repotutor)
        {
            _repoorder = repoorder;
            _reposervice = reposervice;
            _repopet = repopet;
            _repotutor = repotutor;
            _repoclinic = repoclinic;
            _repocollaborator = repocollaborator;
        }
        public async Task<List<OrderServiceOutputDTO>> execute(string iduser,bool wasaccepted)
        {
            List<OrderService> orders = await _repoorder.getByUser(iduser, wasaccepted);
            List<OrderService> orderswithoutreject = orders.Where(x => x.status != "Rejeitado").ToList();
            List<OrderServiceOutputDTO> outputs = new List<OrderServiceOutputDTO>();
            foreach (OrderService order in orderswithoutreject)
            {
                OrderServiceOutputDTO outputDTO = new OrderServiceOutputDTO
                {
                    pet = new PetOS(), // Inicialize os objetos aninhados
                    tutor = new Tutor()
                };
                Pet pet = await _repopet.get(order.idPet);
                User tutor = await _repotutor.get(order.idUserTutor);
                Service service = await _reposervice.get(order.idService);
                User collab = await _repocollaborator.get(order.iduserrecipinet);
                User clinic = await _repoclinic.get(order.iduserrecipinet);
                outputDTO.nameprofissional = clinic.name;
                outputDTO.nameuserowner = clinic.name;
                if (clinic.id != order.iduserattendance)
                {
                    User collaborator = await _repocollaborator.get(order.iduserattendance);
                    outputDTO.nameprofissional = collaborator.name;
                }
                else
                {
                    outputDTO.nameprofissional = clinic.name;
                }
                if (collab != null)
                {
                    outputDTO.nameprofissional = collab.name;
                    outputDTO.nameuserowner = collab.name;
                }
                outputDTO.priority = order.priority;
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
                outputDTO.pet.castrated = pet.Castrated;
                outputDTO.tutor.email = tutor.email;
                outputDTO.tutor.name = tutor.name;
                outputDTO.idattendance = order.idattedance;
                SumPriceOS sum = new SumPriceOS(service, outputDTO.subcategories, outputDTO.vaccines);
                outputDTO.price = sum.sum();
                outputs.Add(outputDTO);
            }
            return outputs;
        }
    }
}
