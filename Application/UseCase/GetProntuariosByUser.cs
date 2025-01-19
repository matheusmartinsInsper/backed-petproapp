using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DomainService;
using System.ComponentModel;

namespace app.Application.UseCase
{
    public class GetProntuariosByUser
    {
        private IRepositoryOrderService _repository;
        private IRepositoryService _reposervice;
        private IRepositoryUserTutor _repouser;
        private IRepositoryPortfolioClient _repoport;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryPet _repopet;
        private IRepositoryProntuario _repoprontuario;
        private IRepositoryAttendance _repoattendance;
        public GetProntuariosByUser(IRepositoryAttendance repoattendance, IRepositoryProntuario repoprontuario, IRepositoryPet repopet, IRepositoryUserClinic repoclinic, IRepositoryOrderService repository, IRepositoryService reposervice, IRepositoryUserTutor repouser, IRepositoryPortfolioClient repoport)
        {
            _repository = repository;
            _reposervice = reposervice;
            _repouser = repouser;
            _repoport = repoport;
            _repoclinic = repoclinic;
            _repopet = repopet;
            _repoprontuario = repoprontuario;
            _repoattendance = repoattendance;
        }
        public async Task<List<OutPutProntuariosDTO>> execute(string idowner)
        {
            List<OutPutProntuariosDTO> output = new List<OutPutProntuariosDTO>();
            List<Prontuario> prontuarios = await _repoprontuario.getByIdOwner(idowner);
            if(prontuarios==null)
                return output;
            foreach(Prontuario prontuario in prontuarios)
            {
                OutPutProntuariosDTO dto = new OutPutProntuariosDTO()
                {
                    pet = new PetOS(),
                    tutor = new Tutor()
                };
                Pet pet = await _repopet.get(prontuario.idpet);
                User tutor = await _repouser.get(prontuario.idtutor);
                dto.idprontuario = prontuario.idprontuario;
                dto.idsattendance = prontuario.idsattendance;
                dto.idsorder = prontuario.idsorderservices;
                dto.datecreate = prontuario.createDate;
                dto.pet.petname = pet.PetName;
                dto.pet.age = pet.Age;
                dto.pet.castrated = pet.Castrated;
                dto.pet.race = pet.Race;
                dto.pet.species = pet.Species;
                dto.pet.weight = pet.Weight;
                dto.pet.castrated = pet.Castrated;
                dto.pet.sex = pet.Sex;
                dto.pet.contraindications = pet.contraindications;
                dto.tutor.email = tutor.email;
                dto.tutor.name = tutor.name;
                dto.tutor.phone = tutor.Fone != null ? $"+{tutor.Fone.countrycode}{tutor.Fone.areacode}{tutor.Fone.phone}" : null;
                output.Add(dto);
            }
            return output;
        }
    }
}
