using app.Application.IAuth;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.User;

namespace app.Application.UseCase
{
    public class SigninUserPlatform
    {
        private IRepositoryUserClinic _repository;
        private IRepositoryUserCollaborator _repocollaborator;
        private ITokenService _tokenService;
        public SigninUserPlatform(IRepositoryUserClinic repository, ITokenService tokenservice,IRepositoryUserCollaborator repocollaborator)
        {
            _repository = repository;
            _tokenService = tokenservice;
            _repocollaborator = repocollaborator;
        }
        public async Task<OutPutSignin> execute(string email, string password)
        {
            User user = await _repository.getUserBaseByEmail(email);
            if (user.id == null || user.password != password)
                throw new Exception("Incorrect user data");
            if (user.categoryCode == "Clinic")
            {
                user = await _repository.getByEmail(email);
            }
            else if (user.categoryCode == "Collaborator")
            {
                user = await _repocollaborator.getByEmail(email);
            }else
            {
                throw new Exception("Categoria de usuario nao encontrada");
            }
            string token = _tokenService.GenerateToken(user);
            OutPutSignin output = new OutPutSignin();
            output.nameuser = user.name;
            output.id = user.id;
            output.email = user.email;
            output.typeuser = user.categoryCode;
            output.token = token;
            return output;
        }
    }
}
