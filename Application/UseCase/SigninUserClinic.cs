using app.Application.IAuth;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.User;

namespace app.Application.UseCase
{
    public class SigninUserClinic
    {
        private IRepositoryUserClinic _repository;
        private ITokenService _tokenService;
        public SigninUserClinic(IRepositoryUserClinic repository, ITokenService tokenservice)
        {
            _repository = repository;
            _tokenService = tokenservice;
        }
        public async Task<OutPutSignin> execute(string email, string password)
        {
            User user = await _repository.getByEmail(email);
            if (user.id == null || user.password != password)
                throw new Exception("Incorrect user data");
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
