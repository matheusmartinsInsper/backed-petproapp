using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.User;

namespace app.Application.UseCase
{
    public class GetUserTutor
    {
        private IRepositoryUserTutor _repository;
        public GetUserTutor(IRepositoryUserTutor repository)
        {
            _repository = repository;
        }
        public async Task<User> execute(string iduser)
        {
            User user = await _repository.get(iduser);
            if (user.id == null)
                throw new Exception("this user not exist");
            return user;
        }
    }
}
