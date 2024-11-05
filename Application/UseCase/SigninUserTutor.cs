using app.Application.IAuth;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Application.UseCase
{
    public class SigninUserTutor
    {
        private IRepositoryUserTutor _repository;
        private ITokenService _tokenService;
        public SigninUserTutor(IRepositoryUserTutor repository,ITokenService tokenservice)
        {
            _repository = repository;
            _tokenService = tokenservice;
        }
        public async Task<string> execute(string email,string password)
        {
            User user = await _repository.getByEmail(email);
            if(user.id == null||user.password != password)
                throw new Exception("Incorrect user data");
            string token = _tokenService.GenerateToken(user);
            return token;
        }
    }
}
