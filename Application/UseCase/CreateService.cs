using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Service;

namespace app.Application.UseCase
{
    public class CreateService
    {
        private IRepositoryService _repository;
        private IRepositoryUserClinic _repoclinic;
        public CreateService(IRepositoryService repository,IRepositoryUserClinic repoclinic)
        {
            _repository = repository;
            _repoclinic = repoclinic;
        }
        public async Task<Service> execute(ServiceDTO service)
        {
            Service myservice = Service.create(service);
            User user = await _repoclinic.get(myservice.idUser);
            if (user.categoryCode == "Clinic" || user.categoryCode == "Collaborator")
            {
                await _repository.save(myservice);
                return myservice;
            }
            throw new Exception("This user does not have permission to create a service");
            
        }
    }
}
