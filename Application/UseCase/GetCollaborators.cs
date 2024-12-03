using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Application.UseCase
{
    public class GetCollaborators
    {
        private IRepositoryUserCollaborator _repocollaborator;
        private IRepositoryUserClinic _repoclinic;
        public GetCollaborators(IRepositoryUserClinic repoclinic,IRepositoryUserCollaborator repocollaborator)
        {
            _repocollaborator = repocollaborator;
            _repoclinic = repoclinic;
        }
        public async Task<List<CollaboratorDTO>> execute(string iduser)
        {
            List<CollaboratorDTO> collaboratorDTOs = new List<CollaboratorDTO>();
            User user = await _repoclinic.get(iduser);
            if (user.categoryCode != "Clinic")
                throw new Exception("Usuario sem permissão para acessar esse recurso");
            List<User> collaborators = await _repocollaborator.getcollaborators(iduser);
            foreach(User collaborator in collaborators)
            {
                CollaboratorDTO collab = new CollaboratorDTO();
                collab.CRMV = collaborator.crmv;
                collab.Name = collaborator.name;
                collab.Email = collaborator.email;
                collab.CPF = collaborator.cpf;
                collab.DataDeNascimento = collaborator.dateborn;
                collab.Status = "Ativo";
                collab.Phone = collaborator.Fone != null ? $"+{collaborator.Fone.countrycode}{collaborator.Fone.areacode}{collaborator.Fone.phone}" : null;
                collaboratorDTOs.Add(collab);
            }
            return collaboratorDTOs;
        }
    }
}
